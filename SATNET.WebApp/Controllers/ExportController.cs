using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SATNET.Service;
using SATNET.WebApp.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.Reflection;

namespace SATNET.WebApp.Controllers
{
    public class ExportController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
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
            catch (Exception e) {
                statusModel.IsSuccess = false;
                statusModel.ErrorCode = "Error in Exporting File!";
            }
            
            return Json(statusModel);
            //return File(stream, "application/pdf", "Sample.pdf");
        }
        public IActionResult ExcelExport(IEnumerable<ExportModel> records, string header, string menu)
        {
            var statusModel = new StatusModel { IsSuccess = true };
            string rootFolder = _hostingEnvironment.WebRootPath;
            string fileName = "Export_"  + menu + "_" + ToJulianDate()  + ".xlsx";
            try {
                List<string> tableHeaders = new List<string>() { "Sr. No" };
                tableHeaders.AddRange(header.Split(',').ToList());
                FileInfo file = new FileInfo(Path.Combine(rootFolder, "Downloads", fileName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(rootFolder, fileName));
                }
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets.Add("Sheet Name");
                    workSheet.TabColor = System.Drawing.Color.Black;
                    workSheet.DefaultRowHeight = 12;
                    // Header of the Excel sheet
                    int thLength = tableHeaders.Count;
                    for (int i = 1; i <= thLength; i++)
                    {
                        workSheet.Cells[1, i].Value = tableHeaders.ElementAt(i - 1);
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

                    package.Save();
                    statusModel.ErrorCode = "File '" + fileName + "' Exported Succesfully";

                }
            }
            catch(Exception e)
            {
                statusModel.IsSuccess = false;
                statusModel.ErrorCode = "Error in Exporting File!";
            }
            return Json(statusModel);
        }
    }
}
