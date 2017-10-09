using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SendBroadcast.Models
{
    public class Broadcast
    {
        [Key]
        public int BroadcastId { get; set; }
        [Display(Name = "Lista de Distribuição")]
        public string DistributionList { get; set; }
        [Display(Name = "Conteúdo")]
        public string Content { get; set; }
        [Display(Name = "Tipo da Mensagem")]
        public string ContentType { get; set; }
        [ForeignKey("BotId")]
        public BotApplication BotApplication { get; set; }
        public int BotId { get; set; }

    }
}