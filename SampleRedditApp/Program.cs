using SampleRedditApp.App;

App app = new App();
Task worker = new Task(() => app.Run());
worker.Start();


Task.WaitAll(worker);
