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
        public ActionResult Create(BotApplication botApplication)
        {
            TempData["AuthorizationToken"] = botApplication.BotAuthorizationTokenApi;
            ViewBag.BotId = botApplication.BotId;
            return View();
        }


        /// <summary>
        /// Method to Get all distribution list 
        /// </summary>
        /// <param name="botAuthorizationTokenApi"></param>
        /// <returns></returns>
        private async Task ShowDistributionListAsync(string botAuthorizationTokenApi)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", botAuthorizationTokenApi);

                Lists blipList = new Lists
                {
                    id = Guid.NewGuid().ToString(),
                    to = "postmaster@broadcast.msging.net",
                    method = OperationType.get,
                    type = "application/vnd.iris.distribution-list+json",
                    uri = "/lists",
                    //resource = new Resource()
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
            BotApplication botApplication = db.BotApplications.Where(x => x.BotId == collection.BotId).FirstOrDefault();
            collection.BotApplication = botApplication;

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
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        private async Task SendMessage(Broadcast collection)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", TempData["AuthorizationToken"].ToString());

                ScheduledMessage scheduledMessage = GenerateScheduledMessage(collection);

                var stringContent = new StringContent(JsonConvert.SerializeObject(scheduledMessage), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync($"{ConfigurationManager.AppSettings["BaseBliPUrl"]}/commands", stringContent);

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        private ScheduledMessage GenerateScheduledMessage(Broadcast collection)
        {
            ScheduledMessage scheduledMessage = new ScheduledMessage
            {
                resource = new Resource
                {
                    message = new Message
                    {
                        id = Guid.NewGuid().ToString(),
                        to = collection.DistributionList,
                        type = collection.ContentType,
                        content = JsonConvert.DeserializeObject(collection.Content)
                    },
                    when = DateTime.Parse(DateTime.Now.AddMinutes(3).AddHours(3).GetDateTimeFormats()[114])
                }
            };

            return scheduledMessage;
        }
    }
}
