﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Newtonsoft.Json;

namespace soho.domain
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Order
    {
        public List<Envelope> envelopes { get; set; }
        public string orderId { get; set; }
        public string taobaoId { get; set; }
        public bool urgent { get; set; }

        [JsonIgnore]
        public DirectoryInfo directory { get; set; }

     
    }
}