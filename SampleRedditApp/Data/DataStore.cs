using System;
using System.Collections.Concurrent;
using System.Linq;

namespace SampleRedditApp.Data
{
    public class DataStore
    {
        private int _TotalPostsAnalyzed = 0;
        private int _PostsAnalyzedSinceLastDump = 0;
        private readonly ConcurrentDictionary<string, int> _SubRedditScoreCard = new ConcurrentDictionary<string, int>();
        private readonly object _Lock = new object();

        public void ProcessPost(string Body, string SubReddit)
        {
            lock (_Lock)
            {
                _SubRedditScoreCard.AddOrUpdate(SubReddit, 1, (key, oldValue) => oldValue + 1);
                _TotalPostsAnalyzed++;
                _PostsAnalyzedSinceLastDump++;
            }
        }

        public void Dump()
        {
            Console.WriteLine("Total Posts Analyzed: " + _TotalPostsAnalyzed.ToString());
            Console.WriteLine("Total Posts Analyzed Since Last Dump: " + _PostsAnalyzedSinceLastDump.ToString());

            Console.WriteLine("\nCurrent Top 10 Subreddits:");

            var sortedSubs = new SortedDictionary<string, int>(_SubRedditScoreCard)
                                .OrderByDescending(pair => pair.Value)
                                .Take(10)
                                .ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (KeyValuePair<string, int> item in sortedSubs)
            {
                Console.WriteLine("{0}: {1}", item.Key, item.Value);
            }

            Console.WriteLine("\n");

            _PostsAnalyzedSinceLastDump = 0;
        }
    }
}
