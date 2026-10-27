using Module4.TDD;
using Xunit;

namespace Module4.Tests;

/// <summary>
/// EXERCISE: Write tests for AnnualAllowanceCalculator
///
/// This file contains a few example tests to get you started.
/// Your task is to:
/// 1. Write the remaining tests FIRST (before implementing the code)
/// 2. Run the tests — they should all fail (RED)
/// 3. Implement the code in AnnualAllowanceCalculator.cs
/// 4. Run the tests again — they should pass (GREEN)
///
/// Test ideas (write at least 6 more tests):
/// - Standard AA for normal earner
/// - MPAA when hasMpaa is true
/// - Tapered AA at various income levels
/// - Tapered AA at minimum (£10,000)
/// - Threshold income below £200k (no taper even if adjusted income high)
/// - Tax charge when below AA (should be 0)
/// - Tax charge when above AA
/// - Negative pension input validation
/// - Invalid tax rate validation
/// - Exactly on threshold (boundary testing)
/// </summary>
public class AnnualAllowanceCalculatorTests
{
    private readonly AnnualAllowanceCalculator _calculator = new();

    // --- Example tests (provided) ---

    [Fact]
    public void GetAnnualAllowance_StandardEarner_Returns60000()
    {
        var result = _calculator.GetAnnualAllowance(100_000m, 100_000m);
        Assert.Equal(60_000m, result);
    }

    [Fact]
    public void GetAnnualAllowance_WithMpaa_Returns10000()
    {
        var result = _calculator.GetAnnualAllowance(100_000m, 100_000m, hasMpaa: true);
        Assert.Equal(10_000m, result);
    }

    [Fact]
    public void CalculateTaxCharge_BelowAllowance_ReturnsZero()
    {
        var result = _calculator.CalculateTaxCharge(50_000m, 60_000m, 0.40m);
        Assert.Equal(0m, result);
    }

    // --- TODO: Write your own tests below ---
    // Follow the Red-Green-Refactor cycle:
    // 1. Write a failing test
    // 2. Run it to see it fail
    // 3. Write the minimum code to make it pass
    // 4. Refactor if needed

    // Example of a test you might write:
    // [Fact]
    // public void GetAnnualAllowance_HighEarner_ReturnsTaperedAmount()
    // {
    //     // Adjusted income £300,000, threshold income £250,000
    //     // Taper: 60,000 - ((300,000 - 260,000) / 2) = 60,000 - 20,000 = 40,000
    //     var result = _calculator.GetAnnualAllowance(300_000m, 250_000m);
    //     Assert.Equal(40_000m, result);
    // }

    // TODO: Test tapered AA at various income levels
    [Fact]
    public void GetAnnualAllowance_NotEarner_ReturnsStandardAA()
    {
        // Adjusted income £0, threshold income £0
        // Taper should not apply because adjusted income is below £260,000
        var result = _calculator.GetAnnualAllowance(0m, 0m);
        Assert.Equal(60_000m, result);
    }

    [Fact]
    public void GetAnnualAllowance_HighEarner_ReturnsTaperedAmount()
    {
        // Adjusted income £300,000, threshold income £250,000
        // Taper: 60,000 - ((300,000 - 260,000) / 2) = 60,000 - 20,000 = 40,000
        var result = _calculator.GetAnnualAllowance(300_000m, 250_000m);
        Assert.Equal(40_000m, result);
    }

    // TODO: Test tapered AA minimum floor (£10,000)
    [Fact]
    public void GetAnnualAllowance_HighEarner_ReturnsMinimum10000()
    {
        //Adjusted income £500,000 - taper would give 60k - 120k = -60k, but minimum is £10,000
        var result = _calculator.GetAnnualAllowance(500_000m, 250_000m);
        Assert.Equal(10_000m, result);
    }

