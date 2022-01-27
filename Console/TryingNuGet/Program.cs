using Newtonsoft.Json;

public class Account
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DateTime DOB { get; set; }
}

public class Program 
{

    static void Main(string[] args)
    {
        Account account = new Account
        {
            Name = "John Doe",
            Email = "john@nuget.com",
            DOB = new DateTime(1981, 6, 13, 11, 15, 0, DateTimeKind.Utc),
        };

        string json = JsonConvert.SerializeObject(account, Formatting.Indented);
        Console.WriteLine(json);
    }
}