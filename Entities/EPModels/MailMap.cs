using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class MailMap
    {
        public string Subject { get; set; }
        public string Messaje { get; set; }
        public List<string> To { get; set; }
        public List<string>? Cc { get; set; }
        public List<string>? Cco { get; set; }
        public AttachmentMap? Attachment { get; set; }
    }
}
