﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ItemConsole
{
    public class UpdateItemRequest
    {
        public static void UpdateFieldOnHomeItem(string fieldName, string fieldValue)
        {
            // Setup Item request
            var request = 
                (HttpWebRequest)WebRequest.Create("http://sitecoreukug/-/item/v1/sitecore/content/home");

            // Username and password
            request.Headers.Add("X-Scitemwebapi-Username", "admin");
            request.Headers.Add("X-Scitemwebapi-Password", "b");
            request.Method = "PUT";

            var data = Encoding.UTF8.GetBytes(string.Format("{0}={1}", HttpUtility.UrlEncode(fieldName), HttpUtility.UrlEncode(fieldValue)));
            request.ContentLength = data.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            var requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            // Check status is OK
            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                dynamic json = JObject.Parse(result);
                if (json.status != "OK") throw new Exception(json.ToString());
            }
        }
    }
}
