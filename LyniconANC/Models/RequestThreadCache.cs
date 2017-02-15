﻿using Lynicon.Extensibility;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Lynicon.Models
{
    /// <summary>
    /// The request thread cache maintains a cache of data scoped to a request, or if not in a request, the current
    /// thread
    /// </summary>
    public class RequestThreadCache
    {
        static readonly RequestThreadCache instance = new RequestThreadCache();

        /// <summary>
        /// The cache currently active - a Dictionary of string x object
        /// </summary>
        public static IDictionary<object, object> Current
        {
            get
            {
                var reqCache = instance.GetRequestCache();
                if (reqCache != null)
                    return reqCache;
                else
                    return instance.GetThreadCache();
            }
        }

        ThreadLocal<Dictionary<object, object>> threadCache = new ThreadLocal<Dictionary<object, object>>(() => new Dictionary<object, object>());

        public RequestThreadCache()
        { }

        IDictionary<object, object> GetRequestCache()
        {
            IDictionary<object, object> items = null;
            try
            {
                items = (IDictionary<object, object>)RequestContextManager.Instance.CurrentContext.Items;
            }
            catch { }
            if (items == null)
                return null;
            else
                return items;
        }

        IDictionary<object, object> GetThreadCache()
        {
            return threadCache.Value;
        }

    }
}