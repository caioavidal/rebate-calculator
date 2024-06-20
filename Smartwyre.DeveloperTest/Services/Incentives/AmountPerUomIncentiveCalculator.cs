using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Incentives;

public class AmountPerUomIncentiveCalculator : IncentiveCalculator
{
    public override IncentiveType IncentiveType => IncentiveType.AmountPerUom;

    protected override RebateCalculationResult CalculateIncentive(RebateCalculation rebateCalculation)
    {
        if (rebateCalculation.Rebate.Amount == 0 || rebateCalculation.Volume == 0)
            return RebateCalculationResult.Failed;

        var calculationResult = rebateCalculation.Rebate.Amount * rebateCalculation.Volume;
        return RebateCalculationResult.Succeed(calculationResult);
    }

    protected override bool ProductHasIncentiveSupport(Product product)
    {
        return product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom);
    }
}