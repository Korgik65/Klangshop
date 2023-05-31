using Klangshop.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Klangshop.Areas.Admin.ViewModels;
using Klangshop.Models.ViewModels;

namespace Klangshop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AllOrdersController : Controller
    {
        // GET: Admin/AllOrders
        public ActionResult Index()
        {
            //Инициализация модели заказов для админа
            List<OrdersForAdminVM> ordersForAdmin = new List<OrdersForAdminVM>();

            using (KlangshopContext db = new KlangshopContext())
            {
                //Инициализация модели заказов
                List<OrderVM> orders = db.Orders.ToArray().Select(x => new OrderVM(x)).ToList();

                //Перебор модели заказов
                foreach (var order in orders)
                {
                    //Инициализация словаря товаров and name
                    Dictionary<string, int> productAndAmount = new Dictionary<string, int>();
                    Dictionary<string, string> customerName = new Dictionary<string, string>();

                    //Объявление переменной общей суммы
                    decimal total = 0m;

                    //Инициализация листа деталей заказа
                    List<OrderDetails> orderDetailsList = db.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();

                    //Получение имени пользователя
                    foreach (var name in orderDetailsList)
                    {
                        Customer customer = db.Customers.FirstOrDefault(x => x.Id == order.CustomerId);
                        customerName.Add(customer.Name, customer.LName);
                    }

                    //Перебор списка товаров из деталей товара
                    foreach (var orderDetails in orderDetailsList)
                    {
                        //Получение товара
                        Product product = db.Products.FirstOrDefault(x => x.Id == orderDetails.ProductId);

                        //Получение цены товара
                        decimal price = product.Price;

                        //Получение названия товара
                        string prodName = product.Name;

                        //Добавление товара в словарь
                        productAndAmount.Add(prodName, orderDetails.Amount);

                        //Получение общей стоимости товаров
                        total += orderDetails.Amount * price;
                    }

                    //Заполнение модели заказов для админа данными
                    ordersForAdmin.Add(new OrdersForAdminVM()
                    {
                        OrderNumber = order.OrderId,
                        CustomerName = customerName,
                        TotalPrice = total,
                        ProductsAndAmount = productAndAmount,
                        Date = order.Date
                    });
                }
            }

            //Возвращение представления
            return View(ordersForAdmin);
        }
    }
}