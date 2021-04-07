using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public class ProductSearchResults : IEnumerable<ProductSearchResult>
    {
        public ProductSearchResults(IEnumerable<ProductSearchResult> results, string typoFixes)
        {
            Results = results;
            TypoFixes = typoFixes;
        }

        public IEnumerable<ProductSearchResult> Results { get; }
        public string TypoFixes { get; }

        public IEnumerator<ProductSearchResult> GetEnumerator() => Results.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Results).GetEnumerator();
    }
}
