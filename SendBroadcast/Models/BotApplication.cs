using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SendBroadcast.Models
{
    public class BotApplication
    {
        [Key]
        public int BotId { get; set; }
        [Display(Name = "Nome do Bot")]
        public string BotName { get; set; }
        [Display(Name = "Identificador")]
        public string BotIdentifier { get; set; }
        [Display(Name = "Token de Acesso")]
        public string BotAccessToken { get; set; }
        [Display(Name ="Token para Autenticação na API")]
        public string BotAuthorizationTokenApi { get; set; }
    }
}