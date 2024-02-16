using MyMoneyManager.Shared;

namespace MyMoneyManager.API.Models;

//public class Ledger
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public string Description { get; set; }
//    public DateTime Created { get; set; }
//    public DateTime Updated { get; set; }

//    public string AppUserId { get; set; }

//    // public AppUser User { get; set; }
//    public ICollection<Account> Accounts { get; set; }
//    public ICollection<Transaction> Transactions { get; set; }
//}

public class Account
{
    public int Id { get; set; }
    public string AppUserId { get; set; }
    public int CurrencyId { get; set; }
    public int? ParentAccountId { get; set; }

    public AccountType Type { get; set; }
    public string Name { get; set; }
    public Decimal Balance { get; set; }

    public Currency Currency { get; set; }
    public Account? ParentAccount { get; set; }


    //public int LedgerId { get; set; }
    public ICollection<Account> ChildAccounts { get; set; }
    public ICollection<Split> Splits { get; set; }

}

public class Transaction
{
    public int Id { get; set; }
    public string AppUserId { get; set; }
    public int TagId { get; set; }

    public DateTime Occured { get; set; }
    public string Description { get; set; }

    public Tag? Tag { get; set; }
    public ICollection<Split> Splits { get; set; }
}

public class Split
{
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public int AccountId { get; set; }
    public string AppUserId { get; set; }
    public int CurrencyId { get; set; }

    public Decimal Balance { get; set; }
    
    public Currency Currency { get; set; }
    public Account Account { get; set; }
    public Transaction Transaction { get; set; }
}

public class Currency
{
    public int Id { get; set; }
    public string AppUserId { get; set; }

    public string CurrencyCode { get; set; }
    public string CurrencyName { get; set; }
}

public class Tag
{
    public int Id { get; set; }
    public string AppUserId { get; set; }

    public string TagName { get; set; }
}