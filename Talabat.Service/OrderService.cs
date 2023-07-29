using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.services;
using Talabat.Core.Specifications.order_specs;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodsRepo;
        //private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService
            //IGenericRepository<Product> productRepo,
            //IGenericRepository<DeliveryMethod> deliveryMethodsRepo,
            //IGenericRepository<Order> orderRepo
            )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            //_productRepo = productRepo;
            // _deliveryMethodsRepo = deliveryMethodsRepo;
            // _orderRepo = orderRepo;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            var orderItems = new List<OrderItem>();
            if (basket?.Items.Count>0)
            {
                foreach (var item in basket.Items)
                {
                    var productRepo = _unitOfWork.Repository<Product>();
                    var product = await productRepo.GetByIdAsync(item.Id);
                    if (product is not null)
                    {
                        var productItemOrdered = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
                        var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                        orderItems.Add(orderItem);
                    }
                }
            }

            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            DeliveryMethod deliveryMethod=new DeliveryMethod();
            var deliveryMethodRepo= _unitOfWork.Repository<DeliveryMethod>();
            if (deliveryMethodRepo is not null)
            {
                deliveryMethod = await deliveryMethodRepo.GetByIdAsync(deliveryMethodId);
            }

            var spec = new OrderWithPaymentIntentIdSpec(basket.PaymentIntentId);
            var existingOrder=await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

            if (existingOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal,basket.PaymentIntentId);

            var orders = _unitOfWork.Repository<Order>();
            if (orders is not null)
            {
                 await orders.Add(order);
                var result=await _unitOfWork.Complete();
                if (result > 0)
                {
                    return order;
                }
            }
            return null;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            var DeliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return DeliveryMethods;
        }

        public async Task<Order?> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderWithItemsAndDeliveryMethodSpec(buyerEmail);
            var order=await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            if (order is null)
            {
                return null;
            }
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail)
        {
            var spec = new OrderWithItemsAndDeliveryMethodSpec(buyerEmail);
           var orders =await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }
    }
}
