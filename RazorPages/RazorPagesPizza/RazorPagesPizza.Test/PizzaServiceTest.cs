using Xunit;
using RazorPagesPizza.Models;
using RazorPagesPizza.Services;
using System;

namespace RazorPagesPizza.Test;

public class PizzaServiceTest
{
    [Fact]
    public void GetAll_ReturnsData()
    {
        // Arrange

        // Act
        var result = PizzaService.GetAll();

        // Assert
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Add_CreatesNewPizza()
    {
        // Arrange

        // Act
        var pizza = new Pizza { Name = "Test pizza add" };
        PizzaService.Add(pizza);

        // Assert
        var addedPizza = Assert.Single(PizzaService.GetAll(), x => x.Name == "Test pizza add");
        var gotPizza = PizzaService.Get(addedPizza.Id);
        Assert.Equal(pizza, gotPizza);
    }

    [Fact]
    public void Delete_RemovesPizza()
    {
        // Arrange
        var pizza = new Pizza { Name = "Delete me" };
        PizzaService.Add(pizza);

        // Act
        PizzaService.Delete(pizza.Id);

        // Assert
        Assert.Null(PizzaService.Get(pizza.Id));
    }

    [Fact]
    public void Update_ValidId_ChangesPizza()
    {
        // Arrange
        var pizza = new Pizza { Name = "Original" };
        PizzaService.Add(pizza);
        int id = pizza.Id;

        // Act
        PizzaService.Update(id, new Pizza { Id = id, Name = "Updated" });

        // Assert
        var updatedPizza = PizzaService.Get(id);
        Assert.Equal("Updated", updatedPizza?.Name);
    }

    [Fact]
    public void Update_InvalidId_ThrowsException()
    {
        // Arrange
        int id = 1;
        int otherId = 2;

        // Act
        Action actual = () => PizzaService.Update(id, new Pizza { Id = otherId });

        // Assert
        Assert.Throws<ArgumentException>(actual);
    }
}
