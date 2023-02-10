using Newtonsoft.Json;
using SampleRedditApp.Data;

namespace SampleRedditApp.Reddit
{
    public class RedditClient
    {
        private const string RedditApiEndpoint = "https://www.reddit.com/r/all/new.json";

        public async static Task SubscribeToStream(DataStore ds)
        {
            using (HttpClient client = new HttpClient())
            {
                await DoSubscribe(client, ds);
            }
        }

        private static async Task DoSubscribe(HttpClient client, DataStore ds)
        {
            try
            {
                client.DefaultRequestHeaders.Add("User-Agent", "RedditStreamingAPIExample/0.0.1");

                using (HttpResponseMessage response = await client.GetAsync(RedditApiEndpoint))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
    
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var redditResponse = JsonConvert.DeserializeObject<RedditResponse>(content);

                        foreach (var child in redditResponse!.Data!.Children!)
                        {
                            ds.ProcessPost(child.Data!.Body!, child.Data!.Subreddit!);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to retrieve data from the Reddit API. Status code: " + response.StatusCode);
                    }
                }

                await Task.Delay(1000);
                await DoSubscribe(client, ds);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("An error occurred while connecting to the Reddit API: " + ex.Message);
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("The task was cancelled.");
            }
        }

    }

    class RedditResponse
    {
        public string? Kind { get; set; }
        public RedditData? Data { get; set; }
    }

    class RedditData
    {
        public RedditChild[]? Children { get; set; }
        public string? After { get; set; }
    }

    class RedditChild
    {
        public RedditChildData? Data { get; set; }
    }

    class RedditChildData
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public long? CreatedUtc { get; set; }
        public string? Subreddit { get; set; }
        public string? Permalink { get; set; }
        public string? Url { get; set; }
        public string? Body { get; set; }
    }
}

