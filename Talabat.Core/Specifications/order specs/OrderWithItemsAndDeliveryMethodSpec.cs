using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.order_specs
{
    public class OrderWithItemsAndDeliveryMethodSpec:BaseSpecification<Order>
    {
        public OrderWithItemsAndDeliveryMethodSpec(string email):base(o=>o.BuyerEmail == email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
            AddOrderByDescending(o => o.OrderDate);
        }
        public OrderWithItemsAndDeliveryMethodSpec(string email,int orderid) : 
            base(o => o.BuyerEmail == email && o.Id == orderid)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }
    }
}
