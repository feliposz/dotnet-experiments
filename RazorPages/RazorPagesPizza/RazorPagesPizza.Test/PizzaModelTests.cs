using Xunit;
using RazorPagesPizza.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace RazorPagesPizza.Test;

public class PizzaModelTests
{
    [Fact]
    public void Model_IsValid()
    {
        // Arrange
        var pizza = new Pizza
        {
            Name = "Valid pizza",
            Price = 10m,
            Size = PizzaSize.Medium,
            IsGlutenFree = true
        };
        var result = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(pizza, new ValidationContext(pizza), result);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void Model_IsNotValid()
    {
        // Arrange
        var pizza = new Pizza
        {
            Name = string.Empty,
            Price = 0,
            Size = PizzaSize.Medium,
            IsGlutenFree = false
        };
        var result = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(pizza, new ValidationContext(pizza), result);

        // Assert
        Assert.False(isValid);
    }
}
