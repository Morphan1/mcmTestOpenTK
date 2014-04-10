using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers
{
    public class WebHandler
    {
        /// <summary>
        /// Gets a webpage object for the given input headers.
        /// </summary>
        /// <param name="input">The input headers</param>
        /// <returns>A valid webpage object</returns>
        public static WebHandler GetWebpage(string input)
        {
            WebHandler handler = new WebHandler();
            handler.InterpretRequest(input.Replace("\r", "").Split('\n'));
            return handler;
        }

        void InterpretRequest(string[] input)
        {
            if (input.Length < 3)
            {
                // Not an HTTP header
                return;
            }
            string[] Request = input[0].Split(' ');
            if (Request.Length != 3)
            {
                // Not an HTTP request
                return;
            }
            BaseRequest = Request[0] + " " + Request[1];
            for (int i = 1; i < Request.Length - 1; i++)
            {
                string[] split = Request[i].Split(new char[] { ':' }, 2);
                if (split.Length != 2)
                {
                    continue;
                }
                string Setting = split[0].Trim().ToLower();
                string Value = split[1].Trim().ToLower();
                if (Setting == "accept-encoding")
                {
                    GZip = Value.Contains("gzip");
                }
            }
            if (Request[0] == "GET" || Request[0] == "HEAD")
            {
                ReadPage(Request[1].ToLower());
            }
            else if (Request[0] == "POST")
            {
                // TODO
                Page = btos("Cannot post currently...");
            }
        }

        void ReadPage(string page)
        {
            if (page.Length == 0)
            {
                return;
            }
            if (page.EndsWith(".html") || page.EndsWith(".php") && page.LastIndexOf('.') != 0)
            {
                Page = btos("Redirecting...");
                AdditionalHeaders = "\r\nLocation: " + page.Substring(0, page.LastIndexOf('.'));
                Status = 302;
                return;
            }
            if (page == "/" || page == "/index")
            {
                // TODO
                Page = btos("TODO");
                Status = 200;
                return;
            }
            else
            {
                // TODO
            }
            Status = 404;
            Page = btos("TODO: 404 page!");
        }

        byte[] btos(string input)
        {
            return FileHandler.encoding.GetBytes(input);
        }

        /// <summary>
        /// The request sent to get this webpage.
        /// </summary>
        public string BaseRequest;

        /// <summary>
        /// The status code for this web page.
        /// </summary>
        public int Status = 400;

        string AdditionalHeaders = "";

        bool GZip = false;

        /// <summary>
        /// All binary data on this page.
        /// </summary>
        public byte[] Page = new byte[0];

        /// <summary>
        /// What type of page this is.
        /// </summary>
        public string ContentType = "text/html; charset=utf-8";

        /// <summary>
        /// Simplies all the webpage data down to transmittable bytes.
        /// </summary>
        /// <returns>A transmittable byte array</returns>
        public byte[] ToBytes()
        {
            byte[] fpage = Page;
            if (GZip)
            {
                fpage = FileHandler.GZip(Page);
                AdditionalHeaders += "\r\nContent-Encoding: gzip";
            }
            byte[] Header = btos("HTTP/1.1 " + StatusString()
                + "\r\nContent-Length: " + fpage.Length
                + "\r\nContent-Type: " + ContentType
                + "\r\nConnection: close"
                + AdditionalHeaders
                + "\r\n\r\n");
            byte[] toret = new byte[Header.Length + fpage.Length];
            Header.CopyTo(toret, 0);
            fpage.CopyTo(toret, Header.Length);
            return toret;
        }

        string StatusString()
        {
            switch (Status)
            {
                case 200:
                    return "200 OK";
                case 400:
                    return "400 Interal Server Error";
                case 404:
                    return "404 Not Found";
                case 302:
                    return "302 Moved Temporarily";
                default:
                    return Status + " Unknown Code";
            }
        }
    }
}
