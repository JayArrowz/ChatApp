using DotNetty.Buffers;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            await MainAsync();
        }

        private static async Task MainAsync()
        {
            var serverEndpoint = new IPEndPoint(IPAddress.Loopback, 5252);
            var server = new Server(serverEndpoint);
            var client = new Client(serverEndpoint);

            await server.BindAsync();
            var channel = await client.ConnectAsync();

            while (true)
            {
                Console.WriteLine("Enter message to send: ");
                var testMessage = Encoding.UTF8.GetBytes(Console.ReadLine());
                var buffer = Unpooled.WrappedBuffer(testMessage);
                await channel.WriteAndFlushAsync(buffer);
            }
        }
    }
}
