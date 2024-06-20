using System.Collections.Generic;
using System.Linq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

public class ProductDataStoreMock : IProductDataStore
{
    private readonly List<Product> _products = new();

    public Product GetProduct(string productIdentifier)
    {
        return _products.FirstOrDefault(x => x.Identifier == productIdentifier);
    }

    public void AddProduct(Product product)
    {
        _products.Add(product);
    }
}