﻿using System;

namespace AcceptanceTests.AppInterface.Data
{
    public class ProductIdentifiable
    {
        public ProductIdentifiable(ProductInfo productInfo)
            : this(productInfo, default) { }
        public ProductIdentifiable(ProductInfo productInfo, ProductId productId)
        {
            ProductInfo = productInfo;
            this.ProductId = productId;
        }

        public ProductInfo ProductInfo { get; }
        public ProductId ProductId { get; set; }

        public static bool DeepEquals(ProductIdentifiable p1, ProductIdentifiable p2)
        {
            return p1.ProductId == p2.ProductId && p1.ProductInfo.Equals(p2.ProductInfo);
        }

        public override bool Equals(object? obj)
        {
            return obj is ProductIdentifiable other && Equals(other);
        }
        public bool Equals(ProductIdentifiable other)
        {
            return other != null && ProductId == other.ProductId;
        }

        public override int GetHashCode()
        {
            return ProductId.GetHashCode();
        }

        public override string ToString()
        {
            return $"Product identifiable {{{ProductId}, '{ProductInfo.Name}'}}";
        }
    }
}
