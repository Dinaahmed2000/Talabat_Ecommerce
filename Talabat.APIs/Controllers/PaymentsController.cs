using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.services;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string _whSecret = "whsec_64abd5bee82239d8c969d5aee29412950a02d926b6d528a582aea459f7d59280";

        public PaymentsController(IPaymentService paymentService,ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        [Authorize]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null)
            {
                return BadRequest(new ApiResponse(400, "A Problem With your basket"));
            }
            return Ok(basket);
        }
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], _whSecret);

            var paymentIntent =(PaymentIntent) stripeEvent.Data.Object;

            Order order;

            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    order= await _paymentService.UpdatePaymentIntentToSuccessedOrFailed(paymentIntent.Id,true);
                    _logger.LogInformation("Payment is succeeded",paymentIntent.Id);
                    break;
                case Events.PaymentIntentPaymentFailed:
                    order= await _paymentService.UpdatePaymentIntentToSuccessedOrFailed(paymentIntent.Id, false);
                    _logger.LogInformation("Payment is Failed", paymentIntent.Id);
                    break;
            }
            return Ok();
        }
    }
}
