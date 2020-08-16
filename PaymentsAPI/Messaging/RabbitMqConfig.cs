using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentsAPI.Messaging
{
    public class RabbitMqConfig
    {
        public string Hostname { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
