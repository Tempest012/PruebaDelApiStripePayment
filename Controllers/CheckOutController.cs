﻿using Microsoft.AspNetCore.Mvc;
using PruebaDelApiStripePayment.Models;
using Stripe.Checkout;

namespace PruebaDelApiStripePayment.Controllers
{
    public class CheckOutController : Controller
    {
        public IActionResult Index()
        {
            List<ProductEntity> productList = new List<ProductEntity>();

            productList = new List<ProductEntity>
            {
                new ProductEntity
                {
                    Product = "Tommy Hilfiger",
                    Rate = 1500,
                    Quanity= 2,
                    ImagePath = "img/Imagen1.jpg"
                },
                  new ProductEntity
                {
                    Product = "TimeWear",
                    Rate = 1000,
                    Quanity= 1,
                    ImagePath = "img/Imagen2.jpg"
                }
            };
            return View(productList);
        }

        public IActionResult OrderConfirmation()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());


            if (session.PaymentStatus == "paid")
            {
                var transaction = session.PaymentIntentId.ToString();
                return View("Success");
            }
            return View("Login");
        }
        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }




        public IActionResult CheckOut()
        {
            List<ProductEntity> productList = new List<ProductEntity>();

            productList = new List<ProductEntity>
            {
                new ProductEntity
                {
                    Product = "Tommy Hilfiger",
                    Rate = 1500,
                    Quanity= 2,
                    ImagePath = "img/Imagen1.jpg"
                },
                  new ProductEntity
                {
                    Product = "TimeWear",
                    Rate = 1000,
                    Quanity= 1,
                    ImagePath = "img/Imagen2.jpg"
                },

                     new ProductEntity
                {
                    Product = "EstoyCansadoJefe",
                    Rate = 3000,
                    Quanity= 2,
                    ImagePath = "img/Imagen2.jpg"
                }
            };
            var domain = "http://localhost:5156/";

            var options = new SessionCreateOptions()
            {
                SuccessUrl = domain + $"CheckOut/OrderConfirmation",
                CancelUrl = domain + "CheckOut/Login",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = "harolcsnrs@gmail.com"
            };

            foreach (var item in productList)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Rate * item.Quanity),
                        Currency = "inr",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.ToString(),
                        }
                    },
                    Quantity = item.Quanity
                };
                options.LineItems.Add(sessionListItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);

            TempData["Session"] = session.Id;

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }



    }
}
