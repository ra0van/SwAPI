﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Repository
{
    using System.Net;

    /// <summary>
    /// Helper class implements <see cref="SW.Repository.IWebHelper" />. Retrieves <see cref="System.Net.WebRequest" /> and <see cref="System.Net.WebResponse" />.
    /// </summary>
    /// <seealso cref="SW.Repository.IWebHelper" />
    public class WebHelper : IWebHelper
    {
        /// <summary>
        /// Gets instance of <see cref="System.Net.WebRequest" />.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        /// Initialized <see cref="System.Net.WebRequest" />.
        /// </returns>
        public virtual WebRequest GetRequest(string url)
        {
            return WebRequest.Create(url);
        }

        /// <summary>
        /// Gets the instance of <see cref="System.Net.WebResponse" /> from request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Response from request.
        /// </returns>
        public virtual WebResponse GetResponse(WebRequest request)
        {
            return request.GetResponse();
        }
    }
}
