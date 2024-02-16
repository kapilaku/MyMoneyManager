using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MyMoneyManager.API.Models;
using System.Reflection.Metadata;

namespace MyMoneyManager.API;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
   

    //public DbSet<Ledger> Ledgers { get; set; }
    public DbSet<Account> Account { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<Split> Split { get; set; }
    public DbSet<Currency> Currency { get; set; }
    public DbSet<Tag> Tag { get; set; }

    //public DbSet<UserAccount> UserAccounts { get; set; }
    //public DbSet<UserCurrency> UserCurrencies { get; set; }
    //public DbSet<UserLedger> UserLedgers { get; set; }
    //public DbSet<UserTransaction> UserTransactions { get; set; }
    //public DbSet<UserTransactionDetail> UserTransactionsDetail { get; set; }



}

public class AppUser : IdentityUser
{
    //public ICollection<Ledger> Ledgers { get; set; } = new List<Ledger>();
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Split> Splits { get; set; } = new List<Split>();
    public ICollection<Currency> Currencies { get; set; } = new List<Currency>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}

//public class AppUserManager : UserManager<AppUser>
//{
//    public AppUserManager(IUserStore<AppUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<AppUser> passwordHasher, IEnumerable<IUserValidator<AppUser>> userValidators, IEnumerable<IPasswordValidator<AppUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<AppUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
//    {

//    }

//    override user
//}