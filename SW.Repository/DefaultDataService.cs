﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Repository
{
    using System.IO;
    using System.Net;

    /// <summary>
    /// This is the default service for consuming data from web.
    /// </summary>
    /// <seealso cref="SW.Repository.IDataService" />
    public class DefaultDataService : IDataService
    {
        /// <summary>
        /// The web helper used for retrieve request and response
        /// </summary>
        private IWebHelper webHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDataService"/> class.
        /// </summary>
        /// <param name="webHelper">The web helper.</param>
        public DefaultDataService(IWebHelper webHelper)
        {
            this.webHelper = webHelper;
        }

        /// <summary>
        /// Gets the result from response helper method.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>System.String or null if there are error while processing the request.</returns>
        public string GetDataResult(string url)
        {
            WebRequest request = this.webHelper.GetRequest(url);
            WebResponse response = null;

            try
            {
                response = this.webHelper.GetResponse(request);
                string json = string.Empty;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    json = reader.ReadToEnd();
                }

                return json;
            }
            catch (WebException ex)
            {
                //// TODO: Check status when there are no Internet connection. 
                return null;
            }
        }
    }
}
