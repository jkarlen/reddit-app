using System;
using System.Collections.Concurrent;
using Xunit;

namespace SampleRedditApp.Data.Tests
{
    public class DataStoreTests
    {
        [Fact]
        public void ProcessPostMethod_UpdatesScorecard()
        {
            // Arrange
            var dataStore = new DataStore();
            var type = typeof(DataStore);
            var scorecardField = type.GetField("_SubRedditScoreCard", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var scorecard = scorecardField!.GetValue(dataStore) as ConcurrentDictionary<string, int>;

            // Act
            dataStore.ProcessPost("test post", "test subreddit");

            // Assert
            Assert.Single(scorecard!);
            Assert.Equal(1, scorecard!["test subreddit"]);
        }


        [Fact]
        public void DumpMethod_ReturnsExpectedOutput()
        {
            // Arrange
            var dataStore = new SampleRedditApp.Data.DataStore();

            // Act
            dataStore.Dump();

            // Assert
            // todo: assert that the output is as expected
        }

    }
}
