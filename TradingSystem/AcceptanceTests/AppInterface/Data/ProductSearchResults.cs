using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AcceptanceTests.AppInterface.Data
{
    public class ProductSearchResults : IEnumerable<ProductIdentifiable>
    {
        public ProductSearchResults(IEnumerable<ProductIdentifiable> results, string? typoFixes)
        {
            Results = results;
            TypoFixes = typoFixes;
        }

        public IEnumerable<ProductIdentifiable> Results { get; }
        public string? TypoFixes { get; }

        public bool IsValid()
        {
            return Results.All(x => x.ProductId.IsValid());
        }

        public IEnumerator<ProductIdentifiable> GetEnumerator() => Results.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Results).GetEnumerator();
    }
}
