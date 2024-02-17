using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MyMoneyManager.Shared.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public int? ParentAccountId { get; set; }
        
        public string Name { get; set; }
        public Decimal Balance { get; set; }
        public AccountType AccountType { get; set; }

        public HashSet<AccountViewModel> Children { get; set; }
    }

    public class TransactionViewModel
    {
        public int Id { get; set; }
        public DateTime Occured { get; set; }
        public string Description { get; set; }
        public int TagId { get; set; }
    }

    public class SplitViewModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public Decimal Balance { get; set; }
        public int CurrencyId { get; set; }
    }

    public class TagViewModel
    {
        public int Id { get; set; }
        public string TagName { get; set; }
    }

    public class CurrencyViewModel
    {
        public int Id { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
    }

    //public class LedgerViewModel
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string Description { get; set; }
    //    public DateTime Created { get; set; }
    //    public DateTime Updated { get; set; }
    //}
}
