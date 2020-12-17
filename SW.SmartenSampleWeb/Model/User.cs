using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SW.Smarten.SampleWeb
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        //public string LastName { get; set; }

        public string LastName { get; set; }


    }
}
