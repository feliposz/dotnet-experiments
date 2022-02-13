namespace ContosoUniversityReverse;

public class RandomDataGenerator
{
    private Random random = new Random();
    private string[] lastNames;
    private string[] firstNames;

    public RandomDataGenerator()
    {
        lastNames = "Mejia Robles Kennedy Case Bryan Leach Mccall Reed Torres Mayo Acevedo Hardy Arroyo Tran Haley Stark Schmidt Kelley Johnston Owens French Banks Flynn Buck Munoz Bauer Hunt Fisher Salinas Roth Sanders Brown Powers Cowan Cochran Weeks Rodgers Krause Salas Jacobson Bush Waters Stevenson Mccann Glenn Santos Everett Chavez Petty Pennington Abbott Maldonado Simon Mendoza Love Nichols Anderson Clay Lyons Chandler Ford Shepard Vargas Blake Singh Green Jefferson Serrano Vang Case Pennington Hawkins Shepard Carlson Waters Walters Greer Ware Rojas Frank Welch Haas Pittman Love Cooper Shepard Hoffman Hernandez Dudley Bright Dillon Moran Mcdaniel Potts Lopez Hill Dodson Leach Michael Walls".Split(" ");
        firstNames = "Joey Aurelio Evan Donny Foster Dwayne Grady Quinton Darin Mickey Hank Kim Peter Jeremy Jess Jimmie Vern Pasquale Romeo Chris Dale Beau Cliff Timothy Raphael Brain Mauro Luke Myron Omar Reynaldo Major Clinton Nolan Raymond Lucien Carey Winfred Dan Abel Elliott Brent Chuck Dirk Tod Emerson Dewey Scot Enrique Al Beatrice Brandy Kathy Jane Marcy Shelly Lucy Cathy Joanna Doris Lindsay Staci Shelia Rosanne Rebecca Luz Flora Rosalie Karla Phoebe Meagan Virginia Amanda Katy Karla Deanne Pearl Christi Victoria Ola Alexandra Marina Lorraine Sybil Adeline Taylor Anita Aurora Neva Alisha Maria Erna Gwendolyn Brenda Bethany Sybil Earline June Brandy Sue".Split(" ");
    }

    public string Pick(string[] data)
    {
        return data[random.Next() % data.Length];
    }

    public T Pick<T>(List<T> data)
    {
        return data[random.Next() % data.Count];
    }

    public string FirstName()
    {
        return Pick(firstNames);
    }

    public string LastName()
    {
        return Pick(lastNames);
    }

    public DateTime Date(int minYear = 2000)
    {
        DateTime minDate = new DateTime(minYear, 1, 1);
        DateTime maxDate = DateTime.Today;
        int days = (maxDate - minDate).Days;
        int randomDays = random.Next() % days;
        return minDate + TimeSpan.FromDays(randomDays);
    }

    public int Range(int min, int max)
    {
        return min + random.Next() % (max - min + 1);
    }
}