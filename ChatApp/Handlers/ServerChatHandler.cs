using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.Handlers
{
    public class ServerChatHandler : SimpleChannelInboundHandler<IByteBuffer>
    {
        protected override void ChannelRead0(IChannelHandlerContext ctx, IByteBuffer msg)
        {
            var message = msg.ReadString(msg.ReadableBytes, Encoding.UTF8);
            var reply = Encoding.UTF8.GetBytes($"Hello i am server and i got your: {message}");
            ctx.WriteAndFlushAsync(Unpooled.WrappedBuffer(reply));
        }
    }
}
