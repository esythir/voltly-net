using Mapster;
using Voltly.Application.Mapping;
using Voltly.Application.Abstractions;
using Voltly.Application.Features.Users.Commands.RegisterUser;
using Voltly.Domain.Exceptions;
using Moq;
using Xunit;

public class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task Should_throw_when_email_exists()
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(r => r.ExistsByEmailAsync("x@y.com", default)).ReturnsAsync(true);

        var uow     = new Mock<IUnitOfWork>();
        var mapper = new ServiceMapper(TypeAdapterConfig.GlobalSettings);

        var handler = new RegisterUserCommandHandler(uow.Object, repo.Object, mapper);
        var cmd     = new RegisterUserCommand(
            new("John", "x@y.com", "P@ssw0rd!", DateOnly.Parse("1990-01-01")));

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(cmd, default));
    }
}