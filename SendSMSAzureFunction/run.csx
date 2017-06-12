#r "Newtonsoft.Json"

using System;
using System.Net;
using Newtonsoft.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"Webhook was triggered!");

    string jsonContent = await req.Content.ReadAsStringAsync();
    dynamic data = JsonConvert.DeserializeObject(jsonContent);

    if (data.message == null) {
        return req.CreateResponse(HttpStatusCode.BadRequest, new {
            error = "Please pass message properties in the input object"
        });
    }


    string accountSid = "AC42074f363c21d748b1ecc901e4e751f5";
    string authToken = "6d05e8f25bdbe69befaffe9ccdf795f6";

            TwilioClient.Init(accountSid, authToken);

            // Send a new outgoing SMS by POSTing to the Messages resource
            MessageResource.Create(
                from: new PhoneNumber("+3197004499156"),
                to: new PhoneNumber("+31628777178"),
                body: $"{data.message}");

    return req.CreateResponse(HttpStatusCode.OK, new {
        greeting = $"Message Sent"
    });


}
