using Microsoft.Extensions.Hosting;
using Sympli.API.Domain;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using HtmlAgilityPack;

namespace Sympli.API.Services
{
    public class SearchEnginesService : IHostedService, IDisposable
    {
        private readonly Timer timer;
        private readonly Occurences occurences;
        private readonly string pattern = @"www.sympli.com.au";

#if DEBUG
        private readonly TimeSpan period = TimeSpan.FromMinutes(1);
        private readonly int num = 10;
#else
        private readonly TimeSpan period = TimeSpan.FromHours(1);
        private readonly int num = 300;
#endif

        public SearchEnginesService(Occurences occurences)
        {
            timer = new Timer(CallSearchEngines);
            this.occurences = occurences;
        }

        public void Dispose()
        {
            timer.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer.Change(TimeSpan.Zero, period);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            return Task.CompletedTask;
        }

        // Loop in each Search Engines and Each 
        public void CallSearchEngines(object state)
        {
            occurences.Values = CountOccurences();
        }

        public IEnumerable<Occurence> CountOccurences()
        {
            var occurences = this.occurences.Clone();

            foreach (var engine in occurences.SearchEngines)
                foreach (var keyword in occurences.Keywords)
                    yield return new Occurence
                    {
                        SearchEngine = engine,
                        Keyword = keyword,
                        Occurences = CountOccurences(GetBody(GetPage(engine, keyword).Result))
                    };
        }

        public int CountOccurences(string input)
            => Regex.Matches(input, pattern).Count;

        public string GetBody(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.DocumentNode.SelectSingleNode("//body").InnerText;
        }

        public async Task<string> GetPage(string searchEngine, string keyword) 
            => await GetUrl(searchEngine, keyword).GetStringAsync();

        public string GetUrl(string engine, string keyword) 
            => $@"https://www.{engine}/search?q={keyword}&num={num}";

    }
}