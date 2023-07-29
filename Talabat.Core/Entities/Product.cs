using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Product :BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }

        //[ForeignKey("ProductBrand")]
        public int ProductBrandId { get; set; }  //foreign key :not allow null
        public ProductBrand ProductBrand { get; set; } //Navigational Property => one

        //[ForeignKey("ProductType")]
        public int ProductTypeId { get; set; }   //foreign key :not allow null
        public ProductType ProductType { get; set; } //Navigational Property => one
    }
}
