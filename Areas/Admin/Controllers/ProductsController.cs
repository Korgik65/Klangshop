using Klangshop.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Klangshop.Areas.Admin.ViewModels;
using Klangshop.Models.ViewModels;
using System.IO;
using PagedList;
using System.Web.Helpers;

namespace Klangshop.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        // GET: Admin/Products
        
        public ActionResult Index(int? page, int? catId, string searchname)
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
            var onePageOfProducts = ProductListVM.ToPagedList(pageNumber, pageSize: 12);
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

        // GET: Admin/Products/AddProduct
        [HttpGet]
        public ActionResult AddProduct()
        {
            //Объявление модели данных
            ProductVM model = new ProductVM();

            //Добавление списка категорий из бд в модель
            using(KlangshopContext db = new KlangshopContext())
            {
                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Type");
            }

            //Возвращение представления
            return View(model);
        }

        // POST: Admin/Products/AddProduct
        [HttpPost]
        public ActionResult AddProduct(ProductVM model, HttpPostedFileBase file)
        {
            //Проверка модели на валидность
            if (!ModelState.IsValid)
            {
                using(KlangshopContext db = new KlangshopContext())
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Type");
                    return View(model);
                }
            }

            //Проверка продукта на уникальность
            using (KlangshopContext db = new KlangshopContext())
            {
                if(db.Products.Any(x => x.Name == model.Name))
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Type");
                    ModelState.AddModelError("", "Товар з такою назвою вже існує!");
                    return View(model);
                }
            }

            //Объявление ПродуктИд
            int id;

            //Инициализация и сохранение модели на основе Продуктс
            using (KlangshopContext db = new KlangshopContext())
            {
                Product product = new Product();

                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;
                product.Available = model.Available;
                product.Maker = model.Maker;
                product.MakerSite = model.MakerSite;

                Category category = db.Categories.FirstOrDefault(x => x.Id == model.CategoryId);

                db.Products.Add(product);
                db.SaveChanges();

                id = product.Id;
            }

            //Вывод сообщения в ТемпДата
            TempData["SM"] = "Товар успішно додано!";

            #region Upload Image

            //Создание необходимых ссылок на директории
            var originalDirectory = new DirectoryInfo(string.Format($"{Server.MapPath(@"\")}Images\\Uploads"));

            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");

            //Проверка наличие директорий (если нет, то создание)
            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);
            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);
            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);

            //Проверка на загрузку файла
            if(file != null && file.ContentLength > 0)
            {
                //Получение расширения файла
                string ext = file.ContentType.ToLower();

                //Проверка расширение файла
                if(ext != "image/jpg" &&
                   ext != "image/jpeg" &&
                   ext != "image/png" &&
                   ext != "image/pjpeg" &&
                   ext != "image/gif" &&
                   ext != "image/x-png")
                {
                    using(KlangshopContext db = new KlangshopContext())
                    {
                        model.Categories = new SelectList(db.Categories.ToList(), "Id", "Type");
                        ModelState.AddModelError("", "Зображення не завантажено - не вірний формат файлу");
                        return View(model);
                    }
                }

                //Переменная с именем изображения
                string imageName = file.FileName;

                //Сохранение имени изображения в модель
                using(KlangshopContext db = new KlangshopContext())
                {
                    Product product = db.Products.Find(id);
                    product.Image = imageName;

                    db.SaveChanges();
                }

                //Назначение пути к изображению
                var path = string.Format($"{pathString2}\\{imageName}");

                //Сохранение изображения
                file.SaveAs(path);

            }
            #endregion

            //Переадресация пользователя
            return RedirectToAction("AddProduct");
        }

        // GET: Admin/Products/EditProduct/id
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            //Объявление модели ПродуктВМ
            ProductVM model;

            using (KlangshopContext db = new KlangshopContext())
            {
                //Получение продукта
                Product product = db.Products.Find(id);

                //Проверка на доступность продукта
                if(product == null)
                {
                    return Content("Цей товар недоступний");
                }

                //Инициализация модели данными
                model = new ProductVM(product);

                //Создание списка категорий
                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Type");

                //Получение всех изображений из галереи
                model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery"))
                    .Select(fn => Path.GetFileName(fn));
            }

            //Возвращение представления
            return View(model);
        }

        // POST: Admin/Products/EditProduct/id
        [HttpPost]
        public ActionResult EditProduct(ProductVM model, HttpPostedFileBase file)
        {
            //Получение ид товара
            int id = model.Id;

            //Заполнение списка категориями и изображениями
            using(KlangshopContext db = new KlangshopContext())
            {
                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Type");
                
            }
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery"))
                .Select(fn => Path.GetFileName(fn));

            //Проверка модели на валидность
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Проверка имени товара на уникальность
            using(KlangshopContext db = new KlangshopContext())
            {
                if (db.Products.Where(x => x.Id != id).Any(x => x.Name == model.Name))
                {
                    ModelState.AddModelError("", "Товар з такою назвою вже існує!");
                    return View(model);
                }
            }

            //Обновление товара
            using(KlangshopContext db = new KlangshopContext())
            {
                Product product = db.Products.Find(id);

                product.Name = model.Name;
                product.CategoryId = model.CategoryId;
                product.Description = model.Description;
                product.Price = model.Price;
                product.Maker = model.Maker;
                product.MakerSite = model.MakerSite;
                product.Available = model.Available;
                product.Image = model.Image;
                product.AnalogVihod = model.AnalogVihod;
                product.Deka = model.Deka;
                product.Efect = model.Efect;
                product.Forma = model.Forma;
                product.Grif = model.Grif;
                product.Kalibr = model.Kalibr;
                product.KilLad = model.KilLad;
                product.KilStrun = model.KilStrun;
                product.Kolir = model.Kolir;
                product.Material = model.Material;
                product.Potuzhnist = model.Potuzhnist;
                product.Priznach = model.Priznach;
                product.Rozmir = model.Rozmir;
                product.Typ = model.Typ;
                product.Vaga = model.Vaga;
                product.Zvukoznim = model.Zvukoznim;

                db.SaveChanges();
            }

            //Сообщение ТемпДата
            TempData["SM"] = "Товар успішно змінено!";

            //Логика обработки изображения
            #region Image Upload

            //Проверка файла на загрузку
            if (file != null && file.ContentLength > 0)
            {
                //Получение расширения файла
                string ext = file.ContentType.ToLower();

                //Проверка расширения
                if (ext != "image/jpg" &&
                   ext != "image/jpeg" &&
                   ext != "image/png" &&
                   ext != "image/pjpeg" &&
                   ext != "image/gif" &&
                   ext != "image/x-png")
                {
                    using (KlangshopContext db = new KlangshopContext())
                    {
                        ModelState.AddModelError("", "Зображення не завантажено - не вірний формат файлу");
                        return View(model);
                    }
                }

                //Установка пути загрузки
                var originalDirectory = new DirectoryInfo(string.Format($"{Server.MapPath(@"\")}Images\\Uploads"));
                var pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());

                //Удаление существующих файлов и директорий
                DirectoryInfo directoryInfo = new DirectoryInfo(pathString1);

                foreach (var item in directoryInfo.GetFiles())
                {
                    item.Delete();
                }

                //Сохранение name изображения
                string imageName = file.FileName;

                using(KlangshopContext db = new KlangshopContext())
                {
                    Product product = db.Products.Find(id);
                    product.Image = imageName;

                    db.SaveChanges();
                }

                //Сохранение оригинала и превью версии
                var path = string.Format($"{pathString1}\\{imageName}");

                //Сохранение изображения
                file.SaveAs(path);
            }

            #endregion

            //Переадресация пользователя
            return RedirectToAction("EditProduct");
        }

        // POST: Admin/Products/DeleteProduct/id
        public ActionResult DeleteProduct(int id)
        {
            //Удаление товара из бд
            using (KlangshopContext db = new KlangshopContext())
            {
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
            }

            //Удаление изображения
            var originalDirectory = new DirectoryInfo(string.Format($"{Server.MapPath(@"\")}Images\\Uploads"));
            var pathString = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());

            if (Directory.Exists(pathString))
                Directory.Delete(pathString, true);

            //Переадресация пользователя
            return RedirectToAction("Index");
        }

        // POST: Admin/Products/SaveGalleryImgs/id
        [HttpPost]
        public void SaveGalleryImages(int id)
        {
            //Перебирание всех полученых файлов
            foreach (string fileName in Request.Files)
            {
                //Инициализация файлов
                HttpPostedFileBase file = Request.Files[fileName];

                //Проверка на нулл
                if (file != null && file.ContentLength > 0)
                {
                    //Назначение путей к директориям
                    var originalDirectory = new DirectoryInfo(string.Format($"{Server.MapPath(@"\")}Images\\Uploads"));
                    var pathString = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");

                    //Назначение путей изображений
                    var path = string.Format($"{pathString}\\{file.FileName}");

                    //Сохраняем ориг изображения
                    file.SaveAs(path);
                    //WebImage img = new WebImage(file.InputStream);
                    //img.Resize(200, 200);
                    //img.Save(path);

                }
            }
        }

        // POST: Admin/Products/DeleteImage/id/imageName
        [HttpPost]
        public void DeleteImage(int id, string imageName)
        {
            string fullPath = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/" + imageName);
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

        }
    }

}