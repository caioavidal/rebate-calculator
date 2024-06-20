using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Incentives;

public class FixedCashAmountIncentiveCalculator : IncentiveCalculator
{
    public override IncentiveType IncentiveType => IncentiveType.FixedCashAmount;

    protected override RebateCalculationResult CalculateIncentive(RebateCalculation rebateCalculation)
    {
        return rebateCalculation.Rebate.Amount is 0
            ? RebateCalculationResult.Failed
            : RebateCalculationResult.Succeed(rebateCalculation.Rebate.Amount);
    }

    protected override bool ProductHasIncentiveSupport(Product product)
    {
        return product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount);
    }
}