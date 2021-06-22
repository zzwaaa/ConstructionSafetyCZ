using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Caching
{
    public class RedisCacheService:ICacheService
    {
        //protected IDatabase _cache;
        protected CSRedisClient _cache;

        //private ConnectionMultiplexer _connection;

        //private readonly string _instance;

        //public RedisCacheService(RedisCacheOptions options, int database = 0)
        //{
        //    _connection = ConnectionMultiplexer.Connect(options.Configuration);

        //    _cache = _connection.GetDatabase(database);
        //    _instance = options.InstanceName;
        //}

        public RedisCacheService(string conn)
        {
            //_connection = ConnectionMultiplexer.Connect(options.Configuration);

            _cache = new CSRedisClient(conn);
            //_cache = _connection.GetDatabase(database);
            //_instance = options.InstanceName;
        }

        /// <summary>
        /// 获取key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetKeyForRedis(string key)
        {
            return key;
        }

        #region 添加缓存

        #region 同步添加缓存
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.Set(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间,Redis中无效）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public bool Add<T>(string key, T value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            var second = expiresSliding.TotalSeconds;
            return _cache.Set(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), (int)second);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间,Redis中无效）</param>
        /// <returns></returns>
        public bool Add<T>(string key, T value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            var second = expiresIn.TotalSeconds;
            return _cache.Set(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), (int)second);
        }
        #endregion

        #region 异步添加缓存
        /// <summary>
        /// 异步添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await Task.Run(() =>
            {
                return _cache.SetAsync(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
            });

        }
        /// <summary>
        /// 异步添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresSliding"></param>
        /// <param name="expiressAbsoulte"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync<T>(string key, T value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await Task.Run(() =>
            {
                var second = expiresSliding.TotalSeconds;
                return _cache.Set(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), (int)second);
            });

        }
        /// <summary>
        /// 异步添加缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresIn"></param>
        /// <param name="isSliding"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync<T>(string key, T value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await Task.Run(() =>
            {
                var second = expiresIn.TotalSeconds;
                return _cache.Set(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), (int)second);
            });

        }
        #endregion
        #endregion

        #region 验证缓存是否存在

        #region 同步验证缓存是否存在
        /// <summary>
        /// 验证缓存是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.Exists(GetKeyForRedis(key));
        }
        #endregion

        #region 异步验证缓存是否存在
        /// <summary>
        /// 异步判断缓存是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await Task.Run(() =>
            {
                return _cache.Exists(GetKeyForRedis(key));
            });

        }
        #endregion
        #endregion

        #region 获取缓存

        #region 同步获取缓存
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (!Exists(GetKeyForRedis(key)))
            {
                return default(T);
            }
            var value = _cache.Get(GetKeyForRedis(key));

            //if (!value.HasValue)
            //{
            //    return default(T);
            //}

            return JsonConvert.DeserializeObject<T>(value);
        }
        /// <summary>
        /// 获得缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (!Exists(GetKeyForRedis(key)))
            {
                return null;
            }
            var value = _cache.Get(GetKeyForRedis(key));

            //if (!value.HasValue)
            //{
            //    return null;
            //}
            return JsonConvert.DeserializeObject(value);
        }
        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }
            var dict = new Dictionary<string, T>();

            keys.ToList().ForEach(item => dict.Add(item, Get<T>(GetKeyForRedis(item))));

            return dict;
        }
        #endregion

        #region 异步获取缓存
        /// <summary>
        /// 异步获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }
            return await Task.Run(() =>
            {
                var dict = new Dictionary<string, T>();

                keys.ToList().ForEach(item => dict.Add(item, Get<T>(GetKeyForRedis(item))));

                return dict;
            });
        }
        /// <summary>
        /// 异步获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await Task.Run(() =>
            {
                if (!Exists(GetKeyForRedis(key)))
                {
                    return default(T);
                }
                var value = _cache.Get(GetKeyForRedis(key));

                //if (!value.HasValue)
                //{
                //    return default(T);
                //}

                return JsonConvert.DeserializeObject<T>(value);
            });

        }
        /// <summary>
        /// 异步获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<object> GetAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return await Task.Run(() =>
            {
                if (!Exists(GetKeyForRedis(key)))
                {
                    return null;
                }

                var value = _cache.Get(GetKeyForRedis(key));

                //if (!value.HasValue)
                //{
                //    return null;
                //}
                return JsonConvert.DeserializeObject(value);
            });

        }
        #endregion
        #endregion

        #region 删除缓存

        #region 同步删除缓存
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            var temp = _cache.Del(GetKeyForRedis(key));
            return temp > 0;
        }
        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="keys"></param>
        public void RemoveAll(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            keys.ToList().ForEach(item => Remove(GetKeyForRedis(item)));
        }
        #endregion

        #region 异步删除缓存
        /// <summary>
        /// 异步移除缓存
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task RemoveAllAsync(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }
            await Task.Run(() =>
            {
                keys.ToList().ForEach(item => Remove(GetKeyForRedis(item)));
            });
        }
        /// <summary>
        /// 异步移除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await Task.Run(() =>
            {
                return _cache.Del(GetKeyForRedis(key)) > 0;
            });

        }
        #endregion
        #endregion

        #region 修改缓存

        #region 同步修改缓存
        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Replace<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (Exists(key))
                if (!Remove(key))
                    return false;

            return Add(key, value);
        }

        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间，Redis无效）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public bool Replace<T>(string key, T value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (Exists(key))
                if (!Remove(key))
                    return false;

            return Add(key, value, expiresSliding, expiressAbsoulte);
        }

        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public bool Replace<T>(string key, T value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (Exists(key))
                if (!Remove(key)) return false;

            return Add(key, value, expiresIn, isSliding);
        }
        #endregion

        #region 异步修改缓存
        /// <summary>
        /// 异步修改缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> ReplaceAsync<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await Task.Run(() =>
            {
                if (Exists(key))
                    if (!Remove(key))
                        return false;

                return Add(key, value);
            });

        }
        /// <summary>
        /// 异步修改缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresSliding"></param>
        /// <param name="expiressAbsoulte"></param>
        /// <returns></returns>
        public async Task<bool> ReplaceAsync<T>(string key, T value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await Task.Run(() =>
            {
                if (Exists(key))
                    if (!Remove(key))
                        return false;

                return Add(key, value, expiresSliding, expiressAbsoulte);
            });

        }
        /// <summary>
        /// 异步修改缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresIn"></param>
        /// <param name="isSliding"></param>
        /// <returns></returns>
        public async Task<bool> ReplaceAsync<T>(string key, T value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await Task.Run(() =>
            {
                if (Exists(key))
                    if (!Remove(key)) return false;

                return Add(key, value, expiresIn, isSliding);
            });

        }
        #endregion
        #endregion

        public void Dispose()
        {
            //if (_connection != null)
            //    _connection.Dispose();
            if (_cache != null)
            {
                _cache.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
