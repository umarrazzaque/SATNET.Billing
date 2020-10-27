using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SATNET.Domain;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SATNET.WebApp.Areas.Identity.Data;
using SATNET.WebApp.Helpers;

namespace SATNET.WebApp.Controllers
{
    public class Base2Controller : Controller
    {
        private readonly IService<Customer> _customerService;
        private readonly UserManager<ApplicationUser> _userManager;
        public Base2Controller(IService<Customer> customerService, UserManager<ApplicationUser> userManager)
        {
            _customerService = customerService;
            _userManager = userManager;

        }
        public static string RenderViewToString(Controller controller, string viewName, object model = null)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions()
                );
                viewResult.View.RenderAsync(viewContext);
                string result = sw.GetStringBuilder().ToString();
                return result;
            }
        }

        protected async Task<List<Customer>> GetCustomerList(Customer obj)
        {
            return await _customerService.List(obj);
        }
        protected async Task<int> GetCustomerId()
        {
            var user = await _userManager.GetUserAsync(User);
            return Utilities.TryInt32Parse(user.CustomerId);
        }
        protected async Task<Customer> GetCustomer(int Id)
        {
            return await _customerService.Get(Id);
        }
        protected async Task<ApplicationUser> GetUser()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}
