namespace Smartwyre.DeveloperTest.Types;

public class RebateCalculationResult
{
    public bool Success { get; init; }
    public decimal Amount { get; init; }
    public static RebateCalculationResult Failed => new();

    public static RebateCalculationResult Succeed(decimal amount)
    {
        return new RebateCalculationResult { Success = true, Amount = amount };
    }
}