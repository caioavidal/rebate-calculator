using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Incentives;

public abstract class IncentiveCalculator : IIncentiveCalculator
{
    public RebateCalculationResult Calculate(RebateCalculation rebateCalculation)
    {
        if (rebateCalculation?.Rebate is null || rebateCalculation.Product is null)
            return RebateCalculationResult.Failed;

        if (!ProductHasIncentiveSupport(rebateCalculation.Product)) return RebateCalculationResult.Failed;

        return CalculateIncentive(rebateCalculation);
    }

    public abstract IncentiveType IncentiveType { get; }

    protected abstract RebateCalculationResult CalculateIncentive(RebateCalculation rebateCalculation);
    protected abstract bool ProductHasIncentiveSupport(Product product);
}