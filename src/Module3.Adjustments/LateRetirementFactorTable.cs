namespace Module3.Adjustments;

/// <summary>
/// Provides late retirement factors by age.
/// Late retirement factors are greater than 1.0 — they increase the pension
/// when a member retires after Normal Pension Age (65).
///
/// Typical factors:
///   Age 65 = 1.00, Age 66 = 1.05, Age 67 = 1.11,
///   Age 68 = 1.17, Age 69 = 1.24, Age 70 = 1.31
/// </summary>
public class LateRetirementFactorTable : IFactorTable
{
    // TODO: Create a Dictionary<int, decimal> with late retirement factors
    // Factors should be > 1.0 for ages above 65
    private readonly Dictionary<int, decimal> _factors = new()
    {
        { 65, 1.00m },
        { 66, 1.05m },
        { 67, 1.11m },
        { 68, 1.17m },
        { 69, 1.24m },
        { 70, 1.31m }
    };

    public decimal GetFactor(int age, string factorType)
    {
        // TODO: Look up the factor by age
        //throw new NotImplementedException("Implement GetFactor");

        if (_factors.TryGetValue(age, out decimal factor))
        {
            return factor;
        }

        throw new KeyNotFoundException($"No late retirement factor found for age: {age}");
    }

    public bool HasFactor(int age, string factorType)
    {
        // TODO: Check if the age exists
        // throw new NotImplementedException("Implement HasFactor");
        return _factors.ContainsKey(age);
    }
}
