// em DbContextSmokeTests.cs
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Voltly.Infrastructure.Persistence;
using Voltly.Domain.Entities;

namespace Voltly.Tests.Integration;

public class DbContextSmokeTests : IClassFixture<OracleTestContainer>
{
    private readonly OracleTestContainer _oracle;

    public DbContextSmokeTests(OracleTestContainer oracle) => _oracle = oracle;

    [Fact]
    public async Task CanInsertUser()
    {
        var options = new DbContextOptionsBuilder<VoltlyDbContext>()
            .UseOracle(_oracle.ConnectionString)
            .UseLazyLoadingProxies()
            .EnableSensitiveDataLogging()
            .Options;

        await using var ctx = new VoltlyDbContext(options);
        await ctx.Database.EnsureCreatedAsync();

        var user = new User { Name = "Ada", Email = "ada@volt.ly", Password = "x" };
        await ctx.Users.AddAsync(user);
        await ctx.CommitAsync();

        (await ctx.Users.CountAsync()).Should().Be(1);
    }
}