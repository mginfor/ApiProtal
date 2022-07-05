using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class GenericResponse
    {
        public bool status { get; set; }
        public object data { get; set; }
        public string message { get; set; }


        public GenericResponse()
        {
            status = true;
        }

        public GenericResponse(bool status, object data, string message)
        {
            this.status = status;
            this.data = data;
            this.message = message;
        }
    }
}
