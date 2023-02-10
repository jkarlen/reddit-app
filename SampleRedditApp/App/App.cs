using System;
using System.Timers;
using SampleRedditApp.Reddit;
using SampleRedditApp.Data;
using Timer = System.Timers.Timer;

namespace SampleRedditApp.App
{
    public class App
    {
        private DataStore _dataStore = new DataStore();
        private Timer _timer = new Timer(10000);

        public App()
        {
            _timer.Elapsed += DumpData;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        public async Task Run()
        {
            try
            {
                await RedditClient.SubscribeToStream(_dataStore);
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DumpData(object? sender, ElapsedEventArgs e)
        {
            _dataStore.Dump();
        }
    }
}
