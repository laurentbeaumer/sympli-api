using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Sympli.API.Tests
{
    using Sympli.API.Domain;
    using Sympli.API.Services;

    public class SearchEnginesServiceTests
    {
        private readonly Occurences occurences;
        private readonly SearchEnginesService service;
        private readonly string html;

        public SearchEnginesServiceTests()
        {
            occurences = new Occurences
            {
                Keywords = new[] { "e-settlements" },
                SearchEngines = new[] { "google.com" },
            };

            service = new SearchEnginesService(occurences);
            html = File.ReadAllText(@"Expected\html.xml");
        }

        [Test]
        public void TestGetUrl()
        {
            const string expected = @"https://www.google.com/search?q=e-settlements&num=10";
            var actual = service.GetUrl(occurences.SearchEngines.First(), occurences.Keywords.First());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task TestGetPage()
        {
            var actual = await service.GetPage(occurences.SearchEngines.First(), occurences.Keywords.First());
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }

        [Test]
        public void TestGetBody()
        {
            var actual = service.GetBody(html);
            Assert.IsFalse(string.IsNullOrEmpty(actual));
        }

        [Test]
        public void TestCountOccurences()
        {
            const int expected = 4;
            var actual = service.CountOccurences(service.GetBody(html));
            Assert.AreEqual(expected, actual);
        }
    }
}