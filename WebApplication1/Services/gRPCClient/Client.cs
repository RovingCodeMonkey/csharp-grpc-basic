using System.Runtime.CompilerServices;
using Grpc.Net.Client;

namespace WebApplication1.Services.gRPCClient;

public class GreeterClient : IDisposable
{
    private readonly GrpcChannel _channel;
    private readonly Greeter.Greeter.GreeterClient _client;

    // http:// address — Grpc.Net.Client treats plain HTTP as insecure by default
    public GreeterClient(string address = "http://localhost:50051")
    {
        _channel = GrpcChannel.ForAddress(address);
        _client = new Greeter.Greeter.GreeterClient(_channel);
    }

    // Unary RPC: SayHello
    public async Task<string> SayHelloAsync(string name, CancellationToken ct = default)
    {
        var reply = await _client.SayHelloAsync(
            new Greeter.HelloRequest { Name = name },
            cancellationToken: ct);
        return reply.Message;
    }

    // Bidirectional streaming RPC: Chat — sends one message and returns the server's reply
    public async Task<(string User, string Text)> ChatAsync(string user, string text, CancellationToken ct = default)
    {
        using var call = _client.Chat(cancellationToken: ct);
        await call.RequestStream.WriteAsync(new Greeter.ChatMessage { User = user, Text = text }, ct);
        await call.RequestStream.CompleteAsync();
        await call.ResponseStream.MoveNext(ct);
        var response = call.ResponseStream.Current;
        return (response.User, response.Text);
    }


    public void Dispose()
    {
        _channel.Dispose();
        GC.SuppressFinalize(this);
    }
}
