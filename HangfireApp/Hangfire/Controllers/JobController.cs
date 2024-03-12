using Hangfire;
using HangfireApp.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace HangfireApp.Controllers
{
    [Route("api/[Controller]")]
    public class JobController : ControllerBase
    {
        [HttpPost]
        [Route("CreateBackgroundJob")]
        public ActionResult CreateBackgroundJob()
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("background job triggered"));
            return Ok("hello world");
        }

        [HttpPost]
        [Route("CreateScheduledJob")]
        public ActionResult CreateScheduledJob()
        {
            DateTime scheduleDate = DateTime.UtcNow.AddSeconds(5);
            DateTimeOffset offset = new DateTimeOffset(scheduleDate);

            BackgroundJob.Schedule(() => Console.WriteLine("background job triggered"), offset);
            return Ok("hello world");
        }

        [HttpPost]
        [Route("CreateContinuationJob")]
        public ActionResult CreateContinuationJob()
        {
            DateTime scheduleDate = DateTime.UtcNow.AddSeconds(5);
            DateTimeOffset offset = new DateTimeOffset(scheduleDate);

            string jobId1 = BackgroundJob.Schedule(() => Console.WriteLine("background job triggered"), offset);

            string jobId2 = BackgroundJob.ContinueJobWith(jobId1, () => Console.WriteLine("continuation 1 job triggered"));

            string jobId3 = BackgroundJob.ContinueJobWith(jobId2, () => Console.WriteLine("continuation 2 job triggered"));

            return Ok("hello world");
        }

        [HttpPost]
        [Route("CreateRecurringJob")]
        public ActionResult CreateRecurringJob()
        {
            RecurringJob.AddOrUpdate("recurring_job_1", () => Console.WriteLine("recurring job triggered"), "* * * * *");

            return Ok("hello world");
        }

        [HttpPost]
        [Route("CreateJob")]
        public ActionResult CreateJob()
        {
            BackgroundJob.Enqueue<TestJob>(x => x.WriteLog("background job triggered"));

            DateTime scheduleDate = DateTime.UtcNow.AddSeconds(5);
            DateTimeOffset offset = new DateTimeOffset(scheduleDate);
            BackgroundJob.Schedule<TestJob>(x => x.WriteLog("background job triggered"), offset);

            string jobId1 = BackgroundJob.Schedule<TestJob>(x => x.WriteLog("background job triggered"), offset);
            string jobId2 = BackgroundJob.ContinueJobWith<TestJob>(jobId1, x => x.WriteLog("continuation 1 job triggered"));
            string jobId3 = BackgroundJob.ContinueJobWith<TestJob>(jobId2, x => x.WriteLog("continuation 2 job triggered"));

            return Ok("hello world");
        }
    }
}
