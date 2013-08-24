// #region License
// // 
// // Author: Tomas Fabian <fabian.frameworks@gmail.com>
// // Copyright (c) 2013, Tomas Fabian
// // 
// // Licensed under the Apache License, Version 2.0.
// // See the file LICENSE.txt for details.
// // 

using System;
using System.Collections.Generic;
using System.Linq;

namespace PushPro.Client.Extensions
{
    public static class QueryExtensions
    {
        #region ToQueryDictionary
        
        /// <summary>
        /// Gets the query key-value pairs.
        /// </summary>
        /// <param name="uri">Uri with the query</param>
        /// <returns>Collection of query values</returns>
        public static IDictionary<string, string> ToQueryDictionary(this Uri uri)
        {
            return uri.Query.
                       Trim('?')
                      .Split('&')
                      .Where(x => !string.IsNullOrWhiteSpace(x))
                      .Select(x => x.Split('='))
                      .ToDictionary(x => x[0], x => x[1]);
        }

        #endregion

        #region GetComponents

        /// <summary>
        /// Gets the uri scheme, path, host and port components.
        /// </summary>
        /// <param name="uri">Uri for component extraction</param>
        /// <returns>The extracted uri components</returns>
        public static string GetMainComponents(this Uri uri)
        {
            return uri.GetComponents(UriComponents.Scheme | UriComponents.HostAndPort | UriComponents.Path,
                                     UriFormat.SafeUnescaped);
        }

        #endregion

    }
}