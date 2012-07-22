using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace YLR.YAdoNet
{
    /// <summary>
    /// SQLServer数据库操作类
    /// </summary>
    public class YMSSQLDataBase : YDataBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public YMSSQLDataBase()
        {
            this._databaseType = DataBaseType.MSSQL;
            this.initValues();
        }

        /// <summary>
        /// 析构函数，断开数据库连接。
        /// </summary>
        ~YMSSQLDataBase()
        {
            this.disconnectDataBase();
        }

        /// <summary>
        /// 带参数的构造函数，直接初始化数据库类型
        /// </summary>
        /// <param name="type">数据库类型</param>
        public YMSSQLDataBase(DataBaseType type)
        {
            this._databaseType = type;
            this.initValues();
        }

        #region 受保护属性

        /// <summary>
        /// SqlServer数据库连接对象
        /// </summary>
        protected SqlConnection connection = new SqlConnection();

        /// <summary>
        /// 连接字符串创建对象
        /// </summary>
        protected SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();

        /// <summary>
        /// 使用的事务
        /// </summary>
        protected SqlTransaction transaction = null;

        #endregion

        #region 公有属性
        
        /// <summary>
        /// 是否使用Windows 帐户凭据进行身份验证。
        /// 默认不使用。
        /// </summary>
        public bool integratedSecurity
        {
            get
            {
                return this.connectionString.IntegratedSecurity;
            }
            set 
            {
                this.connectionString.IntegratedSecurity = value;
            }
        }

        /// <summary>
        /// 连接服务器名称或IP地址。默认是localhost。
        /// </summary>
        protected string _serverName = "localhost";
        /// <summary>
        /// 连接服务器名称或IP地址。默认是localhost。
        /// </summary>
        public string serverName
        {
            get
            {
                return this._serverName;
            }
            set
            {
                this._serverName = value;
            }
        }

        /// <summary>
        /// 连接数据库实例名，使用默认实例设置成""，默认值也是""。
        /// </summary>
        protected string _example = "";
        /// <summary>
        /// 连接数据库实例名，使用默认实例设置成""，默认值也是""。
        /// </summary>
        public string example
        {
            get
            {
                return this._example;
            }
            set
            {
                this._example = value;
            }
        }

        /// <summary>
        /// 连接数据库端口号，默认是1433。
        /// </summary>
        protected uint _port = 1433;
        /// <summary>
        /// 连接数据库端口号，默认是1433。
        /// </summary>
        public uint port
        {
            get
            {
                return this._port;
            }
            set
            {
                this._port = value;
            }
        }

        /// <summary>
        /// 要连接的数据库名称
        /// </summary>
        public string databaseName
        {
            get
            {
                return this.connectionString.InitialCatalog;
            }
            set
            {
                this.connectionString.InitialCatalog = value;
            }
        }

        /// <summary>
        /// 数据库登录名，默认是sa。
        /// </summary>
        public string userID
        {
            get
            {
                return this.connectionString.UserID;
            }
            set
            {
                this.connectionString.UserID = value;
            }
        }

        /// <summary>
        /// 用户登录密码，默认是""。
        /// </summary>
        public string userPassword
        {
            get
            {
                return this.connectionString.Password;
            }
            set
            {
                this.connectionString.Password = value;
            }
        }

        /// <summary>
        /// 获取或设置连接超时时间，默认是15。
        /// </summary>
        public uint connectTimeout
        {
            get
            {
                return (uint)this.connectionString.ConnectTimeout;
            }
            set
            {
                this.connectionString.ConnectTimeout = (int)value;
            }
        }

        /// <summary>
        /// 获取或设置连接被销毁前在连接池中存活的最短时间（以秒为单位），默认是0。
        /// </summary>
        public uint loadBalanceTimeout
        {
            get
            {
                return (uint)this.connectionString.LoadBalanceTimeout;
            }
            set
            {
                this.connectionString.LoadBalanceTimeout = (int)value;
            }
        }

        /// <summary>
        /// 获取或设置针对此特定连接字符串连接池中所允许的最大连接数。默认是5。
        /// </summary>
        public uint maxPoolSize
        {
            get
            {
                return (uint)this.connectionString.MaxPoolSize;
            }
            set
            {
                this.connectionString.MaxPoolSize = (int)value;
            }
        }

        /// <summary>
        /// 获取或设置针对此特定连接字符串连接池中所允许的最小连接数。默认是0。
        /// </summary>
        public uint minPoolSize
        {
            get
            {
                return (uint)this.connectionString.MinPoolSize;
            }
            set
            {
                this.connectionString.MinPoolSize = (int)value;
            }
        }

        /// <summary>
        /// 获取或设置一个布尔值，该值指示每次请求连接时该连接是汇入连接池还是显式打开。默认打开。
        /// </summary>
        public bool pooling
        {
            get
            {
                return this.connectionString.Pooling;
            }
            set
            {
                this.connectionString.Pooling = value;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化属性的默认值
        /// </summary>
        private void initValues()
        {
            this.connectionString.UserID = "sa";
            this.connectionString.Password = "";
            this.connectionString.MaxPoolSize = 5;
        }

        #endregion

        #region YAdoNet 成员

        /// <summary>
        /// 数据库类型，默认是MSSQL
        /// </summary>
        protected DataBaseType _databaseType = DataBaseType.MSSQL;
        /// <summary>
        /// 数据库类型，默认是MSSQL
        /// </summary>
        public DataBaseType databaseType
        {
            get
            {
                return this._databaseType;
            }
            set
            {
                this._databaseType = value;
            }
        }

        /// <summary>
        /// 错误提示信息
        /// </summary>
        protected string _errorText;
        /// <summary>
        /// 错误提示信息
        /// </summary>
        public string errorText
        {
            get { return this._errorText; }
        }

        /// <summary>
        /// 当前程序集版本号
        /// </summary>
        public string version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns>成功返回true，否则返回false</returns>
        public bool connectDataBase()
        {
            //判断是否已经连接
            try
            {
                if (ConnectionState.Closed == this.connection.State)
                {
                    //设置连接数据库的类型
                    if (DataBaseType.MSSQL == this._databaseType || DataBaseType.SQL2000 == this._databaseType)
                    {
                        this.connectionString.TypeSystemVersion = "SQL Server 2000";
                    }
                    else if (DataBaseType.SQL2005 == this._databaseType)
                    {
                        this.connectionString.TypeSystemVersion = "SQL Server 2005";
                    }
                    else if (DataBaseType.SQL2005 == this._databaseType)
                    {
                        this.connectionString.TypeSystemVersion = "SQL Server 2008";
                    }
                    else
                    {
                        this._errorText = "数据库类型错误！";
                        return false;
                    }

                    //组织连接字符串
                    string str = "";
                    if (this._example == "")
                    {
                        str = this._serverName + "," + this._port.ToString();
                    }
                    else
                    {
                        str = this._serverName + "\\" + this._example + "," + this._port.ToString();
                    }

                    this.connectionString.DataSource = str;

                    //连接数据库
                    this.connection.ConnectionString = this.connectionString.ToString();

                    this.connection.Open();
                }

                return true;
            }
            catch (Exception ex)
            {
                this._errorText = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 断开数据库连接
        /// </summary>
        /// <returns>成功返回true，否则返回false</returns>
        public bool disconnectDataBase()
        {
            try
            {
                if (ConnectionState.Closed != this.connection.State)
                {
                    this.connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                this._errorText = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns>成功返回turu，否则返回false</returns>
        public bool beginTransaction()
        {
            try
            {
                if (this.transaction == null)
                {
                    this.transaction = this.connection.BeginTransaction();
                    if (this.transaction == null)
                    {
                        this._errorText = "开启事务返回null值！";
                        return false;
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                this._errorText = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns>成功返回true，否则返回false</returns>
        public bool commitTransaction()
        {
            try
            {
                if (this.transaction != null)
                {
                    this.transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                this._errorText = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <returns>成功返回true，否则返回false</returns>
        public bool rollbackTransaction()
        {
            try
            {
                if (this.transaction != null)
                {
                    this.transaction.Rollback();
                }

                return true;
            }
            catch (Exception ex)
            {
                this._errorText = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 执行带数据集返回的sql语句
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <returns>返回结果集，如果执行失败返回null</returns>
        public DataTable executeSqlReturnDt(string sql)
        {
            if (sql == null || sql == "")
            {
                this._errorText = "未设置执行语句！";
                return null;
            }

            try
            {
                SqlCommand command = this.connection.CreateCommand();

                //是否使用事务
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                //设置执行语句
                command.CommandText = sql;

                //获取数据集
                SqlDataReader dr = command.ExecuteReader();

                //绑定数据到DataTable
                if (dr != null)
                {
                    DataTable dt = new DataTable();
                    //添加列
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        dt.Columns.Add(dr.GetName(i), dr.GetFieldType(i));
                    }

                    //添加行
                    while (dr.Read())
                    {
                        DataRow row = dt.NewRow();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            row[i] = dr.GetValue(i);
                        }
                        dt.Rows.Add(row);
                    }

                    dr.Close();
                    return dt;
                }
                else
                {
                    this._errorText = "返回数据集为null！";
                    return null;
                }
            }
            catch (Exception ex)
            {
                this._errorText = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 执行不带数据集返回的sql语句
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <returns>返回应用行数，-1表式出错</returns>
        public int executeSqlWithOutDs(string sql)
        {
            if (sql == null || sql == "")
            {
                this._errorText = "未设置执行语句！";
                return -1;
            }

            try
            {
                SqlCommand command = this.connection.CreateCommand();

                //是否使用事务
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                //设置执行语句
                command.CommandText = sql;

                //执行
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                this._errorText = ex.Message;
                return -1;
            }
        }

        #endregion
    }
}
