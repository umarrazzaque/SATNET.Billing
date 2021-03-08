using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SATNET.Service;
using SATNET.WebApp.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.Reflection;
using OfficeOpenXml.Style;
using Microsoft.AspNetCore.Http;
using Syncfusion.HtmlConverter;

namespace SATNET.WebApp.Controllers
{
    public class ExportController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public static byte[] fileByteArray;
        public ExportController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public string ToJulianDate()
        {
            DateTime date = DateTime.Now;
            string ret = (date.ToOADate() + 2415018.5).ToString();
            return ret;
        }
        public IActionResult PDFExport(IEnumerable<ExportModel> records)
        {
            var statusModel = new StatusModel { IsSuccess = true };
            string fileName = "Export_" + "Menu" + "_" + ToJulianDate() + ".pdf";
            try
            {
                //Create a new PDF document
                PdfDocument document = new PdfDocument();

                //Add a page to the document
                PdfPage page = document.Pages.Add();

                //Create PDF graphics for the page
                PdfGraphics graphics = page.Graphics;

                //Set the standard font
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

                //Draw the text
                graphics.DrawString("Hello World!!!", font, PdfBrushes.Black, new PointF(0, 0));

                //Saving the PDF to the MemoryStream
                MemoryStream stream = new MemoryStream();

                document.Save(stream);
                string rootFolder = _hostingEnvironment.WebRootPath;

                string path = Path.Combine(rootFolder, "Downloads", fileName);
                using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    file.Write(bytes, 0, bytes.Length);
                    stream.Close();
                }
                statusModel.ErrorCode = "File '" + fileName + "' Exported Succesfully";
            }
            catch (Exception e)
            {
                statusModel.IsSuccess = false;
                statusModel.ErrorCode = "Error in Exporting File!";
            }

