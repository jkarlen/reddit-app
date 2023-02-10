using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SampleRedditApp.Data;
using Xunit;
using SampleRedditApp.Reddit;

namespace SampleRedditApp.Reddit.Tests
{
    public class RedditClientTests
    {
        [Fact]
        public void SubscribeToStream_ShouldProcessPosts()
        {
            // Arrange
            var dataStore = new DataStore();
            var redditClient = new RedditClient();

            // Act
            dataStore.ProcessPost("This is the Body", "Subreddit 1");

            // Assert
            var scorecard = dataStore!.GetType()!.GetField("_SubRedditScoreCard", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(dataStore) as System.Collections.Concurrent.ConcurrentDictionary<string, int>;
            Assert.True(scorecard!.Count == 1);
            Assert.True(scorecard["Subreddit 1"] == 1);
        }
    }
}
