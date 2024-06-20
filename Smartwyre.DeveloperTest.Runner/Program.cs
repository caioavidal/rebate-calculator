using System;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine(
            "Enter rebate incentive type (0 - Fixed Rate Rebate  | 1 - Amount Per Uom  | 2 - Fixed Cash Amount):");
        var incentiveTypeKey = new ReadOnlySpan<char>([Console.ReadKey().KeyChar]);

        if (!Enum.TryParse(incentiveTypeKey, out IncentiveType incentiveType))
        {
            ShowIncentiveTypeError();
            return;
        }

        var rebate = new Rebate { Identifier = Guid.NewGuid().ToString() };
        var product = new Product { Identifier = Guid.NewGuid().ToString() };

        var request = new RebateCalculationRequest
        {
            ProductIdentifier = product.Identifier,
            RebateIdentifier = rebate.Identifier
        };

        switch (incentiveType)
        {
            case IncentiveType.FixedCashAmount:
                if (!ReadAmount(rebate)) return;
                product.SupportedIncentives = SupportedIncentiveType.FixedCashAmount;
                break;

            case IncentiveType.FixedRateRebate:
                if (!ReadVolume(request)) return;
                if (!ReadPercentage(rebate)) return;
                if (!ReadPrice(product)) return;

                product.SupportedIncentives = SupportedIncentiveType.FixedRateRebate;
                break;

            case IncentiveType.AmountPerUom:
                if (!ReadVolume(request)) return;
                if (!ReadAmount(rebate)) return;
                product.SupportedIncentives = SupportedIncentiveType.AmountPerUom;
                break;

            default:
                ShowIncentiveTypeError();
                return;
        }

        rebate.Incentive = incentiveType;

        var rebateDataStore = new RebateDataStoreMock();
        rebateDataStore.AddRebate(rebate);

        var productDataStore = new ProductDataStoreMock();
        productDataStore.AddProduct(product);

        var rebateService = new RebateService(rebateDataStore, productDataStore);

        var result = rebateService.Calculate(request);
        Console.WriteLine($"Result: {result.Amount}");
        Console.ReadKey();
    }

    private static void ShowIncentiveTypeError()
    {
        Console.WriteLine("Invalid incentive type!");
        Console.ReadKey();
    }

    private static bool ReadPercentage(Rebate rebate)
    {
        Console.WriteLine("\nEnter Percentage:");
        if (!decimal.TryParse(Console.ReadLine(), out var percentage))
        {
            Console.WriteLine("\nPercentage is invalid!");
            Console.ReadKey();
            return false;
        }

        rebate.Percentage = percentage;
        return true;
    }

    private static bool ReadPrice(Product product)
    {
        Console.WriteLine("\nEnter Price:");
        if (!decimal.TryParse(Console.ReadLine(), out var price))
        {
            Console.WriteLine("\nPrice is invalid!");
            Console.ReadKey();
            return false;
        }

        product.Price = price;
        return true;
    }

    private static bool ReadVolume(RebateCalculationRequest request)
    {
        Console.WriteLine("\nEnter Volume:");
        if (!decimal.TryParse(Console.ReadLine(), out var volume))
        {
            Console.WriteLine("\nVolume is invalid!");
            Console.ReadKey();
            return false;
        }

        request.Volume = volume;
        return true;
    }

    private static bool ReadAmount(Rebate rebate)
    {
        Console.WriteLine("\nEnter Amount:");
        if (!decimal.TryParse(Console.ReadLine(), out var amount))
        {
            Console.WriteLine("\nAmount is invalid!");
            Console.ReadKey();
            return false;
        }

        rebate.Amount = amount;
        return true;
    }
}