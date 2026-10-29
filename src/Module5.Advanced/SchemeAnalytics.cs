using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace Module5.Advanced;

/// <summary>
/// Advanced LINQ exercises for analysing pension calculation results.
/// These exercises use more complex LINQ operations:
/// SelectMany, Aggregate, GroupBy with projections, and chaining.
/// </summary>
public class SchemeAnalytics
{
    private readonly List<CalculationResult> _results;

    public SchemeAnalytics(List<CalculationResult> results)
    {
        _results = results;
    }

    // Exercise 3a: Total liability by scheme
    /// <summary>
    /// Groups results by SchemeType and returns total adjusted pension per scheme.
    /// </summary>
    public Dictionary<string, decimal> GetTotalLiabilityByScheme()
    {
        // TODO: Use GroupBy and ToDictionary with Sum
       // throw new NotImplementedException("Exercise 3a: Implement GetTotalLiabilityByScheme");
       var LiabiltyByScheme = _results
       .GroupBy(r => r.SchemeType)
       .ToDictionary(
        g=> g.Key,
        g => g.Sum(r => r.AdjustedPension)
       );
       return LiabiltyByScheme;
    }

    // Exercise 3b: Average pension by age band
    /// <summary>
    /// Groups results into age bands (55-59, 60-64, 65-69, 70+) and returns average pension per band.
    /// </summary>
    
    public Dictionary<string, decimal> GetAveragePensionByAgeBand()
    {
        // TODO: Create age band labels, group by band, average the adjusted pension
        // Hint: Use a helper method or inline logic to map age to band:
        //   age < 60 → "55-59"
        //   age < 65 → "60-64"
        //   age < 70 → "65-69"
        //   else → "70+"
        //throw new NotImplementedException("Exercise 3b: Implement GetAveragePensionByAgeBand");

        var AverageByAgeBand = _results
        .Where(p => p.RetirementAge >= 55)
        .GroupBy(p => GetAgeBand(p.RetirementAge))
        .ToDictionary(
            g => g.Key,
            g => g.Average(x => x.AdjustedPension)
        );

        return AverageByAgeBand;
    }
        

    private static string GetAgeBand(int age)
    {
        if (age >= 55 && age <= 59) return "55-59";
        if (age >= 60 && age <= 64) return "60-64";
        if (age >= 65 && age <= 69) return "65-69";
        if (age >= 70) return "70+";
        return "Under 55";
    }

    // Exercise 3c: Members with pension above threshold
    /// <summary>
    /// Returns names of members whose adjusted pension exceeds the threshold.
    /// </summary>
    public List<string> GetHighPensionMembers(decimal threshold)
    {
        // TODO: Filter and project to names
        //throw new NotImplementedException("Exercise 3c: Implement GetHighPensionMembers");

        var HighPensionMembers = _results
        .Where(m => m.AdjustedPension > threshold)
        .Select(m => m.MemberName)
        .ToList();

        return HighPensionMembers;
    }

    // Exercise 3d: Running total using Aggregate
    /// <summary>
    /// Calculates a running total of adjusted pensions, returning each step.
    /// Returns a list of (MemberName, RunningTotal) tuples.
    /// </summary>
    public List<(string MemberName, decimal RunningTotal)> GetRunningTotals()
    {
        // TODO: Use a loop or Aggregate to build running totals
        // Hint:
        //   var runningTotal = 0m;
        //   return _results.Select(r => {
        //       runningTotal += r.AdjustedPension;
        //       return (r.MemberName, runningTotal);
        //   }).ToList();
        //throw new NotImplementedException("Exercise 3d: Implement GetRunningTotals");

        var runningTotal = 0m;
        return _results
        .Select(r =>
            {
                runningTotal += r.AdjustedPension;
                return (r.MemberName, runningTotal);
            }).ToList();
    }

    // Exercise 3e: Flatten multi-tranche pensions with SelectMany
    /// <summary>
    /// Given a list of members where each might have multiple calculation results
    /// (e.g., different tranches), flattens them into a single list and sums by member.
    /// </summary>
    public static Dictionary<string, decimal> FlattenAndSumByMember(
        List<List<CalculationResult>> trancheResults)
    {
        // TODO: Use SelectMany to flatten, then GroupBy member name and sum
        // Hint: trancheResults.SelectMany(t => t).GroupBy(r => r.MemberName)...
        //throw new NotImplementedException("Exercise 3e: Implement FlattenAndSumByMember");

        return trancheResults
        .SelectMany(t => t)
        .GroupBy(r => r.MemberName)
        .ToDictionary(
            g => g.Key,
            g => g.Sum(r => r.AdjustedPension)
        ); 
    }

    // Exercise 3f: Summary statistics
    /// <summary>
    /// Returns a summary of the calculation results.
    /// </summary>
    public SchemeSummary GetSummary()
    {
        // TODO: Calculate all summary fields using LINQ
        //throw new NotImplementedException("Exercise 3f: Implement GetSummary");
        
        // Total Members
        int totalMembers = _results.Count;

        // Total Base Pension for Scheme
        decimal totalBasePension = _results.Sum(r => r.BasePension);

        // Total Adjusted Pension for Scheme
        decimal totaAdjustedPension = _results.Sum(r => r.AdjustedPension);

        // Total Lump Sum
        decimal totalLumpSum = _results.Sum(r => r.LumpSum);

        // Avg Pension
        decimal averagePension = _results.Average(r => r.AdjustedPension);

        // Max Pension
        decimal highestPension = _results.Max(r => r.AdjustedPension);

        // Min Pension
        decimal lowestPension = _results.Min(r => r.AdjustedPension);

        //Hightest Pension Member 
        string highestPensionMember = _results
        .OrderByDescending(m => m.AdjustedPension)
        .Select(m => m.MemberName)
        .First();
    
        return new SchemeSummary{
            TotalMembers = totalMembers,
            TotalBasePension = totalBasePension,
            TotalAdjustedPension = totaAdjustedPension,
            TotalLumpSums = totalLumpSum,
            AveragePension = averagePension,
            HighestPension = highestPension,
            LowestPension = lowestPension,
            HighestPensionMember = highestPensionMember
        };
            
    }
}

public record SchemeSummary
{
    public int TotalMembers { get; init; }
    public decimal TotalBasePension { get; init; }
    public decimal TotalAdjustedPension { get; init; }
    public decimal TotalLumpSums { get; init; }
    public decimal AveragePension { get; init; }
    public decimal HighestPension { get; init; }
    public decimal LowestPension { get; init; }
    public string HighestPensionMember { get; init; } = string.Empty;
}
