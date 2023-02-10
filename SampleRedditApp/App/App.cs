using System;
using SampleRedditApp.Reddit;
using SampleRedditApp.Data;

namespace SampleRedditApp.App
{
	public class App
	{
		private DataStore _dataStore = new DataStore();


		public void Run()
		{
			RedditClient.SubscribeToStream(_dataStore);
		}

		public void DumpData()
		{
			_dataStore.Dump();
			Thread.Sleep(5000);
			DumpData();
		}
	}
}