    // TODO: Test that taper doesn't apply when threshold income <= £200,000
    [Fact]
    public void GetAnnualAllowance_ThresholdIncomeBelow200k_NoTaper()
    {
        // Adjusted income £300,000, threshold income £150,000
        // Taper should not apply because threshold income is below £200,000
        var result = _calculator.GetAnnualAllowance(300_000m, 150_000m);
        Assert.Equal(60_000m, result);}

    // TODO: Test CalculateTaxCharge above allowance
    [Fact]
    public void CalculateTaxCharge_AboveAllowance_ReturnsTaxableAmount()
    {
        // Tax charge = (pensionInput - annualAllowance) × marginalTaxRate
        // Pension input £80,000, annual allowance £60,000, marginal tax rate 40%
        // Excess = 80,000 - 60,000 = 20,000
        // Tax charge = 20,000 * 0.40 = 8,000
        var result = _calculator.CalculateTaxCharge(80_000m, 60_000m, 0.40m);
        Assert.Equal(8_000m, result);
    }

    // TODO: Test CalculateTaxChargeSafe with negative input
    [Fact]
    public void CalculateTaxChargeSafe_NegativePensionInput_ThrowsArgumentException()
    {
        // Pension input must be >= 0
        // Test with a negative pension input
        Assert.Throws<ArgumentException>(() => _calculator.CalculateTaxChargeSafe(-1m, 60_000m, 0.40m));
    }

    // TODO: Test CalculateTaxChargeSafe with invalid tax rate
    [Fact]
    public void CalculateTaxChargeSafe_InvalidTaxRate_ThrowsArgumentException()
    {
        // Tax rate must be between 0 and 1
        // Test with a tax rate greater than 1 and less than 0
        Assert.Throws<ArgumentException>(() => _calculator.CalculateTaxChargeSafe(80_000m, 60_000m, 1.5m));
        Assert.Throws<ArgumentException>(() => _calculator.CalculateTaxChargeSafe(80_000m, 60_000m, -0.1m));
    }
    // TODO: Test boundary case — exactly on threshold
    [Fact]
    public void GetAnnualAllowance_ExactlyOnThreshold_ReturnsStandardAA()
    {
        // Adjusted income £260,000, threshold income £200,000
        // Taper should not apply because adjusted income is not greater than £260,000
        var result = _calculator.GetAnnualAllowance(260_000m, 200_000m);
        Assert.Equal(60_000m, result);
    }

    [Fact]
    public void CalculateTaxChargeSafe_ExactlyOnAllowance_ReturnsZero()
    {
        // Pension input exactly equal to annual allowance
        // Tax charge should be 0
        var result = _calculator.CalculateTaxChargeSafe(60_000m, 60_000m, 0.40m);
        Assert.Equal(0m, result);
    }

    [Fact]
    public void CalculateTaxChargeSafe_ExactlyOnAllowanceWithInvalidTaxRate_ThrowsArgumentException()
    {
        // Pension input exactly equal to annual allowance but invalid tax rate
        // Should throw ArgumentException due to invalid tax rate
        Assert.Throws<ArgumentException>(() => _calculator.CalculateTaxChargeSafe(60_000m, 60_000m, 1.5m));
    }
    [Fact]
    public void CalculateTaxChargeSafe_ExactlyOnAllowanceWithNegativePensionInput_ThrowsArgumentException()
    {
        // Pension input exactly equal to annual allowance but negative pension input
        // Should throw ArgumentException due to negative pension input
        Assert.Throws<ArgumentException>(() => _calculator.CalculateTaxChargeSafe(-60_000m, 60_000m, 0.40m));
    }

    [Fact]
    public void CalculateTaxChargeSafe_BelowOnAllowance_ReturnsZero()
    {
        // Pension input below annual allowance with valid tax rate
        // Tax charge should be 0
        var result = _calculator.CalculateTaxChargeSafe(50_000m, 60_000m, 0.40m);
        Assert.Equal(0m, result);
    }





}
