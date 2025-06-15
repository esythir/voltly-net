using System.Threading.Tasks;
using Xunit;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.WaitStrategies;

namespace Voltly.Tests.Integration;

/// <summary>Sobe um Oracle XE em Docker para testes de integração.</summary>
public sealed class OracleTestContainer : IAsyncLifetime
{
    private readonly TestcontainersContainer _container;

    public string ConnectionString =>           // exposto aos testes
        $"User Id=system;Password=oracle;" +
        $"Data Source=localhost:{_container.GetMappedPublicPort(1521)}/XEPDB1";

    public OracleTestContainer()
    {
        _container = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage("gvenzl/oracle-xe:21-slim-faststart")
            .WithPortBinding(1521, assignRandomHostPort: true)
            .WithEnvironment("ORACLE_PASSWORD", "oracle")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1521))
            .Build();
    }

    public Task InitializeAsync() => _container.StartAsync();
    public Task DisposeAsync()    => _container.DisposeAsync();
}