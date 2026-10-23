namespace Module2.MemberData;

public class MemberQueries
{
    private readonly List<Member> _members;

    public MemberQueries(List<Member> members)
    {
        _members = members;
    }

    // Exercise 1: Filter members by scheme type
    /// <summary>
    /// Returns all members belonging to the specified scheme type.
    /// </summary>
    public List<Member> GetMembersByScheme(string schemeType)
    {
        // TODO: Use .Where() to filter members by SchemeType
        // Example: _members.Where(m => m.SchemeType == schemeType).ToList()
        //throw new NotImplementedException("Exercise 1: Implement GetMembersByScheme");
        return _members.Where(m => m.SchemeType == schemeType).ToList();
    }

    // Exercise 2: Get active (non-deferred) members only
    /// <summary>
    /// Returns all members who are not deferred (i.e., still actively accruing).
    /// </summary>
    public List<Member> GetActiveMembers()
    {
        // TODO: Use .Where() to filter out deferred members
        //throw new NotImplementedException("Exercise 2: Implement GetActiveMembers");
        return _members.Where(m => !m.IsDeferred).ToList();
    }

    // Exercise 3: Calculate total pension liability
    // Use accrual: DB60ths = 1/60, DB80ths = 1/80, CARE = 1/49
    /// <summary>
    /// Calculates the total annual pension across all members.
    /// Uses scheme-appropriate accrual rates: DB60ths=1/60, DB80ths=1/80, CARE=1/49
    /// </summary>
    public decimal CalculateTotalLiability()
    {
        // TODO: Use .Sum() with a lambda that calculates each member's pension
        // Hint: You'll need a helper to get the accrual rate based on SchemeType
        //throw new NotImplementedException("Exercise 3: Implement CalculateTotalLiability");
        return _members.Sum(m => m.Salary * m.ServiceYears * GetAccrualRate(m.SchemeType));
    }

    // Exercise 4: Group by scheme and get average salary
    /// <summary>
    /// Groups members by SchemeType and returns the average salary per group.
    /// </summary>
    public Dictionary<string, decimal> GetAverageSalaryByScheme()
    {
        // TODO: Use .GroupBy() then .ToDictionary() with .Average()
        //throw new NotImplementedException("Exercise 4: Implement GetAverageSalaryByScheme");
        return _members
            .GroupBy(m => m.SchemeType)
            .ToDictionary(g => g.Key, g => g.Average(m => m.Salary));
    }

    // Exercise 5: Find members approaching retirement (within 5 years of age 65)
    /// <summary>
    /// Returns members whose age is 60 or above (within 5 years of NPA 65).
    /// Age is calculated from DateOfBirth relative to the provided 'today' date.
    /// </summary>
    public List<Member> GetMembersApproachingRetirement(DateTime today)
    {
        // TODO: Calculate each member's age, filter those >= 60
        // Hint: Age = today.Year - dob.Year (adjust if birthday hasn't occurred yet this year)
        //throw new NotImplementedException("Exercise 5: Implement GetMembersApproachingRetirement");
        return [.. _members.Where(m =>
        {
            int age = today.Year - m.DateOfBirth.Year;
            if (m.DateOfBirth > today.AddYears(-age)) age--;
            return age >= 60;
        })];
    }

    // Exercise 6: Sort by pension entitlement descending
    /// <summary>
    /// Returns all members ordered by their annual pension amount (highest first).
    /// </summary>
    public List<Member> GetMembersOrderedByPension()
    {
        // TODO: Use .OrderByDescending() with pension calculation in the lambda
        //throw new NotImplementedException("Exercise 6: Implement GetMembersOrderedByPension");
        return [.. _members.OrderByDescending(m => m.Salary * m.ServiceYears * GetAccrualRate(m.SchemeType))];
    }

    // Exercise 7: Project to MemberSummary records
    /// <summary>
    /// Converts each member to a MemberSummary record containing their name,
    /// calculated annual pension, and years until retirement (age 65).
    /// </summary>
    public List<MemberSummary> GetMemberSummaries(DateTime today)
    {
        // TODO: Use .Select() to create MemberSummary for each member
        // throw new NotImplementedException("Exercise 7: Implement GetMemberSummaries");
        

        return _members.Select(m=>
                {
                    var age = today.Year - m.DateOfBirth.Year;

                    if (today.AddYears(-age) < m.DateOfBirth)
                    {
                        age--;
                    }

                    return new MemberSummary(
                        m.Name,
                        m.Salary * m.ServiceYears * GetAccrualRate(m.SchemeType),
                        65 - age
                );
            }).ToList();
    }

    // Helper: Get accrual rate for a scheme type (provided for you)
    public static decimal GetAccrualRate(string schemeType) => schemeType switch
    {
        "DB60ths" => 1m / 60m,
        "DB80ths" => 1m / 80m,
        "CARE" => 1m / 49m,
        _ => throw new ArgumentException($"Unknown scheme type: {schemeType}")
    };
}
