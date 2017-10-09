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

            ViewBag.ContentTypeList = CreateContentTypeList();

            return View();
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

        #region BLiP calls

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
                };

                var stringContent = new StringContent(JsonConvert.SerializeObject(blipList), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync($"{ConfigurationManager.AppSettings["BaseBliPUrl"]}/commands", stringContent);

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                ViewBag.DistributionList = new SelectList(new List<string>(), "ID", "DESCRICAO");
            }
        }
        #endregion

        #region Object Creation

        /// <summary>
        /// Create Scheduler Message to send to Blip
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
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
                    when = DateTime.Parse(collection.DuoDate.AddHours(3).GetDateTimeFormats()[114])
                }
            };

            return scheduledMessage;
        }

        /// <summary>
        /// Create Select List
        /// </summary>
        /// <returns></returns>
        private SelectList CreateContentTypeList()
        {
            return new SelectList(
                        new List<SelectListItem>
                        {
                            new SelectListItem { Selected = true, Text = string.Empty, Value = ""},
                            new SelectListItem { Selected = false, Text = "1 - Texto", Value = "text/plain"},
                            new SelectListItem { Selected = false, Text = "2 - Link de Mídia", Value = "application/vnd.lime.media-link+json"},
                            new SelectListItem { Selected = false, Text = "3 - Link Web", Value = "application/vnd.lime.web-link+json"},
                            new SelectListItem { Selected = false, Text = "4 - Menu", Value = "application/vnd.lime.select+json"},
                            new SelectListItem { Selected = false, Text = "5 - Menu Multimídia", Value = "application/vnd.lime.document-select+json"},
                            new SelectListItem { Selected = false, Text = "6 - Coleção", Value = "application/vnd.lime.collection+json"},
                            new SelectListItem { Selected = false, Text = "7 - Lista", Value = "application/vnd.lime.list+json"},
                            new SelectListItem { Selected = false, Text = "8 - Container", Value = "application/vnd.lime.collection+json"},
                            new SelectListItem { Selected = false, Text = "9 - Localização", Value = "application/vnd.lime.location+json"},
                            new SelectListItem { Selected = false, Text = "10 - Solicitação de Pagamento", Value = "application/vnd.lime.invoice+json"},
                            new SelectListItem { Selected = false, Text = "11 - Status do Pagamento", Value = "application/vnd.lime.invoice-status+json"},
                            new SelectListItem { Selected = false, Text = "12 - Recibo do Pagamento", Value = "application/vnd.lime.payment-receipt+json"},
                            new SelectListItem { Selected = false, Text = "13 - Entrada do Usuário", Value = "application/vnd.lime.input+json"},
                            new SelectListItem { Selected = false, Text = "14 - Informação Confidencial", Value = "application/vnd.lime.sensitive+jso"},
                            new SelectListItem { Selected = false, Text = "15 - Recurso", Value = "application/vnd.iris.resource+json"},
                            new SelectListItem { Selected = false, Text = "16 - Redirecionamento", Value = "application/vnd.lime.redirect+json"},
                            new SelectListItem { Selected = false, Text = "17 - Conteúdo Nativo", Value = "application/json"},
                        }, "Value", "Text", string.Empty);
        }
        #endregion

    }
}
