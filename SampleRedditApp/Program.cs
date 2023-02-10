using SampleRedditApp.App;

internal class Program
{
    private static void Main(string[] args)
    {
        App app = new App();
        Task worker = new Task(() => app.Run().Wait());
        worker.Start();


        Task.WaitAll(worker);
    }
}