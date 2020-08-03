using ChatApp.Handlers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System.Net;
using System.Threading.Tasks;

namespace ChatApp
{
    public class Client
    {
        private IChannel _channel;
        private IEventLoop _eventLoop;
        private readonly IPEndPoint _endPoint;

        public Client(IPEndPoint endPoint)
        {
            _endPoint = endPoint;
        }

        public async Task<IChannel> ConnectAsync()
        {
            _eventLoop = new SingleThreadEventLoop();

            var bootstrap = new Bootstrap();
            bootstrap.Group(_eventLoop)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    channel.Pipeline.AddLast(new ClientChatHandler());
                }));

            _channel = await bootstrap.ConnectAsync(_endPoint);
            return _channel;
        }

        public async ValueTask DisposeAsync()
        {
            await _channel?.CloseAsync();
            await _eventLoop?.ShutdownGracefullyAsync();
        }
    }
}
