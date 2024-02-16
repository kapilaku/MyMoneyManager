using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyMoneyManager.API.Interfaces.IServices;
using MyMoneyManager.API.Services;

namespace MyMoneyManager.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.


        // Add EF Core context
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnetionString"));
        });

        // Add Authorization
        builder.Services.AddAuthorization();

        // Active Identity APIs
        builder.Services.AddIdentityApiEndpoints<AppUser>()
            .AddEntityFrameworkStores<AppDbContext>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        // add services
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITagService, TagService>();
        builder.Services.AddScoped<ICurrencyService, CurrencyService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<ITransactionService, TransactionSerivce>();
        builder.Services.AddScoped<ISplitService, SplitService>();
        builder.Services.AddScoped<AppDbContext>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        //Map Identity routes
        app.MapIdentityApi<AppUser>();

        app.MapControllers();

        app.Run();
    }
}
