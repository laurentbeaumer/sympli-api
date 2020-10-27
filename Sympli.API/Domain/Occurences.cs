using System.Collections.Generic;
using System.Linq;

namespace Sympli.API.Domain
{
    public class Occurences
    {
        public IEnumerable<string> Keywords { get; set; } = new[] { "e-settlements" };
        public IEnumerable<string> SearchEngines { get; set; } = new[] { "google.com" };
        public IEnumerable<Occurence> Values { get; set; } = Enumerable.Empty<Occurence>();

        public Occurences Clone()
        {
            lock (this)
                return new Occurences
                {
                    Keywords = Keywords.Select(x=>x),
                    SearchEngines = SearchEngines.Select(x => x),
                };
        }
    }

    public class Occurence
    {
        public string Keyword { get; set; } = string.Empty;
        public string SearchEngine { get; set; } = string.Empty;
        public int Occurences { get; set; } = 0;
    }
}
