using Xunit;
using RazorPagesPizza.Pages;
using RazorPagesPizza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesPizza.Test;

public class PizzaPageTests
{
    [Fact]
    public void OnGet_ReturnsPizzas()
    {
        // Arrange
        var pizzaModel = new PizzaModel();

        // Act
        pizzaModel.OnGet();

        // Assert
        Assert.NotEmpty(pizzaModel.pizzas);
    }

    [Fact]
    public void OnPost_ValidPizza_CreatesPizza()
    {
        // Arrange
        var pizzaModel = new PizzaModel();

        // Act
        pizzaModel.NewPizza = new Pizza
        {
            Name = "Valid pizza",
            Price = 1m,
            Size = PizzaSize.Medium,
            IsGlutenFree = false
        };
        var result = pizzaModel.OnPost();

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
        pizzaModel.OnGet();
        Assert.Contains(pizzaModel.pizzas, x => x.Name == "Valid pizza");
    }

    [Fact]
    public void OnPost_Invalid_ReturnsError()
    {
        // Arrange
        var pizzaModel = new PizzaModel();
        pizzaModel.ModelState.AddModelError(string.Empty, "Dummy error");

        // Act
        var result = pizzaModel.OnPost();

        // Assert
        Assert.IsType<PageResult>(result);
    }

    [Fact]
    public void OnPostDelete_RemovesPizza()
    {
        // Arrange
        var pizzaModel = new PizzaModel();
        pizzaModel.NewPizza = new Pizza
        {
            Name = "Delete me",
            Price = 1m,
            Size = PizzaSize.Medium,
            IsGlutenFree = false
        };
        var result = pizzaModel.OnPost();
        pizzaModel.OnGet();
        var pizzaToDelete = Assert.Single(pizzaModel.pizzas, x => x.Name == "Delete me");

        // Act
        result = pizzaModel.OnPostDelete(pizzaToDelete.Id);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
        pizzaModel.OnGet();
        Assert.DoesNotContain(pizzaModel.pizzas, x => x.Name == "Delete me");
    }

    [Theory]
    [InlineData(true, "Gluten Free")]
    [InlineData(false, "Contains gluten")]
    public void GlutenFreeText_ShouldReturn(bool isGlutenFree, string expected)
    {
        // Arrange
        var pizzaModel = new PizzaModel();

        // Act
        var actual = pizzaModel.GlutenFreeText(new Pizza { IsGlutenFree = isGlutenFree });

        // Assert
        Assert.Equal(expected, actual);
    }
}