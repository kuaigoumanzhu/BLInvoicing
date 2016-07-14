using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace BL.Framework.Cache
{
    /// <summary>
    /// hpf 2016 07 10
    /// </summary>
    public class RuntimeMemoryCache:ICache
    {
        private static Func<KeyValuePair<string, object>, string> func;
        private readonly MemoryCache memoryCache = new MemoryCache("BL");

        /// <summary>
        /// hpf加入缓存项(绝对过期时间)
        /// </summary>
        /// <param name="key">缓存项标识</param>
        /// <param name="value">缓存项</param>
        /// <param name="timeSpan">缓存失效时间</param>
        public void Add(string key, object value, TimeSpan timeSpan)
        {
            if (!string.IsNullOrEmpty(key) && (value != null))
            {
                CacheItemPolicy cip = new CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.Now.Add(timeSpan)
                };
                this.memoryCache.Add(key, value, cip, null);
            }
        }
        /// <summary>
        /// hpf加入依赖物理文件的缓存项
        /// </summary>
        /// <param name="key">缓存项标识</param>
        /// <param name="value">缓存项</param>
        /// <param name="fullFileNameOfFileDependency">依赖的文件全路径</param>
        public void AddWithFileDependency(string key, object value, string fullFileNameOfFileDependency)
        {
            if (!string.IsNullOrEmpty(key) && (value != null))
            {

                CacheItemPolicy policy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMonths(1)
                };
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { fullFileNameOfFileDependency }));
                this.memoryCache.Add(key, value, policy, null);
            }
        }

        public void Clear()
        {
            if (func == null)
            {
                func = new Func<KeyValuePair<string, object>, string>(RuntimeMemoryCache.GetKey);
            }
            foreach (string str in this.memoryCache.AsParallel<KeyValuePair<string, object>>().ToDictionary<KeyValuePair<string, object>, string>(func).Keys)
            {
                this.memoryCache.Remove(str, null);
            }
        }

        public object Get(string cacheKey)
        {
            return this.memoryCache[cacheKey];
        }
        /// <summary>
        /// 标识删除
        /// </summary>
        /// <param name="key">缓存项标识</param>
        /// <param name="value">缓存项</param>
        /// <param name="timeSpan">缓存失效时间间隔</param>
        public void MarkDeletion(string key, object value, TimeSpan timeSpan)
        {
            this.Remove(key);
        }

        public void Remove(string cacheKey)
        {
            this.memoryCache.Remove(cacheKey, null);
        }
        /// <summary>
        /// 如果不存在缓存项则添加，否则更新(相对过期)
        /// </summary>
        /// <param name="key">缓存项标识</param>
        /// <param name="value">缓存项</param>
        /// <param name="timeSpan">缓存失效时间</param>
        public void Set(string key, object value, TimeSpan timeSpan)
        {
            CacheItemPolicy cip = new CacheItemPolicy()
            {
                SlidingExpiration = timeSpan,
            };
            this.memoryCache.Set(key, value, cip, null);
        }
        /// <summary>
        /// 设置绝对过期时间
        /// </summary>
        /// <param name="key">缓存项标识</param>
        /// <param name="value">缓存项</param>
        /// <param name="timeSpan">缓存失效时间</param>
        public void SetAbsoluteExpiration(string key, object value, TimeSpan timeSpan)
        {
            CacheItemPolicy cip = new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTime.Now.Add(timeSpan),
            };
            this.memoryCache.Set(key, value, cip, null);
        }
        private static string GetKey(KeyValuePair<string, object> keyValuePair)
        {
            return keyValuePair.Key;
        }


        public T Get<T>(string cacheKey) where T : class
        {
            object obj = this.Get(cacheKey);
            if (obj != null)
            {
                return (obj as T);
            }
            return default(T);
        }
    }
}
