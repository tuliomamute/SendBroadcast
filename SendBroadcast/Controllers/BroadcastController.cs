using Newtonsoft.Json;
using SendBroadcast.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SendBroadcast.Controllers
{
    public class BroadcastController : Controller
    {
        private SendBroadcastContext db = new SendBroadcastContext();

        // GET: Broadcast
        public ActionResult Index()
        {
            return View(db.BotApplications.ToList());
        }

        // GET: Broadcast/Create
        public async Task<ActionResult> Create(BotApplication botApplication)
        {
            await ShowDistributionListAsync(botApplication.BotAuthorizationTokenApi);
            ViewBag.AuthorizationToken = botApplication.BotAuthorizationTokenApi;

            return View();
        }

        private async Task ShowDistributionListAsync(string botAuthorizationTokenApi)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", botAuthorizationTokenApi);

                Lists blipList = new Lists
                {
                    id = 1,
                    to = "postmaster@broadcast.msging.net",
                    method = OperationType.get,
                    type = "application/vnd.iris.distribution-list+json",
                    uri = "/lists",
                    resource = new Resource()
                };

                var stringContent = new StringContent(JsonConvert.SerializeObject(blipList), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync($"{ConfigurationManager.AppSettings["BaseBliPUrl"]}/commands", stringContent);

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                ViewBag.DistributionList = new SelectList(new List<string>(), "ID", "DESCRICAO");
            }
        }

        // POST: Broadcast/Create
        [HttpPost]
        public async Task<ActionResult> Create(Broadcast collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Broadcasts.Add(collection);
                    db.SaveChanges();

                    await SendMessage(collection);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private async Task SendMessage(Broadcast collection)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", ViewBag.AuthorizationToken);

                Message blipList = new Message
                {
                    to = collection.DistributionList,
                    type = "text/plain",
                    content = collection.Content
                };

                var stringContent = new StringContent(JsonConvert.SerializeObject(blipList), Encoding.UTF8, "application/json");

                //HttpResponseMessage response = await client.GetAsync("/lists");
                HttpResponseMessage response = await client.PostAsync($"{ConfigurationManager.AppSettings["BaseBliPUrl"]}/message", stringContent);

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
