using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CsChat.Core
{
    /// <summary>
    /// 会话帮助类
    /// </summary>
    public class ConcurrentDictionaryHelper<T> where T : class
    {
        /// <summary>
        /// 会话
        /// </summary>
        private static ConcurrentDictionary<string, T> dic = new ConcurrentDictionary<string, T>();

        /// <summary>
        /// 新增会话
        /// </summary>
        /// <param name="value"></param>
        public static void Add(string key, T value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                dic.TryAdd(key, value);
            }
        }

        /// <summary>
        /// 关闭会话
        /// </summary>
        /// <param name="value"></param>
        public static void Remove(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                T value = null;
                dic.TryRemove(key, out value);
            }
        }

        /// <summary>
        /// 根据id获取会话
        /// </summary>
        /// <param name="valueID"></param>
        /// <returns></returns>
        public static T Get(string key)
        {
            T value = null;
            if (dic.TryGetValue(key, out value))
            {
                return value;
            }
            return null;
        }

        public static void Using(Action<ConcurrentDictionary<string, T>> action)
        {
            action(dic);
        }

    }
}
