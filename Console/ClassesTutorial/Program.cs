using System;
using ClassesTutorial;

void DisplayError(string title, string message)
{
    Console.BackgroundColor = ConsoleColor.Red;
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write($"[ {title} ]");
    Console.ResetColor();
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Write($"Message: {message}");
    Console.ResetColor();
    Console.WriteLine();
}

var account = new BankAccount("Satoshi Nakamoto", 1000);
Console.WriteLine($"Account {account.Number} was create for {account.Owner} with {account.Balance:c} initial balance.");

account.MakeWithdrawal(500, DateTime.Today.AddDays(1), "Rent payment");
Console.WriteLine($"Balance after withdrawal....{account.Balance:c}");

account.MakeDeposit(100, DateTime.Today.AddDays(2), "Friend payed me back");
Console.WriteLine($"Balance after deposit.......{account.Balance:c}");

BankAccount invalidAccount;
try
{
    invalidAccount = new BankAccount("invalid", -1);
}
catch (ArgumentOutOfRangeException e)
{
    DisplayError("Exception caught creating account with negative balance.", e.Message);
}

try
{
    account.MakeWithdrawal(1000, DateTime.Now, "Attempt to withdrawal");
}
catch (InvalidOperationException e)
{
    DisplayError("Exception caught trying to overdraw", e.Message);
}

Console.WriteLine(account.GetAccountHistory());

var giftCard = new GiftCardAccount("Gift Card", 100, 50);
giftCard.MakeWithdrawal(20, DateTime.Today.AddDays(-10), "Get expensive coffee");
giftCard.MakeWithdrawal(20, DateTime.Today.AddDays(-5), "Buy groceries");
giftCard.PerformMonthEndTransactions();
giftCard.MakeDeposit(27.50m, DateTime.Today.AddDays(1), "Add some additional spending money");
Console.WriteLine(giftCard.GetAccountHistory());

var savings = new InterestEarningAccount("Savings account", 10000);
savings.MakeDeposit(750, DateTime.Today.AddDays(-3), "Save money");
savings.MakeDeposit(1250, DateTime.Today.AddDays(-2), "Add more savings");
savings.MakeWithdrawal(250, DateTime.Today.AddDays(-1), "Needed to pay monthly bills");
savings.PerformMonthEndTransactions();
Console.WriteLine(savings.GetAccountHistory());

var lineOfCredit = new LineOfCreditAccount("Line of credit", 0, 2000);
lineOfCredit.MakeWithdrawal(1000m, DateTime.Today, "Take out monthly advance");
lineOfCredit.MakeDeposit(50m, DateTime.Today.AddDays(1), "Pay back small ammount");
lineOfCredit.MakeWithdrawal(5000m, DateTime.Today.AddDays(2), "Emergency funds for repairs");
lineOfCredit.MakeDeposit(150m, DateTime.Today.AddDays(3), "Partial restoration of repairs");
lineOfCredit.PerformMonthEndTransactions();
Console.WriteLine(lineOfCredit.GetAccountHistory());

