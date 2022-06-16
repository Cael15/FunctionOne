using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionOne
{
    internal class ClientEntity
    {
        public ClientEntity()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
