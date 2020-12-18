using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SW.Smarten.SampleWeb.Model
{
    public class TraceTypeTranslations : IEntityTranslation 
    {
        public string Description { get; set; }
        public string PublicDescription { get; set; }
        public string Language { get; set; }
    }
}
