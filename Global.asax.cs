using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using Klangshop.Models.ViewModels;
using Klangshop.Models.Data;
using System.Security.Principal;

namespace Klangshop
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //Метод обработки запросов аутентификации
        protected void Application_AuthenticateRequest()
        {
            //Проверка авторизации пользователя
            if (User == null)
                return;

            //Получение имени пользователя
            string userName = Context.User.Identity.Name;

            //Объявление массива ролей
            string[] roles = null;

            using(KlangshopContext db = new KlangshopContext())
            {
                //Заполнение массива ролями
                Customer customer = db.Customers.FirstOrDefault(x => x.Email == userName);

                if (customer == null)
                    return;

                roles = db.UserRoles.Where(x => x.UserId == customer.Id).Select(x => x.Role.Name).ToArray();
            }

            //Создание объекта интерфейса ИПринципал
            IIdentity userIdentity = new GenericIdentity(userName);
            IPrincipal newUserObj = new GenericPrincipal(userIdentity, roles);

            //Объявление и инициализация данных Контекст.Юзер
            Context.User = newUserObj;
        }
    }
}