            return Json(statusModel);
            //return File(stream, "application/pdf", "Sample.pdf");
        }
        public ActionResult ExcelExport(IEnumerable<ExportModel> records, string header, string menu)
        {
            var statusModel = new StatusModel { IsSuccess = true };
            var checkMenuName = menu.ToCharArray();
            for (int i = 0; i < checkMenuName.Length; i++)
            {
                if (!char.IsLetterOrDigit(checkMenuName[i]))
                {
                    checkMenuName[i] = '_';
                }
            }
            menu = new string(checkMenuName);
            var fileName = "Export_" + menu + "_" + ToJulianDate() + ".xlsx";
            try
            {
                List<string> tableHeaders = new List<string>() { "Sr. No" };
                tableHeaders.AddRange(header.Split(',').ToList());
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(menu.ToString());
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    // Header of the Excel sheet
                    int thLength = tableHeaders.Count;
                    for (int i = 1; i <= thLength; i++)
                    {
                        workSheet.Cells[1, i].Value = tableHeaders.ElementAt(i - 1);
                        #region design Header
                        //worksheet.Cells[headerRange].Style.Font.Bold = true;
                        workSheet.Cells[1, i].Style.Font.Size = 11;
                        workSheet.Cells[1, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[1, i].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkGray);
                        //worksheet.Cells[headerRange].Style.WrapText = true;
                        workSheet.Cells[1, i].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        workSheet.Cells[1, i].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        workSheet.Column(i).AutoFit();
                        #endregion
                    }
                    PropertyInfo[] propsB = typeof(ExportModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    List<string> propNames = new List<string>();
                    int recordIndex = 2;
                    foreach (var item in records)
                    {
                        workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                        Type type = item.GetType();
                        PropertyInfo[] propVal = type.GetProperties();
                        int excelColStart = 2;
                        foreach (PropertyInfo prop in propVal)
                        {
                            //propNames.Add(prop.GetValue(item).ToString());
                            var propertyVal = prop.GetValue(item);
                            propertyVal = propertyVal != null ? propertyVal.ToString() : "";
                            workSheet.Cells[recordIndex, excelColStart].Value = propertyVal;
                            excelColStart++;
                        }
                        //workSheet.Cells[recordIndex, 3].Value = item.Property2;
                        recordIndex++;
                    }
                    workSheet.Column(3).AutoFit();
                    fileByteArray = package.GetAsByteArray();
                    statusModel.ErrorCode = fileName;
                }
            }
            catch (Exception e)
            {
                statusModel.IsSuccess = false;
                statusModel.ErrorCode = e.Message + " - Error in Exporting File!";
            }
            return Json(statusModel);
        }

        [HttpGet]
        public ActionResult Download(string fileName)
        {
            //Get the temp folder and file path in server
            //string rootFolder = _hostingEnvironment.WebRootPath;
            //string filePath = Path.Combine(rootFolder, "Downloads", fileName);
            //byte[] fileByteArray = System.IO.File.ReadAllBytes(filePath);
            //System.IO.File.Delete(filePath);
            var fileByteSafeCopy = fileByteArray;
            fileByteArray = null;
            return File(fileByteSafeCopy, "application/vnd.ms-excel", fileName);
        }
        //pdf writing image, text
        //[HttpGet]
        //public IActionResult GetPDFFileTest()
        //{
        //    string fileName = "Export_" + "Menu" + "_" + ToJulianDate() + ".pdf";
        //    //Create a new PDF document
        //    PdfDocument document = new PdfDocument();

        //    //Add a page to the document
        //    PdfPage page = document.Pages.Add();

        //    //Create PDF graphics for the page
        //    PdfGraphics graphics = page.Graphics;

        //    //Set the standard font
        //    PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

        //    MemoryStream imgStream = new MemoryStream();
        //    string path = _hostingEnvironment.WebRootPath + "/Themes/QMSN/Resources/img";//logo-icon.png
        //    //string path = "C:/Users/Umer/Desktop/temp";
        //    if (Directory.Exists(path))
        //    {
        //        var img = System.Drawing.Image.FromFile(path + "/logo-icon.png");
        //        img.Save(imgStream, System.Drawing.Imaging.ImageFormat.Png);

        //    }

        //    PdfBitmap image = new PdfBitmap(imgStream);
        //    //Draw the image
        //    graphics.DrawImage(image, 0, 0);

        //    //Draw the text
        //    graphics.DrawString("As-Salam-o-Alikum!", font, PdfBrushes.Black, new PointF(0, 0));

        //    MemoryStream stream = new MemoryStream();
        //    //Save and return the PDF file
        //    document.Save(stream);
        //    document.Close(true);
        //    return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "Sample.pdf");

        //}
        //pdf writing html
        [HttpGet]
        public IActionResult GetPDFFileTest()
        {
            //Initialize HTML to PDF converter
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);
            WebKitConverterSettings settings = new WebKitConverterSettings();
            //Set the page orientation 
            settings.Orientation = PdfPageOrientation.Portrait;
            //Set WebKit path
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            settings.WebKitPath = contentRootPath+ "/QtBinariesWindows/";
            //Assign WebKit settings to HTML converter
            htmlConverter.ConverterSettings = settings;
            //HTML string and base URL 
            string htmlText = "<html><body Align='Left'><br><p> <font size='12'>As-Salam-o-Alikum! </p></font> </body></html>";
            string baseUrl = string.Empty;
            //Convert a URL to PDF with HTML converter
            PdfDocument document = htmlConverter.Convert(htmlText, baseUrl);
            //Save and close the PDF document
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            document.Close(true);
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "HTMLtoPDF.pdf");
        }

        [HttpGet]
        public IActionResult DownloadPDF()
        {
            //Initialize HTML to PDF converter
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.WebKit);
            WebKitConverterSettings settings = new WebKitConverterSettings();
            //Set the page orientation 
            settings.Orientation = PdfPageOrientation.Portrait;
            //Set WebKit path
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            settings.WebKitPath = contentRootPath + "/QtBinariesWindows/";
            //Assign WebKit settings to HTML converter
            htmlConverter.ConverterSettings = settings;
            //HTML string and base URL 
            //string htmlText = "<html><body Align='Left'><br><p> <font size='12'>As-Salam-o-Alikum! </p></font> </body></html>";
            string baseUrl = string.Empty;
            //Convert a URL to PDF with HTML converter
            PdfDocument document = htmlConverter.Convert("http://usatbillingapp.westeurope.cloudapp.azure.com/SOInvoice/ViewInvoice?id=4102/");
            //Save and close the PDF document
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            document.Close(true);
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "HTMLtoPDF.pdf");
        }


    }
}
//20.46.43.179
