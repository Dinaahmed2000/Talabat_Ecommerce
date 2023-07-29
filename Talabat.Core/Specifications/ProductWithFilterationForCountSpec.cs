using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithFilterationForCountSpec:BaseSpecification<Product>
    {
        public ProductWithFilterationForCountSpec(ProductSpecParams productSpecParams):
             base(p=>
                    (string.IsNullOrEmpty(productSpecParams.Search) || p.Name.ToLower().Contains(productSpecParams.Search)) &&
                    (!productSpecParams.brandId.HasValue || p.ProductBrandId== productSpecParams.brandId.Value) &&
                    (!productSpecParams.typeId.HasValue || p.ProductTypeId == productSpecParams.typeId.Value)
            )

        {

        }
    }
}
