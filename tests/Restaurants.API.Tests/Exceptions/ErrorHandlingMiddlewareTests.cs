using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Xunit;

namespace Restaurants.API.Exceptions.Tests;

public class ErrorHandlingMiddlewareTests
{
    [Fact]
    public async void InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
    {
        // arrange

        var middleware = new ErrorHandlingMiddleware(
            new Mock<ILogger<ErrorHandlingMiddleware>>().Object
        );
        var context = new DefaultHttpContext();
        var nextDelegateMock = new Mock<RequestDelegate>();

        // act

        await middleware.InvokeAsync(context, nextDelegateMock.Object);

        // assert

        nextDelegateMock.Verify(next => next.Invoke(context), Times.Once);
    }

    [Fact]
    public async void InvokeAsync_WhenEntityNotFoundExceptionThrown_ShouldSetStatusCodeTo404()
    {
        // arrange

        var middleware = new ErrorHandlingMiddleware(
            new Mock<ILogger<ErrorHandlingMiddleware>>().Object
        );
        var context = new DefaultHttpContext();
        var entityNotFoundException = new EntityNotFoundException(nameof(Restaurant), "1");

        // act

        await middleware.InvokeAsync(context, _ => throw entityNotFoundException);

        // assert
        context.Response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async void InvokeAsync_WhenForbidExceptionThrown_ShouldSetStatusCodeTo403()
    {
        // arrange

        var middleware = new ErrorHandlingMiddleware(
            new Mock<ILogger<ErrorHandlingMiddleware>>().Object
        );
        var context = new DefaultHttpContext();
        var forbidException = new ForbidException();

        // act

        await middleware.InvokeAsync(context, _ => throw forbidException);

        // assert
        context.Response.StatusCode.Should().Be(403);
    }

    [Fact]
    public async void InvokeAsync_WhenGenericExceptionThrown_ShouldSetStatusCodeTo500()
    {
        // arrange

        var middleware = new ErrorHandlingMiddleware(
            new Mock<ILogger<ErrorHandlingMiddleware>>().Object
        );
        var context = new DefaultHttpContext();
        var exception = new Exception();

        // act

        await middleware.InvokeAsync(context, _ => throw exception);

        // assert
        context.Response.StatusCode.Should().Be(500);
    }
}
