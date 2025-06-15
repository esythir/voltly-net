using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Xunit;
using Xunit.Abstractions; // se usar IAsyncLifetime


namespace Voltly.Tests.Integration;

/// <summary>
/// Sobe um Oracle XE em container para testes de integração.
/// </summary>
public sealed class OracleTestContainer : IAsyncLifetime
{
    private readonly TestcontainersContainer _container;
    public string ConnectionString => $"User Id=system;Password=oracle;Data Source=localhost:{_container.GetMappedPublicPort(1521)}/XEPDB1";

    public OracleTestContainer()
    {
        _container = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage("gvenzl/oracle-xe:21-slim-faststart")
            .WithPortBinding(1521, true)
            .WithEnvironment("ORACLE_PASSWORD", "oracle")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1521))
            .Build();
    }

    public async Task InitializeAsync() => await _container.StartAsync();
    public async Task DisposeAsync() => await _container.DisposeAsync();
}