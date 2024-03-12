﻿using Hangfire;
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

            string jobId3 = BackgroundJob.ContinueJobWith(jobId2, () => Console.WriteLine("continuation 1 job triggered"));

            return Ok("hello world");
        }

        [HttpPost]
        [Route("CreateRecurringJob")]
        public ActionResult CreateRecurringJob()
        {
            RecurringJob.AddOrUpdate("recurring_job_1", () => Console.WriteLine("recurring job triggered"), "* * * * *");

            return Ok("hello world");
        }
    }
}
