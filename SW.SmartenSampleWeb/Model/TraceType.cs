using SW.PrimitiveTypes;
using SW.Smarten.SampleWeb.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SW.Smarten.SampleWeb.Model
{
    class TraceType : IEntity<string>, IMultiLingualEntity<TraceTypeTranslations>
    {
        public TraceType()
        {
            Translations = new List<TraceTypeTranslations>();
        }

        public string Id { get; set; }



        public bool IsFinal { get; set; }
        public bool IsCritical { get; set; }
        public bool IsDiscrepancy { get; set; }
        public bool IsDelivery { get; set; }
        public bool IsPublic { get; set; }
        public ICollection<TraceTypeTranslations> Translations { get; set; }
    }
}
