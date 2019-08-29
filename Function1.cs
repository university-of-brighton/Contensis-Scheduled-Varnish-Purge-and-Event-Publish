using System;
using System.Net;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionApp_ScheduledEvent
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("0 */15 * * * *")]TimerInfo myTimer, ILogger log)
        {
            var key = Environment.GetEnvironmentVariable("ApiKey");
            var endpoint = Environment.GetEnvironmentVariable("RunnerEndPoint");
            var whenRun = GetRoundedUKTime();
            var url = $"{endpoint}?when={whenRun}&key={key}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.GetResponseAsync();

            log.LogInformation($"Requested Scheduled events for: {whenRun}");
        }

        static string GetRoundedUKTime()
        {
            var ukTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
            int roundMins = (ukTime.Minute / 15) * 15;
            var time = $"{ukTime.ToString("yyyy-MM-ddTHH")}:{roundMins}";
            return time;
        }
    }
}