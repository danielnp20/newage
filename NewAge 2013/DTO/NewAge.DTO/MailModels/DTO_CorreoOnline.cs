using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.MailModels
{
    public class DTO_CorreoOnline
    {
        public string UrlHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
