using System;
using System.Collections.Generic;
using System.Text;

namespace SW.Smarten.SampleWeb.Model
{
    class TraceTypeInfo
    {
        public string Id { get; set; }

        public string Description { get; set; }
        public string PublicDescription { get; set; }
        public bool IsFinal { get; set; }
        public bool IsCritical { get; set; }
        public bool IsDiscrepancy { get; set; }
        public bool IsDelivery { get; set; }
        public bool IsPublic { get; set; }
    }
}
