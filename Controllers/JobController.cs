using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace ThanosHangfireApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : ControllerBase
    {
        public JobController(){ }

        [HttpPost]
        [Route("CreateOrderBackgroundJob")]
        public ActionResult CreateOrderBackgroundJob()
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Processing order"));

            return Ok(new { message = "We have received your order. Within 4 hours we will send you an email with confirmation." });
        }

        [HttpPost]
        [Route("ConfirmOrderScheduledJob")]
        public ActionResult ConfirmOrderScheduledJob(int day = 1)
        {
            BackgroundJob.Schedule(() => Console.WriteLine("$\"Sending confirmation email for order..."), TimeSpan.FromDays(day));

            return Ok(new { message = "Order confirmation scheduled." });
        }

        [HttpPost]
        [Route("GenerateOrderReportJob")]
        public ActionResult GenerateOrderReportJob()
        {
            var ordersJobId = BackgroundJob.Enqueue(() => Console.WriteLine("Get sales of the day."));

            var ordersProcessedJobId = BackgroundJob.ContinueWith(ordersJobId, () => Console.WriteLine("Process sales of the day"));

            BackgroundJob.ContinueWith(ordersProcessedJobId, () => Console.WriteLine("Send email with report"));

            return Ok(new { message = "Generation of new order spreadsheet started." });
        }

        [HttpPost]
        [Route("GenerateRecurringComplianceReportsJob")]
        public ActionResult GenerateRecurringComplianceReportsJob()
        {
            RecurringJob.AddOrUpdate("myrecurringjob", () => Console.WriteLine("It will generate spreadsheets for new customers daily"), Cron.Daily);

            RecurringJob.AddOrUpdate("myrecurringjob2", () => Console.WriteLine("It will generate spreadsheets for new customers in 5 seconds"), "*/5 * * * * *");


            return Ok(new { message = "Recurrence to create a spreadsheet for new customers created." });
        }
    }
}

