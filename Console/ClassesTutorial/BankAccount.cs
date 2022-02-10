using System;
using System.Text;
using System.Collections.Generic;

namespace ClassesTutorial
{
    public class BankAccount
    {
        private static int accountNumberSeed = 1234567890;
        private readonly decimal minimumBalance;
        private List<Transaction> allTransactions = new List<Transaction>();

        public string Number { get; }
        public string Owner { get; set; }
        public decimal Balance
        {
            get
            {
                decimal balance = 0;
                foreach (var item in allTransactions)
                {
                    balance += item.Amount;
                }
                return balance;
            }
        }

        public BankAccount(string name, decimal initialBalance) : this(name, initialBalance, 0) {}

        public BankAccount(string name, decimal initialBalance, decimal minimumBalance)
        {
            Number = accountNumberSeed.ToString();
            accountNumberSeed++;
            Owner = name;
            this.minimumBalance = minimumBalance;
            if (initialBalance != 0)
                MakeDeposit(initialBalance, DateTime.Now, "Initial balance");
        }

        public void MakeDeposit(decimal amount, DateTime date, string note)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
            }
            var deposit = new Transaction(amount, date, note);
            allTransactions.Add(deposit);
        }

        public void MakeWithdrawal(decimal amount, DateTime date, string note)
        {
            if (amount <= 0)
            {
                throw new ArgumentException(nameof(amount), "Amount of withdrawal must be positive");
            }
            var overdraftTransaction = CheckWithdrawalLimit(Balance - amount < minimumBalance);
            var withdrawal = new Transaction(-amount, date, note);
            allTransactions.Add(withdrawal);
            if (overdraftTransaction != null)
                allTransactions.Add(overdraftTransaction);
        }

        protected virtual Transaction? CheckWithdrawalLimit(bool isOverdrawn)
        {
            if (isOverdrawn)
            {
                throw new InvalidOperationException("Not sufficient funds for this withdrawal.");
            }
            else
            {
                return default;
            }
        }

        public string GetAccountHistory()
        {
            var report = new StringBuilder();
            decimal balance = 0;
            report.AppendLine($"Account history for {Owner} with account number {Number}");
            report.AppendLine($"{"Date",-12} {"Amount",10} {"Balance",10}  Note");
            foreach (var item in allTransactions)
            {
                balance += item.Amount;
                report.AppendLine($"{item.Date.ToShortDateString(),-12} {item.Amount,10:N2} {balance,10:N2}  {item.Notes}");
            }
            return report.ToString();
        }

        public virtual void PerformMonthEndTransactions() {}
    }
}