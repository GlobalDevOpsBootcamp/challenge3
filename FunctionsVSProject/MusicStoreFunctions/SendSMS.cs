using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace MusicStoreFunctions
{
    public static class SendSMS
    {
        [FunctionName("SendSMS")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"Webhook was triggered!");

            string jsonContent = await req.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(jsonContent);

            if (data.message == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    error = "Please pass message properties in the input object"
                });
            }
            
            string accountSid = "AC57c366c44159b8c1794e3611b6626988";
            string authToken = "417f7425bdadd152a34ce02f60f166eb";

            TwilioClient.Init(accountSid, authToken);

            // Send a new outgoing SMS by POSTing to the Messages resource
            MessageResource.Create(
                from: new PhoneNumber("+3197004499261"),
                to: new PhoneNumber("+31622991250"),
                body: $"{data.message}");

            return req.CreateResponse(HttpStatusCode.OK, new
            {
                greeting = $"Message Sent"
            });

        }
    }
}