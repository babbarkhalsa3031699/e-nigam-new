using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Filters
{
    public class NoCacheAttribute : ActionFilterAttribute
    {
        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
        //    filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
        //    filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
        //    filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    filterContext.HttpContext.Response.Cache.SetNoStore();

        //    base.OnResultExecuting(filterContext);
        //}
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext == null) throw new ArgumentNullException("filterContext");

            var cache = GetCache(filterContext);

            cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            cache.SetValidUntilExpires(false);
            cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }

        /// <summary>
        /// Get the reponse cache
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        protected virtual HttpCachePolicyBase GetCache(ResultExecutingContext filterContext)
        {
            return filterContext.HttpContext.Response.Cache;
        }
    }
}