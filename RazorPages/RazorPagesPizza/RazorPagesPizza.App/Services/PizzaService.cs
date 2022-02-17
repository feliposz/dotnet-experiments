using RazorPagesPizza.Models;

namespace RazorPagesPizza.Services
{
    public static class PizzaService
    {
        static List<Pizza> Pizzas { get; }
        static int nextId = 1;

        static PizzaService()
        {
            Pizzas = new List<Pizza>();
            Add(new Pizza { Name = "Classic Italian", Price = 20.00M, Size = PizzaSize.Large, IsGlutenFree = false });
            Add(new Pizza { Name = "Veggie", Price = 15.00M, Size = PizzaSize.Small, IsGlutenFree = true });
        }

        public static void Add(Pizza pizza)
        {
            pizza.Id = nextId++;
            Pizzas.Add(pizza);
        }

        public static List<Pizza> GetAll() => Pizzas;

        public static Pizza? Get(int id) => Pizzas.SingleOrDefault(x => x.Id == id);

        public static void Delete(int id)
        {
            var pizza = Get(id);
            if (pizza is null)
            {
                return;
            }
            Pizzas.Remove(pizza);
        }

        public static void Update(int id, Pizza pizza)
        {
            if (id != pizza.Id)
            {
                throw new ArgumentException("id mismatch");
            }
            var index = Pizzas.FindIndex(x => x.Id == id);
            if (index == -1)
            {
                return;
            }
            Pizzas[index] = pizza;
        }
    }
}