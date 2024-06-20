using System;
using System.Collections.Generic;
using System.Linq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services.Incentives;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore)
    : IRebateService
{
    //adding new incentives is easy as adding to this list.
    //it could be a dictionary to avoid type conflict and increase efficiency when picking the incentive calculator.
    //In a bigger project this can be registered to the DI and resolved in the constructor as a List
    private readonly List<IIncentiveCalculator> _incentivesCalculators =
    [
        new AmountPerUomIncentiveCalculator(),
        new FixedCashAmountIncentiveCalculator(),
        new FixedRateRebateIncentiveCalculator()
    ];

    public RebateCalculationResult Calculate(RebateCalculationRequest calculationRequest)
    {
        ArgumentNullException.ThrowIfNull(calculationRequest);

        var rebate = rebateDataStore.GetRebate(calculationRequest.RebateIdentifier);
        var product = productDataStore.GetProduct(calculationRequest.ProductIdentifier);

        if (rebate is null || product is null) return RebateCalculationResult.Failed;

        var rebateCalculation = new RebateCalculation
        {
            Rebate = rebate,
            Product = product,
            Volume = calculationRequest.Volume
        };

        var incentiveCalculator = _incentivesCalculators.FirstOrDefault(x => x.IncentiveType == rebate.Incentive);

        var rebateCalculationResult =
            incentiveCalculator?.Calculate(rebateCalculation) ?? RebateCalculationResult.Failed;

        if (rebateCalculationResult.Success)
        {
            var storeRebateDataStore = new RebateDataStore();
            storeRebateDataStore.StoreCalculationResult(rebate, rebateCalculationResult.Amount);
        }

        return rebateCalculationResult;
    }
}