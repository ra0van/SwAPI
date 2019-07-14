using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Repository
{
    /// <summary>
    /// A Service for consuming data from remote source. 
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Gets the data result as JSON. The syntax and format of the result should be valid. 
        /// </summary>
        /// <param name="url">The URL for consuming.</param>
        /// <returns>JSON data as string.</returns>
        string GetDataResult(string url);
    }
}
