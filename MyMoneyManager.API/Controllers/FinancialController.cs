using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMoneyManager.API.Interfaces.IServices;
using MyMoneyManager.API.Models;
using MyMoneyManager.API.Services;
using MyMoneyManager.Shared.ViewModels;
using System.Diagnostics.CodeAnalysis;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyMoneyManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FinancialController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IUserService _userService;
    public FinancialController(AppDbContext appDbContext, IUserService userService)
    {
        _context = appDbContext;
        _userService = userService;
    }

    // GET api/<FinancialController>/5
    [HttpGet("daily/{accountId}")]
    public ICollection<FinancialStatusViewModel> GetDayily(int accountId)
    {
        return GetFinancialStatus(accountId, DateTime.UtcNow.Date.AddDays(1), "day");
    }
    [HttpGet("monthly/{accountId}")]
    public ICollection<FinancialStatusViewModel> GetMonthly(int accountId)
    {
        return GetFinancialStatus(accountId, DateTime.UtcNow.Date.AddDays(1), "month");

    }
    [HttpGet("yearly/{accountId}")]
    public ICollection<FinancialStatusViewModel> GetYearly(int accountId)
    {
        return GetFinancialStatus(accountId, DateTime.UtcNow.Date.AddDays(1), "year");
    }

    private ICollection<FinancialStatusViewModel> GetFinancialStatus(int accountId, DateTime currentDate, string interval)
    {
        var userId = _userService.GetUserId(HttpContext);
        DateTime startTime = GetStartDate(accountId);

        var result = _context.Transaction
            .Where(t => t.AppUserId == userId &&
                        t.Splits.Any(s => s.AccountId == accountId) &&
                        t.Occured >= startTime &&
                        t.Occured <= currentDate)
            .Include(t => t.Splits)
            .OrderBy(t => t.Occured)
            .AsEnumerable()
            .GroupBy(t => GetGroupingKey(t.Occured, interval))
            .Select(group => new FinancialStatusViewModel
            {
                TimePeriod = group.Key,
                FundChange = group.Sum(t => t.Splits.Where(s => s.AccountId == accountId)
                                                    .Sum(s => s.Balance)),
                CurrentBalance = CalculateRunningTotal(group, accountId, group.Key)
            })
            .ToList();
        return result;
    }

    private decimal CalculateRunningTotal(IGrouping<DateTime, Transaction> group, int accountId, DateTime currentTime)
    {
        decimal runningTotal = 0;
        foreach (var transaction in group.OrderBy(t => t.Occured))
        {
            if (transaction.Occured <= currentTime)
            {
                runningTotal += transaction.Splits.Where(s => s.AccountId == accountId).Sum(s => s.Balance);
            }
        }
        return runningTotal;
    }

    private DateTime GetStartDate(int accountId)
    {
        var userId = _userService.GetUserId(HttpContext);
        var earliestTransaction = _context.Transaction
            .Where(t => t.AppUserId == userId && t.Splits.Any(s => s.AccountId == accountId))
            .OrderBy(t => t.Occured)
            .FirstOrDefault();

        return earliestTransaction?.Occured.Date ?? DateTime.Now.Date;
    }

    private DateTime GetGroupingKey(DateTime transactionDate, string interval)
    {
        if (interval == "day")
        {
            return transactionDate.Date.AddDays(1);
        }
        if (interval == "month")
        {
            return new DateTime(transactionDate.Year, transactionDate.AddMonths(1).Month, 1);
        }
        if (interval == "year")
        {
            return new DateTime(transactionDate.AddYears(1).Year, 1, 1);
        }

        return transactionDate.Date;
    }
}
