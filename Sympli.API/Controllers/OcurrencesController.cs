using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Sympli.API.Controllers
{
    using Sympli.API.Domain;

    [Route("api/occurences/")]
    [ApiController]
    public class OccurencesController : ControllerBase
    {
        private readonly Occurences occurrences;

        public OccurencesController(Occurences occurrences)
        {
            this.occurrences = occurrences;
        }

        [HttpGet]
        [Route("values")]
        public IEnumerable<IEnumerable<string>> GetValues()
            => occurrences.Values.Select(occ => new[] { occ.SearchEngine, occ.Keyword, occ.Occurences.ToString() });

        [HttpGet]
        [Route("searchengines")]
        public IEnumerable<string> GetSearchEngines() => occurrences.SearchEngines;


        [HttpGet]
        [Route("keywords")]
        public IEnumerable<string> GetKeywords() => occurrences.Keywords;

        // POST: set the search engines
        [HttpPost]
        [Route("searchengines")]
        public void SetSearchEngines([FromBody] string[] values)
            => occurrences.SearchEngines = values.Distinct();     

        // POST: set keywords
        [HttpPost]
        [Route("keywords")]
        public void SetKeywords([FromBody] string[] values) 
            => occurrences.Keywords = values.Distinct().Select(x=>x.Replace(" ", "+"));
    }
}
