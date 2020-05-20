using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SATNET.WebApp.Helpers
{
    public static class Utilities
    {
        public static Int32 TryInt32Parse(Object obj)
        {
            if (obj == null) return 0;
            Int32 retval = 0;
            Int32.TryParse(obj.ToString(), out retval);
            return retval;
        }

		public static List<SelectListItem> GetABCItems(char to)
		{
			var items = new List<SelectListItem>();
			for (char i = 'A'; i <= to; i++)
			{
				items.Add(
				new SelectListItem
				{
					Text = i.ToString(),
					Value = i.ToString()
				});
			}
			return items;
		}

		public static List<SelectListItem> GetNumberItems(int to)
		{
			var items = new List<SelectListItem>();
			for (int i = 1; i <= to; i++)
			{
				var oneZero = "0";
				var twoZero = "00";

				var value = "";
				if (i < 10)
				{
					value = twoZero + i;
				}
				if (i > 10 && i < 99)
				{
					value = oneZero + i;
				}
				items.Add(
				new SelectListItem
				{
					Text = value,
					Value = value
				});
			}
			return items;
		}

	    public static DateTime DefaultDate()
        {
            return new DateTime(0001,01,01).Date;
        }
        public static DateTime CurrentDate()
        {
            return DateTime.Now.Date;
        }
        public static void DeleteDirectoryData(string path)
        {
            var di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        public static Boolean TryBooleanParse(Object obj)
        {
            if (obj == null) return false;

            Boolean retval;
            Boolean.TryParse(obj.ToString(), out retval);
            return retval;
        }

        public static String TryStringParse(Object obj)
        {
            return obj == null ? "" : obj.ToString();
        }

	    public static string DisplayText(this string str, int charAllowed)
	    {
		    if (str.Length > charAllowed)
		    {
			    return str.Substring(0, charAllowed) + "...";
		    }
		    return str;
	    }

	    public static string GetImageFromByte(byte[] image)
	    {
			string imageBase64Data = Convert.ToBase64String(image);
			string imageData = string.Format("data:image/png;base64,{0}", imageBase64Data);
		    return imageData;
	    }

        public static string GetColour(string type)
        {
            string colour = "";
            switch (type)
            {
                case   "LOW":
                    colour = "bg-green";
                    break;
                case "NORMAL":
                    colour = "bg-blue";
                    break;
                case "HIGH":
                    colour = "bg-red";
                    break;
            }
            return colour;
        }

        public static DateTime GetDateTime(DateTime date)
        {
           var date1=  date.ToShortDateString();
           return Convert.ToDateTime(date1);
        }
        /// <summary>
        /// Compare time in Days, Hours, Mins, Secs with current Date
        /// </summary>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        public static String GetTimeSpan(DateTime fromDate)
        {
            TimeSpan diff = DateTime.Now - fromDate;
            
            var result = "";
            if (diff.Days != 0 && diff.Hours != 0)
            {
                result += string.Format("{0}d {1}h ", diff.Days, diff.Hours);
            }
            else if (diff.Hours != 0 && diff.Minutes != 0)
            {
                result += string.Format("{0}h {1}m ", diff.Hours, diff.Minutes);
            }
            else if (diff.Minutes != 0 && diff.Seconds != 0)
            {
                result += string.Format("{0}m {1}s ", arg0: diff.Minutes, arg1: diff.Seconds);
            }
            else
            {
                result += diff.Seconds + "s ";
            }
            return result;
        }

        /// <summary>
        /// get month name from date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static String GetReadableTime(DateTime date)
        {
            var result = date.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(date.Month)
                            + ", " + date.Year;
            return result;
        }
    }
}