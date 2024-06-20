using System;
using System.Collections.Generic;
using System.Linq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

public class RebateDataStoreMock : IRebateDataStore
{
    private readonly List<Rebate> _rebates = new();

    public Rebate GetRebate(string rebateIdentifier)
    {
        return _rebates.FirstOrDefault(x => x.Identifier == rebateIdentifier);
    }

    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        throw new NotImplementedException();
    }

    public void AddRebate(Rebate rebate)
    {
        _rebates.Add(rebate);
    }
}