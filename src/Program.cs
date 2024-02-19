using Microsoft.Extensions.DependencyInjection;
using static Demo.Program;

namespace Demo;
public class Program
{
    public delegate IMyRepository? ServiceResolver(string key);

    public static void Main()
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<RepoA>()
            .AddTransient<RepoB>()
            .AddTransient<RepoC>()
            .AddTransient<MyReadService>()
            .AddTransient<ServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "A":
                        return serviceProvider.GetService<RepoA>();
                    case "B":
                        return serviceProvider.GetService<RepoB>();
                    case "C":
                        return serviceProvider.GetService<RepoC>();
                    default:
                        throw new KeyNotFoundException(); // or maybe return null, up to you
                }
            })
            .BuildServiceProvider();

       var svc = serviceProvider.GetService<MyReadService>();
        svc.Write();
        Console.ReadLine();     

    }
}

public class MyReadService
{
    public readonly IMyRepository? _repositoryA;
    public readonly IMyRepository? _repositoryB;
    public readonly IMyRepository? _repositoryC;

    public MyReadService(ServiceResolver serviceAccessor)
    {
        _repositoryA = serviceAccessor("A");
        _repositoryB = serviceAccessor("B");
        _repositoryC = serviceAccessor("C");
    }

    internal void Write()
    {
        Console.WriteLine(_repositoryA.Read());
        Console.WriteLine(_repositoryB.Read());
        Console.WriteLine(_repositoryC.Read());
    }
}
    public interface IMyRepository
    {
        public string Read();
    }

    public class RepoA : IMyRepository
    {
        public string Read()
        {
            return nameof(RepoA);
        }
    }

    public class RepoB : IMyRepository
    {
        public string Read()
        {
            return nameof(RepoB);
        }
    }

    public class RepoC : IMyRepository
    {
        public string Read()
        {
            return nameof(RepoC);
        }
    }
