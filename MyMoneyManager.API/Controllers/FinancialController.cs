using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMoneyManager.API.Interfaces.IServices;
using MyMoneyManager.API.Models;
using MyMoneyManager.API.Services;
using MyMoneyManager.Shared.ViewModels;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

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

    [HttpGet("dailyzeros/{accountId}")]
    public ICollection<FinancialStatusViewModel> GetDayilyZeros(int accountId)
    {
        return GetFinancialStatusWithZeros(accountId, DateTime.UtcNow.Date.AddDays(1), "day");
    }
    [HttpGet("monthlyzeros/{accountId}")]
    public ICollection<FinancialStatusViewModel> GetMonthlyZeros(int accountId)
    {
        return GetFinancialStatusWithZeros(accountId, DateTime.UtcNow.Date.AddDays(1), "month");

    }
    [HttpGet("yearlyzeros/{accountId}")]
    public ICollection<FinancialStatusViewModel> GetYearlyZeros(int accountId)
    {
        return GetFinancialStatusWithZeros(accountId, DateTime.UtcNow.Date.AddDays(1), "year");
    }

    private DateTime GetStartDate(string userId)
    {
        return _context.Split.Where(s => s.AppUserId == userId).Include(s => s.Transaction).First().Transaction.Occured;
    }
    private ICollection<FinancialStatusViewModel> GetFinancialStatusWithZeros(int accountId, DateTime currentDate, string interval)
    {
        var userId = _userService.GetUserId(HttpContext);
        DateTime startTime = GetStartDate(userId);

        var originalResult = _context.Transaction
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
                CurrentBalance = CalculateRunningTotal(_context.Transaction.Where(t => t.AppUserId == userId &&
                                                                               t.Splits.Any(s => s.AccountId == accountId) &&
                                                                               t.Occured >= startTime &&
                                                                               t.Occured <= currentDate)
                                                  .Include(t => t.Splits)
                                                  .AsEnumerable()
                                                  .GroupBy(t => GetGroupingKey(t.Occured, interval)), accountId, group.Key)
            })
            .ToList();

        var existingTimePeriods = originalResult.Select(item => item.TimePeriod).ToList();

        var allTimePeriods = GetPossibleTimePeriods(startTime, currentDate.AddDays(1), interval).ToList();

        var missingTimePeriods = allTimePeriods.Except(existingTimePeriods);

        foreach (var missingTimePeriod in missingTimePeriods)
        {
            originalResult.Add(new FinancialStatusViewModel
            {
                TimePeriod = missingTimePeriod,
                FundChange = 0,
                CurrentBalance = 0
            });
        }
        var finalResult = originalResult.OrderBy(item => item.TimePeriod).ToList();
        return finalResult;
    }

    private List<DateTime> GetPossibleTimePeriods(DateTime startDate, DateTime endDate, string interval)
    {
        List<DateTime> possibleTimePeriods = new List<DateTime>();

        DateTime currentDate = startDate;
        while (currentDate <= endDate)
        {
            possibleTimePeriods.Add(GetGroupingKey(currentDate, interval));
            currentDate = GetTimePeriodSpan(interval, currentDate);
        }

        return possibleTimePeriods;
    }
    private DateTime GetTimePeriodSpan(string interval, DateTime currentDateTime)
    {
        if (interval == "day")
        {
            return currentDateTime.AddDays(1);
        }
        if (interval == "month")
        {
            return currentDateTime.AddMonths(1);
        }
        if (interval == "year")
        {
            return currentDateTime.AddYears(1);
        }

        return currentDateTime.AddDays(1);
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
                CurrentBalance = CalculateRunningTotal(_context.Transaction.Where(t => t.AppUserId == userId &&
                                                                               t.Splits.Any(s => s.AccountId == accountId) &&
                                                                               t.Occured >= startTime &&
                                                                               t.Occured <= currentDate)
                                                  .Include(t => t.Splits)
                                                  .AsEnumerable()
                                                  .GroupBy(t => GetGroupingKey(t.Occured, interval)), accountId, group.Key)
            })
            .ToList();
        return result;
    }

    private decimal CalculateRunningTotal(IEnumerable<IGrouping<DateTime, Transaction>> groups, int accountId, DateTime currentTime)
    {
        decimal runningTotal = 0;
        //runningTotal += group.Sum(g => g.Sum(t =>  t.Splits.Where(s => s.AccountId == accountId).Sum(s => s.Balance)));
        foreach (var group in groups.OrderBy(g => g.Key))
        {

            if (group.Key <= currentTime)
            {
                runningTotal += group.Sum(t => t.Splits.Where(s => s.AccountId == accountId).Sum(s => s.Balance));
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

        return earliestTransaction?.Occured.Date ?? DateTime.UtcNow.Date;
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
