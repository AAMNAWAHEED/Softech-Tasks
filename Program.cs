using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Identity;
using Microsoft.Azure.ServiceBus;  //main nuget package.
using Newtonsoft.Json;
using Azure.Messaging.ServiceBus.Administration;


namespace PubSub
{
    class UserMessage
    {

        public string id { get; set; }
        public string message { get; set; }
    }

    class AzureConnect
    {
        private ITopicClient topicClient;
        public async Task connectToService(string url,string Topic)
        {
            var adminClient = new ServiceBusAdministrationClient(url);
            if (!await adminClient.TopicExistsAsync(Topic))
            {
                await adminClient.CreateTopicAsync(Topic);
                Console.WriteLine($"Topic '{Topic}' created successfully.");
            }
            
            topicClient = new TopicClient(url, Topic);
            

        }
        public async Task CretaeMessage(string msg)
        {
            if(topicClient == null)
            {
                Console.WriteLine("empty");
                return;
            }
            UserMessage usermessage = new UserMessage();
            string guid = Guid.NewGuid().ToString();
            usermessage.id = guid;
            usermessage.message = msg;

            string serializeMsg = JsonConvert.SerializeObject(usermessage);
            var message = new Message(Encoding.UTF8.GetBytes(serializeMsg));
            await topicClient.SendAsync(message);

        }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
                AzureConnect conn = new AzureConnect();
                await conn.connectToService("Endpoint=sb://mysamplebus1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=f6VKQJgywzQn30moDBtCNgNKPgZKyA/2m+ASbI9Ob18=", "testtopic");
                
                //generate uuid
                await conn.CretaeMessage("new request");

            
        }
    }
}
