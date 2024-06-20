using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public interface IRebateService
{
    RebateCalculationResult Calculate(RebateCalculationRequest calculationRequest);
}