using ChatApp.Handlers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    class Server
    {
        private IChannel _channel;
        private MultithreadEventLoopGroup _eventLoop;
        private MultithreadEventLoopGroup _childEventLoop;
        private readonly IPEndPoint _endPoint;

        public Server(IPEndPoint endPoint)
        {
            _endPoint = endPoint;
        }

        public async Task BindAsync()
        {
            _eventLoop = new MultithreadEventLoopGroup();
            _childEventLoop = new MultithreadEventLoopGroup();
            var bootstrap = new ServerBootstrap();
            bootstrap.Group(_eventLoop, _childEventLoop)
                .Channel<TcpServerSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    channel.Pipeline.AddLast(new ServerChatHandler());
                }));

            _channel = await bootstrap.BindAsync(_endPoint);
        }

        public async ValueTask DisposeAsync()
        {
            await _channel?.CloseAsync();
            await _eventLoop?.ShutdownGracefullyAsync();
            await _childEventLoop?.ShutdownGracefullyAsync();
        }
    }
}
