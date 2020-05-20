using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SATNET.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace SATNET.WebApp.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class LayoutInjectorAttribute : ActionFilterAttribute
    {
        private readonly Layouts _layout;

        public LayoutInjectorAttribute(Layouts layout)
        {
            _layout = layout;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);


            //var layoutPath = "~/Views/Shared/Layouts/_Layout{0}.cshtml";
            ////var layoutPath = SessionObjects.Theme.ThemePath + "~/Views/Shared/Layouts/_Layout{0}.cshtml";
            ////var model = filterContext.HttpContext.Request["ModelDialog"];
            //var layout = _layout.ToString();

            //layoutPath = String.Format(layoutPath, layout);

            //if (filterContext.Result.GetType() == typeof(PartialViewResult))
            //{
            //    var result = filterContext.Result as PartialViewResult;
            //    if (result == null) return;
            //    //  result.MasterName = layoutPath;
            //}
            //else
            //{
            //    var result = filterContext.Result as ViewResult;
            //    if (result == null) return;
                
            //    //result.ViewData["Layout"] = layoutPath;
            //    //var res = result.ViewData["Layout"].ToString();
            //    //result.MasterName = layoutPath;
            //    //result.ViewData = new ViewDataDictionary(filterContext.Exception);

            //}
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}
