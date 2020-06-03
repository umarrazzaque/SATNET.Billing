using Microsoft.AspNetCore.Http;
using SATNET.WebApp.Models;
using System;
using System.Collections.Generic;

namespace SATNET.WebApp.Helpers
{
    public static class SessionObjects
    {
        private static ThemeProperties _theme;

        static SessionObjects()
        {
            PasswordLength = 5;
			LoginedUserId = 0;
			LoginedUserName = null;
	        MilestoneId = 0;
	        ParentTaskId = 0;

        }

        public static Int32 MilestoneId
		{
            get; set;
        }

		public static Int32 ParentTaskId
		{
            get; set;
        }

        public static Int32 LoginedUserId
        {
            get; set;
        }

        public static String LoginedUserName
        {
            get; set;
        }

        
        /// <summary>
        /// WpId Foreign key from Workpress
        /// Shakeel Zafar 
        /// </summary>
        public static Int32 LoginedUserWpId
        {
            get; set;
        }
        public static Int32 ModuleId
        {
            get; set;
        }
        public static String ModuleHeading
        {
            get; set;
        }
        public static String ModuleSubHeading
        {
            get; set;
        }

        public static String ModuleUrl
        {
            get; set;
        }

        public static String EncMenuId
        {
            get; set;
        }
        //public static List<MenuModel> MenuLinksList
        //{
        //    get; set;
        //}

 
        public static String MenuHeading
        {
            get; set;
        }
        public static String MenuSubHeading
        {
            get; set;
        }
        public static String MenuBriefDescription
        {
            get; set;
        }
        public static List<MenuModel> BredCrums
        {
            get; set;
        }


        //public static HttpRequestBase SearchRequest
        //{
        //    get; set;
        //}  
        public static void InitSession(UserViewModel model)
        {
            //HttpContext.Current.Session.Clear();
            //LoginedUserId = model.UserId;
            //LoginedUserName = model.UserName;
            //LoginedUserWpId = model.WpId;
        }

        public static void EndSession()
        {
            //HttpContext.Current.Session.Clear();
        }


        public static Int32 PasswordLength { get; set; }


        public static Int32 RecordsPerPage
        {
            get; set;
            //get
            //{
            //    var recordsPerPage = System.Web.HttpContext.Current.Session["RecordsPerPage"];
            //    if (recordsPerPage == null || String.IsNullOrEmpty(recordsPerPage.ToString()))
            //        System.Web.HttpContext.Current.Session["RecordsPerPage"] = ConfigurationManager.AppSettings["RecordsPerPage"];

            //    return Utilities.TryInt32Parse(System.Web.HttpContext.Current.Session["RecordsPerPage"]);
            //}
            //set { System.Web.HttpContext.Current.Session["RecordsPerPage"] = value; }
        }

        public static string SiteUrl
        {
            get; set;
        }

        public static ThemeProperties Theme
        {
            get { return _theme ?? (_theme = new ThemeProperties()); }
        }

        public static void Dispose()
        {
            //close session
        }

    }

    public class ThemeProperties
    {
        public String Name
        {
            get; set;
        }

        public String Color
        {
            get; set;
        }

        public String ThemePath
        {
            get { return  "~/Themes/" + Name; }
        }

        public String ThemeResourcePath
        {
            get { return string.Format("{0}Themes/{1}/Resource", SessionObjects.SiteUrl, Name); }
        }

        public String ThemeImagesPath
        {
            get { return ThemeResourcePath + "/img"; }
        }
        public String ThemeCssPath
        {
            get { return ThemeResourcePath + "/css"; }
        }
        public String ThemePluginsPath
        {
            get { return ThemeResourcePath + "/plugins"; }
        }
        public String ThemeJsPath
        {
            get { return ThemeResourcePath + "/js"; }
        }
    }
}