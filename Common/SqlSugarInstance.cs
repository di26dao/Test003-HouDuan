using SqlSugar;
using Test003.Models;
using Test003.Models.Shared.Consts;

namespace Test003.Common
{
    public sealed class SqlSugarInstance
    {
        #region 构造：无参构造函数，禁止实例化 + public SqlSugarInstance()

        /// <summary>
        /// 禁止实例化
        /// </summary>
        private SqlSugarInstance()
        {
        }

        #endregion 构造：无参构造函数，禁止实例化 + public SqlSugarInstance()

        #region 静态：获取Sugar客户端实例对象 + public static SqlSugarClient GetInstance()

        /// <summary>
        /// 获取Sugar客户端实例对象
        /// </summary>
        /// <returns>SqlSugarClient</returns>
        public static SqlSugarClient GetInstance()
        {
            // 默认DB或对应DB连接串为空
            if (string.IsNullOrEmpty(Appsettings.EnableDb) || string.IsNullOrEmpty(Appsettings.ConnectionStrings))
            {
                return null;
            }

            #region 数据库类型

            DbType type;
            if (string.Compare(Appsettings.EnableDb, DbTypes.SQLServer, StringComparison.OrdinalIgnoreCase) == 0)
            {
                type = DbType.SqlServer;
            }
            else if (string.Compare(Appsettings.EnableDb, DbTypes.MySQL, StringComparison.OrdinalIgnoreCase) == 0)
            {
                type = DbType.MySql;
            }
            else if (string.Compare(Appsettings.EnableDb, DbTypes.PostgreSQL, StringComparison.OrdinalIgnoreCase) == 0)
            {
                type = DbType.PostgreSQL;
            }
            else if (string.Compare(Appsettings.EnableDb, DbTypes.Oracle, StringComparison.OrdinalIgnoreCase) == 0)
            {
                type = DbType.Oracle;
            }
            // 默认数据库类型
            else
            {
                type = DbType.SqlServer;
            }

            #endregion 数据库类型

            var dbClient = new SqlSugarClient(new ConnectionConfig()
            {
                // 连接字符串
                ConnectionString = Appsettings.ConnectionStrings,
                // 数据库类型
                DbType = type,
                // 自动释放和关闭数据库连接，如果有事务事务结束时关闭，否则每次操作后关闭
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                MoreSettings = new ConnMoreSettings()
                {
                    // SqlServer特殊配置：和他库不同一般选用Nvarchar，可以使用这个配置让他和其他数据库区分（其他库是varchar）
                    SqlServerCodeFirstNvarchar = true,
                    IsAutoDeleteQueryFilter = true,
                },
                // 配置数据库
                ConfigureExternalServices = new ConfigureExternalServices
                {
                    EntityService = (property, info) =>
                    {
                        // SQLServer 数据类型处理
                        if (string.Compare(Appsettings.EnableDb, DbTypes.SQLServer, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            // string类型处理
                            if (property.PropertyType == typeof(string) && info.Length == 0)
                            {
                                info.DataType = "NVARCHAR(MAX)";
                            }
                        }
                        // PostgreSQL 数据类型处理
                        else if (string.Compare(Appsettings.EnableDb, DbTypes.PostgreSQL, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                        }
                        // MySQL 数据类型处理
                        else if (string.Compare(Appsettings.EnableDb, DbTypes.MySQL, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                        }
                        // Oracle 数据类型处理
                        else if (string.Compare(Appsettings.EnableDb, DbTypes.Oracle, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                        }
                        // ** 其他数据库类型 待拓展

                        // 自动Nullable处理 类型?&&Nullable<类型>
                        if (info.IsPrimarykey == false && new NullabilityInfoContext().Create(property).WriteState is NullabilityState.Nullable)
                        {
                            info.IsNullable = true;
                        }
                    }
                },
                // AOP
                AopEvents = new AopEvents
                {
                    DataExecuting = (obj, model) =>
                    {
                        // ***前提是先查询再更新，对于 new {} 无效
                        if (model.PropertyName == "UpdateTime" && model.OperationType == DataFilterType.UpdateByObject)
                        {
                            // 设置 UpdateTime 值为当前时间
                            model.SetValue(DateTime.Now);
                        }
                    },
                    OnLogExecuting = (obj, model) =>
                    {
                        Microsoft.IdentityModel.Logging.LogHelper.LogInformation(obj.ToString());
                    },
                    //打印错误LOG
                    OnError = (sqlSugarException) =>
                    {
                        Microsoft.IdentityModel.Logging.LogHelper.LogInformation($"[SqlSugarClient],Sql:{sqlSugarException.Sql},Parametres:{sqlSugarException.Parametres},Data:{sqlSugarException.Data},Message:{sqlSugarException.Message}", sqlSugarException.InnerException);
                    }
                }
            },
            db =>
            {
                db.QueryFilter.AddTableFilter<IDeleted>(it => it.IsDeleted == false);
            });
            return dbClient;
        }

        #endregion 静态：获取Sugar客户端实例对象 + public static SqlSugarClient GetInstance()
    }
}
