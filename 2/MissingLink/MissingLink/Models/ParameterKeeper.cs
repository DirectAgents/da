using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication1.Models
{

    public class ParameterKeeper
    {
        public string query { get; set; }
        public string searchString { get; set; }
        public string website { get; set; }
        public int numResults { get; set; }
        public string exclude { get; set; }
        public int jump { get; set; }
        public float delay { get; set; }

        public List<MvcApplication1.Models.ProcessHub.SearchResult> results { get; set; }
        public int omit_count { get; set; }
        public bool search_error_encountered { get; set; }
        public string search_error_msg { get; set; }
    }
}