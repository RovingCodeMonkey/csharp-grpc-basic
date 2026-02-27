using System.Text;
using Greeter;
using Grpc.Core;

public class ServerCLass : Greeter.Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
    }

    public override async Task Chat(
            IAsyncStreamReader<ChatMessage> requestStream,
            IServerStreamWriter<ChatMessage> responseStream,
            ServerCallContext context)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Chat started:");
        await foreach (var message in requestStream.ReadAllAsync())
        {
            sb.AppendLine($"{message.User}: {message.Text}");
            Console.WriteLine($"Received: {message.User}: {message.Text}");
        }
        await responseStream.WriteAsync(new ChatMessage { User = "Server", Text = $"Message received: {sb.ToString()}" });
    }
}
