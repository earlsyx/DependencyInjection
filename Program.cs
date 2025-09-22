using Microsoft.Extensions.DependencyInjection;

IServiceCollection serviceCollection = new ServiceCollection();

serviceCollection.AddScoped<SomeEndPoint>();
serviceCollection.AddTransient<ServiceA>() ;
serviceCollection.AddSingleton<App>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var app = serviceProvider.GetRequiredService<App>();
for (int i = 1; i < 4; i++)
{
    app.Run(i, serviceProvider);
}
public class App(ServiceA serviceA)
{
    private readonly ServiceA _serviceA = serviceA;
    public void Run(int requestNumber, ServiceProvider serviceProvider)
    {
        Console.WriteLine($"Request {requestNumber}:");
        using var scope = serviceProvider.CreateScope();
        var endpoint = scope.ServiceProvider
            .GetRequiredService<SomeEndPoint>();
        endpoint.Handle();
        _serviceA.DoWork();
    }
}

public class SomeEndPoint(ServiceA serviceA) : BaseClass
{
    private readonly ServiceA _serviceA = serviceA;
    public void Handle()
    {
        Out($"{this}SomeEndPoint.Handle");
        _serviceA.DoWork();
    }
}

public class ServiceA : BaseClass
{
    internal void DoWork()
    {
        Out($"{this} doing work.");
    }
}
public class BaseClass
{
    private readonly IndentTracker _indentTracker = new();
    public override string ToString()
    {
        return base.ToString() + '-' + GetHashCode().ToString()[^4..];
    }

    public void Out(string msg)
    {
        Console.WriteLine($"{_indentTracker.GetIndent()}{msg}");
    }
}

public sealed class IndentTracker
{
    public string GetIndent()
    {
        int count = Environment.StackTrace.Count(a => a == '\n') - 5;

        return new string(' ', count * 2);
    }
}