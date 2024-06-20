using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Incentives;

public class FixedRateRebateIncentiveCalculator : IncentiveCalculator
{
    public override IncentiveType IncentiveType => IncentiveType.FixedRateRebate;

    protected override RebateCalculationResult CalculateIncentive(RebateCalculation rebateCalculation)
    {
        if (rebateCalculation.Rebate.Percentage == 0 || rebateCalculation.Product.Price == 0 ||
            rebateCalculation.Volume == 0)
            return RebateCalculationResult.Failed;

        return RebateCalculationResult.Succeed(rebateCalculation.Product.Price * rebateCalculation.Rebate.Percentage *
                                               rebateCalculation.Volume);
    }

    protected override bool ProductHasIncentiveSupport(Product product)
    {
        return product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate);
    }
}