using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service.Interface;
using SATNET.WebApp.Areas.Identity.Data;
using SATNET.WebApp.Helpers;
using SATNET.WebApp.Mappings;
using SATNET.WebApp.Models.Invoice;
using SATNET.WebApp.Models.Report;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Compression;


namespace SATNET.WebApp.Controllers
{
    [Authorize]
    public class SOInvoiceController : Base2Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IService<SOInvoice> _invoiceService;
        private readonly IService<Site> _siteService;
        private readonly IService<Lookup> _lookupService;
        private readonly IMapper _mapper;
        private static string invoiceHtml;
        public SOInvoiceController(IWebHostEnvironment hostingEnvironment, IService<SOInvoice> invoiceService, IService<Site> siteService, IService<Lookup> lookupService, UserManager<ApplicationUser> userManager, IMapper mapper, IService<Customer> customerService) : base(customerService, userManager)
        {
            _siteService = siteService;
            _invoiceService = invoiceService;
            _mapper = mapper;
            _lookupService = lookupService;
            _hostingEnvironment = hostingEnvironment;
        }
        [Authorize(Policy = "ReadOnlySOInvoicePolicy")]
        public async Task<IActionResult> Index()
        {
            var invoiceStatuses = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.InvoiceStatus) });

            ViewBag.InvoiceStatusSelectList = new SelectList(invoiceStatuses, "Id", "Name");
            List<Customer> customers = new List<Customer>();
            int customerId = await GetCustomerId();
            if (customerId == 0)
            {
                customers = await GetCustomerList(new Customer());
                ViewBag.CustomerSelectList = new SelectList(customers, "Id", "Name");
            }
            var model = await GetInvoiceList(customerId, 0,DateTime.MinValue, DateTime.MinValue);

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "ReadOnlySOInvoicePolicy")]
        public async Task<IActionResult> ViewInvoice(int id)
        {
            string viewName = "";
            ViewBag.InvoiceId = id;
            SOInvoiceViewModel model = new SOInvoiceViewModel();
            var serviceResult = await _invoiceService.Get(id);
            if (serviceResult != null)
            {
                model = _mapper.Map<SOInvoiceViewModel>(serviceResult);
                viewName = GetInvoiceViewName(serviceResult.RequestTypeId, "view");
            }
            return View(viewName, model);
        }

        public async Task<IActionResult> FilterInvoiceList(int customerId, int siteId, DateTime startDate, DateTime endDate)
        {
            var model = await GetInvoiceList(customerId, siteId, startDate, endDate);
            return PartialView("_List", model);
        }

        private async Task<List<SOInvoiceViewModel>> GetInvoiceList(int customerId, int siteId, DateTime startDate, DateTime endDate)
        {
            List<SOInvoiceViewModel> objList = new List<SOInvoiceViewModel>();
            var serviceResult = await _invoiceService.List(new SOInvoice() { CustomerId = customerId, SiteId=siteId, StartDate = startDate, EndDate=endDate });
            if (serviceResult.Any())
            {
                objList = SOInvoiceMapping.GetListViewModel(serviceResult);
            }
            return objList;
        }

        //[HttpGet]
        //[Authorize(Policy = "ReadOnlySOInvoicePolicy")]
        //public async Task<IActionResult> GetInvoiceHtml(int invoiceId)
        //{
        //    string viewName = "";
        //    ViewBag.InvoiceId = invoiceId;
        //    SOInvoiceViewModel model = new SOInvoiceViewModel();
        //    var serviceResult = await _invoiceService.Get(invoiceId);
        //    if (serviceResult != null)
        //    {
        //        model = _mapper.Map<SOInvoiceViewModel>(serviceResult);
        //        viewName = GetInvoiceViewName(serviceResult.RequestTypeId, "pdf");
        //    }
        //    invoiceHtml = RenderViewToString(this, viewName, model);

        //    return Json(true);
        //}


        private string GetInvoiceViewName(int requestTypeId, string type)
        {
            string viewName = "";
            switch (requestTypeId)
            {
                case 1: //Activation
                case 32://Re-Activation
                    if (type == "view")
                    {
                        viewName = "Detail/Activation";
                    }
                    else
                    {
                        viewName = "Detail/Activation";
                    }
                    break;

                case 2://Termination
                    viewName = "Detail/Termination";
                    break;
                case 3://Upgrade
                    viewName = "Detail/Upgrade";
                    break;
                case 4://Downgrade
                    viewName = "Detail/Downgrade";
                    break;
                case 5://Token Top up
                    viewName = "Detail/Token";
                    break;
                case 6://Lock
                    viewName = "Detail/Lock";
                    break;
                case 7://UnLock
                    viewName = "Detail/Unlock";
                    break;
                case 8://Other
                    viewName = "Detail/Other";
                    break;
                case 67://Change Plan
                    viewName = "Detail/ChangePlan";
                    break;
                case 68://Change IP
                    viewName = "Detail/ChangeIP";
                    break;
                case 69://Modem Swap
                    viewName = "Detail/ModemSwap";
                    break;
            }
            return viewName;
        }

        //[HttpGet]
        //public IActionResult DownloadPDF()
        //{
        //    //Initialize HTML to PDF converter
        //    HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);
        //    WebKitConverterSettings settings = new WebKitConverterSettings();
        //    //Set print media type
        //    settings.MediaType = MediaType.Print;
        //    //Set the page orientation 
        //    settings.Orientation = PdfPageOrientation.Portrait;
        //    //Set WebKit path
        //    string contentRootPath = _hostingEnvironment.ContentRootPath;
        //    settings.WebKitPath = contentRootPath + "/QtBinariesWindows/";
        //    settings.EnableJavaScript = true;
        //    settings.EnableHyperLink = true;
        //    //Assign WebKit settings to HTML converter
        //    htmlConverter.ConverterSettings = settings;
        //    //HTML string and base URL 
        //    //string htmlText = "<html><body Align='Left'><br><p> <font size='12'>As-Salam-o-Alikum! </p></font> </body></html>";
        //    string baseUrl = _hostingEnvironment.WebRootPath;
        //    baseUrl = baseUrl + "/htmlToPdfFiles/";
        //    //Convert a URL to PDF with HTML converter
        //    string testHtmlText = "<html><body><img src=\"logo-icon.gif\" alt=\"logo-icon\" width=\"200\" height=\"70\"><p> As-Salam-o-Alikum!</p></body></html>";
        //    PdfDocument document = htmlConverter.Convert(testHtmlText, baseUrl);
        //    //PdfDocument document = htmlConverter.Convert(invoiceHtml, baseUrl);
        //    //Save and close the PDF document
        //    MemoryStream stream = new MemoryStream();
        //    document.Save(stream);
        //    document.Close(true);
        //    return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "HTMLtoPDF.pdf");
        //}

        //[HttpGet]
        //public IActionResult DownloadPDF()
        //{
        //    //Initialize HTML to PDF converter
        //    HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);
        //    WebKitConverterSettings settings = new WebKitConverterSettings();
        //    settings.EnableForm = true;
        //    //Set cookie name
        //    //string cookieName = ".AspNetCore.Identity.Application";
        //    ////Get cookie value from HttpRequest object for the requested page
        //    //string cookieValue = string.Empty;
        //    //if (Request.Cookies[cookieName] != null)
        //    //{
        //    //    cookieValue = Request.Cookies[cookieName];
        //    //}
        //    //settings.Cookies.Add(cookieName, cookieValue);
        //    //Set print media type
        //    settings.MediaType = MediaType.Print;
        //    //Set the page orientation 
        //    settings.Orientation = PdfPageOrientation.Portrait;
        //    //Set WebKit path
        //    string contentRootPath = _hostingEnvironment.ContentRootPath;
        //    settings.WebKitPath = contentRootPath + "/QtBinariesWindows/";
        //    settings.EnableJavaScript = true;
        //    settings.EnableHyperLink = true;
        //    //Assign WebKit settings to HTML converter
        //    htmlConverter.ConverterSettings = settings;
        //    //HTML string and base URL 
        //    //string htmlText = "<html><body Align='Left'><br><p> <font size='12'>As-Salam-o-Alikum! </p></font> </body></html>";
        //    string baseUrl = _hostingEnvironment.WebRootPath;
        //    baseUrl = baseUrl + "/htmlToPdfFiles/";
        //    //Convert a URL to PDF with HTML converter
        //    //string testHtmlText = "<html><body><img src=\"logo-icon.png\" alt=\"logo-icon\" width=\"200\" height=\"70\"><p><bold> As-Salam-o-Alikum!</bold></p></body></html>";
        //    //PdfDocument document = htmlConverter.Convert(testHtmlText, baseUrl);
        //    PdfDocument document = htmlConverter.ConvertPartialHtml(invoiceHtml, baseUrl,"");

        //    //PdfDocument document = htmlConverter.Convert("https://localhost:44394/SOInvoice/ViewInvoice?id=7454/");
        //    //Save and close the PDF document
        //    MemoryStream stream = new MemoryStream();
        //    document.Save(stream);
        //    document.Close(true);
        //    return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "HTMLtoPDF.pdf");
        //}
        [HttpGet]
        public IActionResult DownloadPDF(int invoiceId)
        {
            //Initialize HTML to PDF converter
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);
            WebKitConverterSettings settings = new WebKitConverterSettings();
            settings.EnableForm = true;
            //Set cookie name
            string cookieName = ".AspNetCore.Identity.Application";
            //Get cookie value from HttpRequest object for the requested page
            string cookieValue = string.Empty;
            if (Request.Cookies[cookieName] != null)
            {
                cookieValue = Request.Cookies[cookieName];
            }
            settings.Cookies.Add(cookieName, cookieValue);
            //Set print media type
            settings.MediaType = MediaType.Print;
            //Set the page orientation 
            settings.Orientation = PdfPageOrientation.Portrait;
            //Set WebKit path
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            settings.WebKitPath = contentRootPath + "/QtBinariesWindows/";
            settings.EnableJavaScript = true;
            settings.EnableHyperLink = true;
            //Assign WebKit settings to HTML converter
            htmlConverter.ConverterSettings = settings;
            //Convert a URL to PDF with HTML converter
            PdfDocument document = htmlConverter.ConvertPartialHtml("https://localhost:44394/SOInvoice/ViewInvoice?id=" + invoiceId, "divInvoicePdf");//local
            //PdfDocument document = htmlConverter.ConvertPartialHtml("http://usatbillingapp.westeurope.cloudapp.azure.com/SOInvoice/ViewInvoice?id=" + invoiceId, "divInvoicePdf");//live
            //Save and close the PDF document
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            document.Close(true);
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "HTMLtoPDF.pdf");
        }

    }
}
