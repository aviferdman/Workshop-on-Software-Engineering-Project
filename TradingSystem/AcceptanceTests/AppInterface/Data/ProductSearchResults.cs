using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public class ProductSearchResults : IEnumerable<ProductId>
    {
        public ProductSearchResults(IEnumerable<ProductId> results, string typoFixes)
        {
            Results = results;
            TypoFixes = typoFixes;
        }

        public IEnumerable<ProductId> Results { get; }
        public string TypoFixes { get; }

        public IEnumerator<ProductId> GetEnumerator() => Results.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Results).GetEnumerator();
    }
}
