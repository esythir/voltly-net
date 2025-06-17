using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;
using Voltly.Application.Abstractions;
using Voltly.Domain.Entities;
using Voltly.Infrastructure.Repositories;
using Xunit;

namespace Voltly.Tests.Unit.Repositories;

public class UserRepositoryTests
{
    private readonly Mock<IApplicationDbContext> _dbContextMock;
    private readonly IUserRepository _userRepository;

    public UserRepositoryTests()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();
        _userRepository = new UserRepository(_dbContextMock.Object);
    }

    [Fact]
    public async Task GetByEmailAsync_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        var email = "test@example.com";
        var expectedUser = new User
        {
            Id = 1,
            Name = "Test User",
            Email = email,
            Password = "hashed_password",
            BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-30)),
            Role = Domain.Enums.UserRole.User,
            IsActive = true
        };

        var data = new[] { expectedUser }.AsQueryable();
        var dbSetMock = new Mock<DbSet<User>>();

        dbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<User>(data.Provider));
        dbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
        dbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
        dbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _dbContextMock.Setup(x => x.Users).Returns(dbSetMock.Object);

        // Act
        var result = await _userRepository.GetByEmailAsync(email);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public async Task GetByEmailAsync_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var email = "nonexistent@example.com";
        var data = Array.Empty<User>().AsQueryable();
        var dbSetMock = new Mock<DbSet<User>>();

        dbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<User>(data.Provider));
        dbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
        dbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
        dbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _dbContextMock.Setup(x => x.Users).Returns(dbSetMock.Object);

        // Act
        var result = await _userRepository.GetByEmailAsync(email);

        // Assert
        result.Should().BeNull();
    }
}

internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    internal TestAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        return new TestAsyncEnumerable<TEntity>(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new TestAsyncEnumerable<TElement>(expression);
    }

    public object? Execute(Expression expression)
    {
        return _inner.Execute(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return _inner.Execute<TResult>(expression);
    }

    public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
    {
        return new TestAsyncEnumerable<TResult>(expression);
    }

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        var resultType = typeof(TResult).GetGenericArguments()[0];
        var executionResult = typeof(IQueryProvider)
            .GetMethod(
                name: nameof(IQueryProvider.Execute),
                genericParameterCount: 1,
                types: new[] { typeof(Expression) })
            ?.MakeGenericMethod(resultType)
            .Invoke(this, new[] { expression });

        return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
            ?.MakeGenericMethod(resultType)
            .Invoke(null, new[] { executionResult })!;
    }
}

internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    private readonly IQueryProvider _provider;

    public TestAsyncEnumerable(IEnumerable<T> enumerable)
        : base(enumerable)
    {
        _provider = new TestAsyncQueryProvider<T>(Enumerable.Empty<T>().AsQueryable().Provider);
    }

    public TestAsyncEnumerable(Expression expression)
        : base(expression)
    {
        _provider = new TestAsyncQueryProvider<T>(Enumerable.Empty<T>().AsQueryable().Provider);
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    IQueryProvider IQueryable.Provider => _provider;
}

internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    public void Dispose()
    {
        _inner.Dispose();
    }

    public T Current
    {
        get { return _inner.Current; }
    }

    public ValueTask<bool> MoveNextAsync()
    {
        return new ValueTask<bool>(_inner.MoveNext());
    }

    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return new ValueTask();
    }
}