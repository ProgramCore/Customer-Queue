using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerQueue.Models
{
    public class Customer
    {
        public Guid ID { get; private set; } = new Guid();
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
