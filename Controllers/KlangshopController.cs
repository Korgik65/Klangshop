using Klangshop.Models.Data;
using Klangshop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Klangshop.Models.ViewModels.Account;
using System.Web.Security;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace Klangshop.Controllers
{
    public class KlangshopController : Controller
    {
        // GET: Main
        public ActionResult Main()
        {
            return View();
        }

        // GET: Klangshop/CartList
        public ActionResult CartList()
        {
            // Объявляем лист Корзины
            var cart = Session["CartList"] as List<CartVM> ?? new List<CartVM>();

            // Проверка на товары
            if (cart.Count == 0 || Session["CartList"] == null)
            {
                ViewBag.Message = "порожній!";
                return View();
            }

            // Складывание суммы и запись во ВьюБег
            decimal total = 0m;

            foreach (var item in cart)
            {
                total += item.TotalPrice;
            }

            ViewBag.GrandTotal = total;

            // Возвращение листа в представление
            return View(cart);
        }

        public ActionResult AddToCartPartial(int id)
        {
            // Объявляем лист с параметризированым типом
            List<CartVM> cart = Session["CartList"] as List<CartVM> ?? new List<CartVM>();

            // Объявляем модель
            CartVM model = new CartVM();

            using (KlangshopContext db = new KlangshopContext())
            {
                // Получаем товар
                Product prod = db.Products.Find(id);

                // Проверка есть ли уже в корзине
                var prodInCart = cart.FirstOrDefault(x => x.ProductId == id);

                // Если нет, то добавляем новый товар в корзину
                if (prodInCart == null)
                {
                    cart.Add(new CartVM()
                    {
                        ProductId = prod.Id,
                        ProductName = prod.Name,
                        Amount = 1,
                        Price = prod.Price
                    });
                }
                // Если да, добавляем единицу товара
                else
                {
                    prodInCart.Amount++;
                }
            }

            // Получаем общее кол-во, цену и добавляем в модель
            int am = 0;
            decimal price = 0m;

            foreach (var item in cart)
            {
                am += item.Amount;
                price += item.Amount * item.Price;
            }

            model.Amount = am;
            model.Price = price;

            // Сохраняем состояние корзины в сессию
            Session["CartList"] = cart;

            // Возвращаем частичное представление с моделью
            return PartialView("_AddToCartPartial", model);
        }

        public ActionResult CartPartial()
        {
            // Объявляем модель Корзины
            CartVM model = new CartVM();

            // Объявляем переменную кол-ва
            int amount = 0;

            // Объявляем переменную цены
            decimal price = 0m;

            // Проверяем сессию корзины
            if (Session["CartList"] != null)
            {
                // Получаем общее кол-во товаров и цену
                var list = (List<CartVM>)Session["CartList"];

                foreach (var item in list)
                {
                    amount += item.Amount;
                    price += item.Amount * item.Price;
                }

                model.Amount = amount;
                model.Price = price;
            }
            else
            {
                // Или устанваливаем кол-во и цену в 0
                model.Amount = 0;
                model.Price = 0m;
            }

            // Возвращаем частичное представление с моделью
            return PartialView("_CartPartial", model);
        }

        // GET: /cart/IncrementProduct
        public JsonResult IncrementProduct(int productId)
        {
            // Объявление лист карт
            List<CartVM> cart = Session["CartList"] as List<CartVM>;

            using (KlangshopContext db = new KlangshopContext())
            {
                // Получение модели КартВМ из листа
                CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

                // Добавление кол-ва
                if(model.Amount < 15)
                    model.Amount++;
                else
                {
                    model.Amount = 1;
                }
                    

                // Сохранение данных
                var result = new { am = model.Amount, price = model.Price };

                // Возвращение ответа ДжСОН с данными
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DecrementProduct(int productId)
        {
            // Объявление лист карт
            List<CartVM> cart = Session["CartList"] as List<CartVM>;

            using (KlangshopContext db = new KlangshopContext())
            {
                // Получение модели КартВМ из листа
                CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

                // Уменьшение кол-ва
                if (model.Amount > 1)
                    model.Amount--;
                else
                {
                    model.Amount = 0;
                    cart.Remove(model);
                }

                // Сохранение данных
                var result = new { am = model.Amount, price = model.Price };

                // Возвращение ответа ДжСОН с данными
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public void RemoveProduct(int productId)
        {
            // Объявление лист карт
            List<CartVM> cart = Session["CartList"] as List<CartVM>;

            using (KlangshopContext db = new KlangshopContext())
            {
                // Получение модели КартВМ из листа
                CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

                cart.Remove(model);
            }
        }

        // GET: Klangshop/ProductList
            
        public ActionResult ProductList(int? page, int? catId, string searchname)
        {
            
            //Объявление списка товаров (ProductList)
            List<ProductVM> ProductListVM;

            //Установка номера страницы
            var pageNumber = page ?? 1;

            using (KlangshopContext db = new KlangshopContext())
            {
                //Инициализация списка товаров (KlangshopContext) и заполнение
                ProductListVM = db.Products
                    .ToArray()
                    .Where(x => catId == null || catId == 0 || x.CategoryId == catId)
                    .Select(x => new ProductVM(x))
                    .ToList();

                //Заполнение категорий данными
                ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Type");

                //Установка выбранной категории
                ViewBag.SelectedCat = catId.ToString();
            }

            //Установка постраничной навигации
            var onePageOfProducts = ProductListVM.ToPagedList(pageNumber, pageSize: 9);
            ViewBag.onePageOfProducts = onePageOfProducts;

            if (searchname != null)
            {
                using (KlangshopContext db = new KlangshopContext())
                {
                    //Инициализация списка товаров (KlangshopContext) и заполнение
                    ProductListVM = db.Products
                        .ToArray()
                        .Where(x => x.Name.StartsWith(searchname))
                        .Select(x => new ProductVM(x))
                        .ToList();
                }

                //Установка постраничной навигации
                var onePageOfProducts1 = ProductListVM.ToPagedList(pageNumber, pageSize: 9);
                ViewBag.onePageOfProducts = onePageOfProducts1;

                return View(ProductListVM);
            }

            //Возвращение списка в представление
            return View(ProductListVM);
        }

        // GET: Klangshop/Product/name
        public ActionResult Product(string name)
        {
            //Объявление двух моделей
            Product product;
            ProductVM model;

            //Инициализация ид продукта
            int id = 0;

            using (KlangshopContext db = new KlangshopContext())
            {
                //Инициализация модель данными
                product = db.Products.Where(x => x.Name == name).FirstOrDefault();

                //Получение ид продукта
                id = product.Id;

                //Инициализация модели ВМ данными   
                model = new ProductVM(product);

                //model.Characteristics = new SelectList(db.Characteristics.ToList(), "Id", "Name");
                //model.ProdCharacters = new SelectList(db.ProdCharacters.ToList(), "Id", "Value");
            }

            //Получение изображений из галереи
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery")).Select(fn => Path.GetFileName(fn));

            //Возвращение в представление
            return View("Product", model);
        }

        [HttpGet]
        public ActionResult CreateOrderPartial()
        {
            return PartialView();
        }

        // GET: Klangshop/Privacy
        public ActionResult Privacy()
        {
            return View();
        }

        // GET: Klangshop/Create-Account
        [ActionName("Create-Account")]
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        // POST: Klangshop/Create-Account
        [ActionName("Create-Account")]
        [HttpPost]
        public ActionResult CreateAccount(CustomerVM model, CustomerLoginVM login)
        {
            //Проверка на валидность
            if (!ModelState.IsValid)
                return View("CreateAccount", model);

            using (KlangshopContext db = new KlangshopContext())
            {
                //Проверка почты на уникальность
                if(db.Customers.Any(x => x.Email.Equals(model.Email)))
                {
                    ModelState.AddModelError("", $"Електрона пошта {model.Email} вже використовується іншим користувачем.");
                    model.Email = "";
                    return View("CreateAccount", model);
                }

                //Создание экземпляра класса
                Customer customer = new Customer()
                {
                    Name = model.Name,
                    LName = model.LName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address,
                    Country = model.Country,
                    City = model.City,
                    Index = model.Index,
                    Password = model.Password
                };

                //Добавление данных в модель
                db.Customers.Add(customer);

                //Сохранение данных
                db.SaveChanges();

                //Добавление роли пользователя
                int id = customer.Id;

                UserRole userRole = new UserRole()
                {
                    UserId = id,
                    RoleId = 2
                };

                db.UserRoles.Add(userRole);
                db.SaveChanges();
            }

            //Сообщение в ТемпДата
            TempData["SM"] = "Ваш обліковий запис успішно створено!";

            FormsAuthentication.SetAuthCookie(login.Email, login.RememberMe);
            return Redirect(FormsAuthentication.GetRedirectUrl(login.Email, login.RememberMe));

            //Переадресация пользователя
            return RedirectToAction("Login");
        }

        // GET: Klangshop/Login
        [HttpGet]
        public ActionResult Login()
        {
            //Подтвержение не авторизации
            string email = User.Identity.Name;

            if (!string.IsNullOrEmpty(email))
                return RedirectToAction("User-Profile");

            //Возвращение представления
            return View();
        }

        // POST: Klangshop/Login
        [HttpPost]
        public ActionResult Login(CustomerLoginVM model)
        {
            //Проверка модели на валидность
            if (!ModelState.IsValid)
                return View(model);

            //Проверка пользователя на валидность
            bool isValid = false;

            using (KlangshopContext db = new KlangshopContext())
            {
                if (db.Customers.Any(x => x.Email.Equals(model.Email) && x.Password.Equals(model.Password)))
                    isValid = true;

                if (!isValid)
                {
                    ModelState.AddModelError("", "Невірні пошта або пароль.");
                    return View(model);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
                    return Redirect(FormsAuthentication.GetRedirectUrl(model.Email, model.RememberMe));
                }
            }
        }

        // GET: Klangshop/Logout
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [Authorize]
        public ActionResult CustNavPartial()
        {
            //Получение имени пользователя
            string userName = User.Identity.Name;

            //Объявление модели
            CustNavPartialVM model;

            using(KlangshopContext db = new KlangshopContext())
            {
                //Получение пользователя
                Customer customer = db.Customers.FirstOrDefault(x => x.Email == userName);

                //Заполнение модель данными из контекста
                model = new CustNavPartialVM()
                {
                    Name = customer.Name,
                    LName = customer.LName
                };
            }

            //Возвращение частичного представления
            return PartialView(model);
        }

        // GET: Klangshop/User-Profile
        [ActionName("User-Profile")]
        [HttpGet]
        [Authorize]
        public ActionResult UserProfile()
        {
            //Получение почты пользователя
            string userName = User.Identity.Name;

            //Объявление модели
            UserProfileVM model;

            using(KlangshopContext db = new KlangshopContext())
            {
                //Получение пользователя
                Customer customer = db.Customers.FirstOrDefault(x => x.Email == userName);

                //Инициализация модели данными
                model = new UserProfileVM(customer);
            }

            //Возвращение представления
            return View("UserProfile", model);
        }

        // POST: Klangshop/User-Profile
        [ActionName("User-Profile")]
        [HttpPost]
        [Authorize]
        public ActionResult UserProfile(UserProfileVM model)
        {
            bool userNameIsChanged = false;

            //Проверка на валидность
            if (!ModelState.IsValid)
                return View("UserProfile", model);

            //Проверка пароля при изменении
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Пароль не співпадає!");
                    return View("UserProfile", model);
                }
            }

            using (KlangshopContext db = new KlangshopContext())
            {
                //Получение почты пользователя
                string userName = User.Identity.Name;

                //Проверка смены имени
                if(userName != model.Email)
                {
                    userName = model.Email;
                    userNameIsChanged = true;
                }

                //Проверка почты на уникальность
                if (db.Customers.Where(x => x.Id != model.Id).Any(x => x.Email == userName))
                {
                    ModelState.AddModelError("", $"Електрона пошта {model.Email} вже використовується іншим користувачем.");
                    model.Email = "";
                    return View("UserProfile", model);
                }

                //Изменение модели контекста данных
                Customer customer = db.Customers.Find(model.Id);

                customer.Name = model.Name;
                customer.LName = model.LName;
                customer.Email = model.Email;
                customer.Phone = model.Phone;
                customer.Address = model.Address;
                customer.Country = model.Country;
                customer.City = model.City;
                customer.Index = model.Index;
                if (!string.IsNullOrWhiteSpace(model.Password))
                    customer.Password = model.Password;

                //Сохранение изменений
                db.SaveChanges();

            }

            //Сообщение в ТемпДата
            TempData["SM"] = "Дані вашого облікового запису успішно оновлено!";

            //Возвращение представления
            if (!userNameIsChanged)
                return View("UserProfile", model);
            else
                return RedirectToAction("Logout");
        }

        public ActionResult PayPalPartial()
        {
            //Получение товаров из корзины
            List<CartVM> cart = Session["CartList"] as List<CartVM>;

            //Возвращение представления
            return PartialView(cart);
        }


        // POST: Klangshop/PlaceOrder
        [HttpPost]
        public void PlaceOrder()
        {
            //Получение списка товаров из корзины
            List<CartVM> cart = Session["CartList"] as List<CartVM>;

            //Получение имени пользователя
            string userName = User.Identity.Name;

            //Объявление переменной для ордерИд
            int orderId = 0;

            using(KlangshopContext db = new KlangshopContext())
            {
                //Объявление модели Ордер
                Order order = new Order();

                //Получение Ид пользователя
                var q = db.Customers.FirstOrDefault(x => x.Email == userName);
                int customerId = q.Id;

                //Заполнение модели Ордер и сохранение
                order.CustomerId = customerId;
                order.Date = DateTime.Now;

                db.Orders.Add(order);
                db.SaveChanges();

                //Получение ордерИд
                orderId = order.OrderId;

                //Объявление модели ОрдерДетали
                OrderDetails orderDetails = new OrderDetails();

                //Заполнение данных в модель Детали
                foreach (var item in cart)
                {
                    orderDetails.OrderId = orderId;
                    orderDetails.CustomerId = customerId;
                    orderDetails.ProductId = item.ProductId;
                    orderDetails.Amount = item.Amount;

                    db.OrderDetails.Add(orderDetails);
                    db.SaveChanges();
                }
            }

            //Отправление письма о заказе на почту
            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("3a5e2641f3b200", "78c5a2860e71b8"),
                EnableSsl = true
            };
            client.Send("klangshop2022@gmail.com", "admin@example.com", "Нове замволення", $"У Вас зя'вилося нове замволення. \nНомер замовлення: {orderId} \nЗамовник: {userName}");
            Console.WriteLine("Sent");

            //Обнуление сессии
            Session["CartList"] = null;
            Response.Redirect("AllOrders");
        }

        // GET: Klangshop/AllOrders
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public ActionResult AllOrders()
        {
            //Инициализация модели OrdersForCustomers
            List<OrdersForCustomersVM> ordersForCustomers = new List<OrdersForCustomersVM>();

            using (KlangshopContext db = new KlangshopContext())
            {
                //Получение ид пользователя
                Customer customer = db.Customers.FirstOrDefault(x => x.Email == User.Identity.Name);
                int customerId = customer.Id;

                //Инициализация модели заказов
                List<OrderVM> orders = db.Orders.Where(x => x.CustomerId == customerId).ToArray().Select(x => new OrderVM(x)).ToList();

                //Перебор модели заказов
                foreach (var order in orders)
                {
                    //Инициализация словаря товаров and name
                    Dictionary<string, int> productAndAmount = new Dictionary<string, int>();

                    //Объявление переменной общей суммы
                    decimal total = 0m;

                    //Инициализация листа деталей заказа
                    List<OrderDetails> orderDetailsList = db.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();

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
                    ordersForCustomers.Add(new OrdersForCustomersVM()
                    {
                        OrderNumber = order.OrderId,
                        TotalPrice = total,
                        ProductsAndAmount = productAndAmount,
                        Date = order.Date
                    });
                }
            }

            //Возвращение представления
            return View(ordersForCustomers);
        }

    }
}