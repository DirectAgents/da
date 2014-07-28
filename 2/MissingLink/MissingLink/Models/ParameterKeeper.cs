﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{

    public class ParameterKeeper
    {

        public string query { get; set; }
        public string searchString {get; set; }
        public string website { get; set; }
        public int numResults { get; set; }
        public string exclude { get; set; }
        public int jump { get; set; }
        public float delay { get; set; }

        public List<MvcApplication1.Models.ProcessHub.SearchResult> results { get; set; }
        public int omit_count { get; set; }
    }
}