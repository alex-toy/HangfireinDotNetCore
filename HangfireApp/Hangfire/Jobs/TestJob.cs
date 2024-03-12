namespace HangfireApp.Jobs
{
    public class TestJob
    {
        public TestJob()
        {
        }

        public void WriteLog(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd hh:mm:ss tt} {message}");
        }
    }
}
