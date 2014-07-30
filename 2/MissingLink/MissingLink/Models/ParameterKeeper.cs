using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication1.Models
{

    public class ParameterKeeper
    {
        [Required(ErrorMessage = "Google Search Query cannot be left blank.")]
        public string query { get; set; }

        public string searchString { get; set; }
        
        [Required(ErrorMessage = "You must provide at least one website to seek out.")]
        public string website { get; set; }
        
        [Range(1, 64, ErrorMessage = "Number of results must be between 1 and 64.")]
        public int numResults { get; set; }
        
        public string exclude { get; set; }

        [Range(1, 61, ErrorMessage = "Can only jump between Rank IDs 1 and 61.")]
        public int jump { get; set; }
        
        [Range(0,60, ErrorMessage = "Delay time must be between 0.0 and 60.0 sec.")]
        public float delay { get; set; }

        public List<MvcApplication1.Models.ProcessHub.SearchResult> results { get; set; }
        public int omit_count { get; set; }
        public bool search_error_encountered { get; set; }
        public string search_error_msg { get; set; }
    }
}