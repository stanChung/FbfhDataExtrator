using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace FbfhDataExtrator
{
    public class HttpWebClient : WebClient
    {
        // Cookie 容器
        private CookieContainer cookieContainer;

        public HttpWebClient()
        {
            this.cookieContainer = new CookieContainer();
        }

        public HttpWebClient(CookieContainer cc)
        {
            this.cookieContainer = cc;
        }

        /// <summary>
        /// Cookie 容器
        /// </summary>
        public CookieContainer MyCookies
        {
            get { return this.cookieContainer; }
            set { this.cookieContainer = value; }
        }

        /// <summary>
        /// 覆寫web request方法，讓webclient能保持session
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            //throw new Exception();
            WebRequest request;
            request = base.GetWebRequest(address);
            //判斷是不是HttpWebRequest.只有HttpWebRequest才有此属性
            if (request is HttpWebRequest)
            {
                HttpWebRequest httpRequest = request as HttpWebRequest;
                httpRequest.CookieContainer = this.cookieContainer;
            }
            return request;
        }
    }
}