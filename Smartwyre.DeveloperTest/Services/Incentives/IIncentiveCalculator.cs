using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.Incentives;

public interface IIncentiveCalculator
{
    IncentiveType IncentiveType { get; }
    RebateCalculationResult Calculate(RebateCalculation rebateCalculation);
}