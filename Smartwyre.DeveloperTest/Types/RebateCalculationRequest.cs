namespace Smartwyre.DeveloperTest.Types;

public class RebateCalculationRequest
{
    public string RebateIdentifier { get; set; }

    public string ProductIdentifier { get; set; }

    public decimal Volume { get; set; }
}