using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public class ProductSearchResults : IEnumerable<Product>
    {
        public ProductSearchResults(IEnumerable<Product> results, string typoFixes)
        {
            Results = results;
            TypoFixes = typoFixes;
        }

        public IEnumerable<Product> Results { get; }
        public string TypoFixes { get; }

        public IEnumerator<Product> GetEnumerator() => Results.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Results).GetEnumerator();
    }
}
