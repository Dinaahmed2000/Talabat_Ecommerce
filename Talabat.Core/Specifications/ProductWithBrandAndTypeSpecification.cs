using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecification:BaseSpecification<Product>
    {
        //This constructor is used to get all products
        public ProductWithBrandAndTypeSpecification(ProductSpecParams productSpecParams) :
            base(p=>
                    (string.IsNullOrEmpty(productSpecParams.Search) || p.Name.ToLower().Contains(productSpecParams.Search)) &&
                    (!productSpecParams.brandId.HasValue || p.ProductBrandId== productSpecParams.brandId.Value) &&
                    (!productSpecParams.typeId.HasValue || p.ProductTypeId == productSpecParams.typeId.Value)
            )
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);

            AddOrderBy(p => p.Name);

            if (!string.IsNullOrEmpty(productSpecParams.sort))
            {
                switch (productSpecParams.sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;

                }
            }

            ApplyPagination(productSpecParams.pageSize*(productSpecParams.pageIndex-1), productSpecParams.pageSize);

        }

        //This constructor is used to get a specific product by id
        public ProductWithBrandAndTypeSpecification(int id):base(p=>p.Id == id)
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
        }
    }
}
