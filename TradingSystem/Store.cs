using System;
using System.Collections.Generic;

namespace TradingSystem
{

	public class Store
	{
		private ICollection<Product> products;
		private Product policy;
		public Store()
		{
			products.Add(new Product());
			new Product().Print();
		}
	}
}