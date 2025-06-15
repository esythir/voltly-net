using System;
using System.Threading.Tasks;
using Testcontainers.Oracle;
using Xunit;

namespace Voltly.Tests.Integration;

/// <summary>
/// Sobe (quando possível) um Oracle XE em container para testes de integração.
/// Se o Docker não estiver disponível, o container não é inicializado e os
/// testes podem ser ignorados usando <c>Skip.IfNot</c>.
/// </summary>
public sealed class OracleTestContainer : IAsyncLifetime
{
    private OracleContainer? _container;
    
    public bool IsDockerAvailable { get; private set; }

    public OracleTestContainer()
    {
        try
        {
            _container = new OracleBuilder()
                .WithImage("gvenzl/oracle-xe:21-slim-faststart")
                .WithPassword("oracle")
                .Build();

            IsDockerAvailable = true;
        }
        catch
        {
            IsDockerAvailable = false;
        }
    }
    
    public string ConnectionString =>
        _container?.GetConnectionString()
        ?? throw new InvalidOperationException("Docker is not available.");

    public async Task InitializeAsync()
    {
        if (IsDockerAvailable && _container is not null)
        {
            try
            {
                await _container.StartAsync();
            }
            catch
            {
                IsDockerAvailable = false;
            }
        }
    }

    public async Task DisposeAsync()
    {
        if (IsDockerAvailable && _container is not null)
        {
            await _container.DisposeAsync();
        }
    }
    
}
