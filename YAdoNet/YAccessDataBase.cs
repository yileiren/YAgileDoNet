using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Reflection;

namespace YLR.YAdoNet
{
    /// <summary>
    /// 微软Access数据库操作
    /// </summary>
    public class YAccessDataBase : YDataBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public YAccessDataBase()
        {
            
        }

        /// <summary>
        /// 析构函数，断开数据库连接。
        /// </summary>
        ~YAccessDataBase()
        {
            //对象释放时断开连接。
            this.disconnectDataBase();
        }

        #region 公有属性

        #endregion

        
        #region 私有属性
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        protected OleDbConnection connection = new OleDbConnection();

        /// <summary>
        /// 数据库连接使用的事务
        /// </summary>
        protected OleDbTransaction transaction = null;

        #endregion

        #region YAdoNet 成员

        /// <summary>
        /// 数据库类型
        /// </summary>
        protected DataBaseType _databaseType = DataBaseType.Access;
        /// <summary>
        /// 数据库类型
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
        /// 数据库文件路径
        /// </summary>
        protected string _filePath = "";
        /// <summary>
        /// 数据库文件路径
        /// </summary>
        public string filePath
        {
            get
            {
                return this._filePath;
            }
            set
            {
                this._filePath = value;
            }
        }

        /// <summary>
        /// 数据库访问密码
        /// </summary>
        protected string _password = "";
        /// <summary>
        /// 数据库访问密码
        /// </summary>
        public string password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
            }
        }

        /// <summary>
        /// 上次出错的错误信息
        /// </summary>
        public string _errorText = "";
        /// <summary>
        /// 上次出错的错误信息
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
            //判断路径是否设置
            if (this._filePath == "")
            {
                this._errorText = "未设置数据库文件路径！";
                return false;
            }

            //连接数据库
            try
            {
                if (ConnectionState.Closed == this.connection.State)
                {
                    //组织连接字符串
                    //"Provider=Microsoft.ACE.OLEDB.12.0; Data Source="
                    //Provider=Microsoft.Jet.OLEDB.4.0; Data Source=d:\Northwind.mdb;User ID=Admin;Password=;
                    this.connection.ConnectionString = "Microsoft.Jet.OLEDB.4.0; Data Source=" + this._filePath + ";User ID=Admin;Password=" + this._password + ";";

                    //连接
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
        /// 开启事务并指定事务的隔离级别。
        /// </summary>
        /// <param name="isolationLevel">隔离级别</param>
        /// <returns>成功返回turu，否则返回false</returns>
        public bool beginTransaction(IsolationLevel isolationLevel)
        {
            try
            {
                if (this.transaction == null)
                {
                    this.transaction = this.connection.BeginTransaction(isolationLevel);
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
        public System.Data.DataTable executeSqlReturnDt(string sql)
        {
            if (sql == null || sql == "")
            {
                this._errorText = "未设置执行语句！";
                return null;
            }

            try
            {
                OleDbCommand command = this.connection.CreateCommand();

                //是否使用事务
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                //设置执行语句
                command.CommandText = sql;

                //获取数据集
                OleDbDataReader dr = command.ExecuteReader();

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
        /// 获取分页数据集。
        /// </summary>
        /// <param name="sql">sql语句，语句获取所有数据，方法自动为数据分页。</param>
        /// <param name="pageNum">要获取的页号，从1开始。</param>
        /// <param name="dataCount">每页显示的数据总数。</param>
        /// <returns>分页数据。</returns>
        public PagerData executeSqlReturnDt(string sql, int pageNum, int dataCount)
        {
            if (sql == null || sql == "")
            {
                this._errorText = "未设置执行语句！";
                return null;
            }

            try
            {
                OleDbCommand command = this.connection.CreateCommand();

                //是否使用事务
                if (this.transaction != null)
                {
                    command.Transaction = this.transaction;
                }

                //设置执行语句
                command.CommandText = sql;

                //获取数据集
                OleDbDataReader dr = command.ExecuteReader();

                //绑定数据到DataTable
                if (dr != null)
                {
                    DataTable dt = new DataTable();
                    //添加列
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        dt.Columns.Add(dr.GetName(i), dr.GetFieldType(i));
                    }

                    int rowCount = 0; //总行数。

                    if (pageNum <= 0)
                    {
                        pageNum = 1;
                    }

                    if (dataCount <= 0)
                    {
                        dataCount = 20;
                    }

                    //添加行
                    while (dr.Read())
                    {
                        rowCount++;

                        if (rowCount > (pageNum - 1) * dataCount && rowCount <= pageNum * dataCount)
                        {
                            DataRow row = dt.NewRow();
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                row[i] = dr.GetValue(i);
                            }
                            dt.Rows.Add(row);
                        }
                    }
                    dr.Close();

                    PagerData retData = new PagerData();
                    retData.data = dt;
                    retData.dataCount = dataCount;
                    retData.pageNum = pageNum;

                    if (rowCount % dataCount == 0)
                    {
                        retData.pageCount = rowCount / dataCount;
                    }
                    else
                    {
                        retData.pageCount = rowCount / dataCount + 1;
                    }

                    return retData;
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
                OleDbCommand command = this.connection.CreateCommand();

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
