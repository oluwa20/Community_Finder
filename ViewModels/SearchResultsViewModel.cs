using System.Collections.Generic;
using Community_Finder2.Models;
namespace Community_Finder2.ViewModels
{
   

   
        public class SearchResultsViewModel
        {
            public string SearchTerm { get; set; }
            public Community TargetCommunity { get; set; }
            public List<Community> NearestCommunities { get; set; }
        }

    

}
