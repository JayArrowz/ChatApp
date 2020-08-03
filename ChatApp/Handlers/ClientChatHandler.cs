using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.Handlers
{
    public class ClientChatHandler : SimpleChannelInboundHandler<IByteBuffer>
    {
        protected override void ChannelRead0(IChannelHandlerContext ctx, IByteBuffer msg)
        {            
            var message = msg.ReadString(msg.ReadableBytes, Encoding.UTF8);
            Console.WriteLine($"Server Response: {message}");
        }
    }
}
