using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCUtil.DBS
{
    public class DapperHelper<T>
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private readonly string connectionString;
        public DapperHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }
        //ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="sql">查询的sql</param>
        /// <param name="param">替换参数</param>
        /// <returns></returns>
        public List<T> Query(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.Query<T>(sql, param).ToList();
            }
        }
        /// <summary>
        /// 查询列表异步方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return await con.QueryAsync<T>(sql, param);
            }
        }

        /// <summary>
        /// 查询第一个数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QueryFirst(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.QueryFirst<T>(sql, param);
            }
        }
        /// <summary>
        /// 查询第一个数据异步方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<T> QueryFirstAsync(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return await con.QueryFirstAsync<T>(sql, param);
            }
        }

        /// <summary>
        /// 查询第一个数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QueryFirstOrDefault(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.QueryFirstOrDefault<T>(sql, param);
            }
        }

        /// <summary>
        /// 异步查询第一个数据，没有则返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<T> QueryFirstOrDefaultAsync(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return await con.QueryFirstOrDefaultAsync<T>(sql, param);
            }
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QuerySingle(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.QuerySingle<T>(sql, param);
            }
        }
        /// <summary>
        /// 异步查询单条数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<T> QuerySingleAsync(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return await con.QuerySingleAsync<T>(sql, param);
            }
        }

        /// <summary>
        /// 查询单条数据没有返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T QuerySingleOrDefault(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.QuerySingleOrDefault<T>(sql, param);
            }
        }
        /// <summary>
        /// 异步查询单条数据，没有则返回默认值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<T> QuerySingleOrDefaultAsync(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return await con.QuerySingleOrDefaultAsync<T>(sql, param);
            }
        }



        /// <summary>
        /// 增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Execute(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.Execute(sql, param);
            }
        }

        /// <summary>
        /// 异步增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return await con.ExecuteAsync(sql, param);
            }
        }

        /// <summary>
        /// Reader获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.ExecuteReader(sql, param);
            }
        }
        /// <summary>
        /// 异步Reader获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IDataReader> ExecuteReaderAsync(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return await con.ExecuteReaderAsync(sql, param);
            }
        }

        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.ExecuteScalar(sql, param);
            }
        }

        /// <summary>
        /// 异步Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object> ExecuteScalarAsync(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return await con.ExecuteScalarAsync(sql, param);
            }
        }

        /// <summary>
        /// Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T ExecuteScalarForT(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return con.ExecuteScalar<T>(sql, param);
            }
        }
        /// <summary>
        /// 异步Scalar获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<T> ExecuteScalarForTAsync(string sql, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return await con.ExecuteScalarAsync<T>(sql, param);
            }
        }

        /// <summary>
        /// 带参数的存储过程
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<T> ExecutePro(string proc, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                List<T> list = con.Query<T>(proc,
                    param,
                    null,
                    true,
                    null,
                    CommandType.StoredProcedure).ToList();
                return list;
            }
        }
        /// <summary>
        /// 异步执行带参数的存储过程
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> ExecuteProAsync(string proc, object param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                return await con.QueryAsync<T>(proc,
                    param,
                    null,
                    null,
                    CommandType.StoredProcedure);
            }
        }


        /// <summary>
        /// 事务1 - 全SQL
        /// </summary>
        /// <param name="sqlarr">多条SQL</param>
        /// <param name="param">param</param>
        /// <returns></returns>
        public int ExecuteTransaction(string[] sqlarr)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        int result = 0;
                        foreach (var sql in sqlarr)
                        {
                            result += con.Execute(sql, null, transaction);
                        }

                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
        /// <summary>
        /// 异步执行事务1-纯sql，无参数
        /// </summary>
        /// <param name="sqlarr"></param>
        /// <returns></returns>
        public async Task<int> ExecuteTransactionAsync(string[] sqlarr)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        int result = 0;
                        foreach (var sql in sqlarr)
                        {
                            result += await con.ExecuteAsync(sql, null, transaction);
                        }
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// 事务2 - 声明参数
        ///demo:
        ///dic.Add("Insert into Users values (@UserName, @Email, @Address)",
        ///        new { UserName = "jack", Email = "380234234@qq.com", Address = "上海" });
        /// </summary>
        /// <param name="Key">多条SQL</param>
        /// <param name="Value">param</param>
        /// <returns></returns>
        public int ExecuteTransaction(Dictionary<string, object> dic)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        int result = 0;
                        foreach (var sql in dic)
                        {
                            result += con.Execute(sql.Key, sql.Value, transaction);
                        }

                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
        /// <summary>
        /// 异步执行事务2 -带参数sql
        /// demo:
        /// dic.Add("Insert into Users values (@UserName, @Email, @Address)",
        ///        new { UserName = "jack", Email = "380234234@qq.com", Address = "上海" });
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public async Task<int> ExecuteTransactionAsync(Dictionary<string, object> dic)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        int result = 0;
                        foreach (var sql in dic)
                        {
                            result += await con.ExecuteAsync(sql.Key, sql.Value, transaction);
                        }

                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }

        public int ExecuteTransaction(List<Dictionary<string, object>> sqlDic)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        int result = 0;
                        foreach (var dic in sqlDic)
                        {
                            foreach (var sql in dic)
                            {
                                result += con.Execute(sql.Key, sql.Value, transaction);
                            }
                        }


                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}
