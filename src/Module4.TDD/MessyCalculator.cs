namespace Module4.TDD;

/// <summary>
/// REFACTORING EXERCISE
///
/// This calculator works correctly — all tests pass — but the code is messy.
/// Your task: refactor it to be cleaner while keeping all tests green.
///
/// Things to improve:
/// - Magic numbers → named constants or parameters
/// - Duplicated logic → extract methods
/// - Long method → break into smaller, focused methods
/// - Poor naming → rename for clarity
/// </summary>
public class MessyCalculator
{
    public const int NormalPensionAge = 65;
    public const decimal EarlyRetirementReductionPerYear = 0.04m;
    public const decimal LateRetirementIncreasePerYear = 0.05m;
    public const decimal CommutationFactorUnder60 = 18.5m;
    public const decimal CommutationFactor60ToNormal = 16.0m;
    public const decimal CommutationFactorOverNormal = 13.0m;



    // This method calculates everything in one go — it's correct but ugly
    static public decimal  ApplyEarlyRetirementReduction(decimal pension, int age)
    {
        // early retirement
        if (age < NormalPensionAge)
        {
            int yearsEarly = NormalPensionAge - age;
            decimal reduction = 1m - (yearsEarly * EarlyRetirementReductionPerYear);
            if (reduction < 0) reduction = 0;
            pension = pension * reduction;
        }
        return pension;
    }

    static public decimal ApplyLateRetirementIncrease(decimal pension, int age)
    {

        // late retirement
        if (age > NormalPensionAge)
        {
            int yearsLate = age - NormalPensionAge;
            decimal increase = 1m + (yearsLate * LateRetirementIncreasePerYear);
            pension = pension * increase;
        }
        return pension;
    }

    static public decimal ApplyCommutation(decimal pension, int age, bool wantLumpSum)
    {
        // Commutation factor depends on age
        decimal commFactor = age switch
        {
            < 60 => CommutationFactorUnder60,
            >= 60 and <= NormalPensionAge => CommutationFactor60ToNormal,
            > NormalPensionAge => CommutationFactorOverNormal,
        };
        // Commutation reduces pension by 25% and gives a lump sum of 25% × commutation factor
        if (wantLumpSum)
        {
            decimal lumpSum = (pension * 0.25m) * commFactor;
            pension = pension * 0.75m;
            // Note: lump sum is calculated but we only return the residual pension
            // In a real system you'd return both
        }

        // return the adjusted pension after commutation
        return pension;
    }
    public decimal CalcPension(decimal salary, int years, string type, int age, bool wantLumpSum)
    {
        decimal factor = type switch
        { 
            "60ths" => 1m / 60m,
            "80ths" => 1m/ 80m,
            "CARE" => 1m /49m,
            _ => throw new ArgumentException("Unknown scheme type")
        };

        decimal pension = salary * years * factor;

        //Early/late retirement adjustments
        if (age < NormalPensionAge)
        {
            pension = ApplyEarlyRetirementReduction(pension, age);
        }
        if (age > NormalPensionAge)
        {
            pension = ApplyLateRetirementIncrease(pension, age);
        }

        // Commutation adjustments
        if (wantLumpSum)
        {
            pension = ApplyCommutation(pension, age, wantLumpSum);
        }

        // Round to 2 decimal places for currency
        return Math.Round(pension, 2);
    }

    
    
}
