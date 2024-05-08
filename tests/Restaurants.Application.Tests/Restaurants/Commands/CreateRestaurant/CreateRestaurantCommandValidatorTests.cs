using FluentValidation.TestHelper;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandValidatorTests
{
    [Fact]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // arrange

        var command = new CreateRestaurantCommand()
        {
            Name = "Testing-1",
            Category = "Pakistani",
            ContactEmail = "test@test.com",
            PostalCode = "12345",
        };

        var validator = new CreateRestaurantCommandValidator();

        // act

        var result = validator.TestValidate(command);

        // assert

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_ForValidCommand_ShouldHaveValidationErrors()
    {
        // arrange

        var command = new CreateRestaurantCommand()
        {
            Name = "AB",
            Category = "Japanese",
            ContactEmail = "testing",
            PostalCode = "123467",
        };

        var validator = new CreateRestaurantCommandValidator();

        // act

        var result = validator.TestValidate(command);

        // assert

        result.ShouldHaveValidationErrorFor(c => c.Name);
        result.ShouldHaveValidationErrorFor(c => c.Category);
        result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }

    [Theory]
    [InlineData("Pakistani")]
    [InlineData("Indian")]
    [InlineData("Chinese")]
    public void Validator_ToValidCategory_ShouldNotHaveValidationError(string category)
    {
        // arrange

        var validator = new CreateRestaurantCommandValidator();
        var command = new CreateRestaurantCommand() { Category = category };

        // act

        var result = validator.TestValidate(command);

        // assert

        result.ShouldNotHaveValidationErrorFor(c => c.Category);
    }
}
