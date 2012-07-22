using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Reflection;
using System.Data;

namespace YLR.YAdoNet
{
    /// <summary>
    /// Sqlite数据库操作封装。
    /// </summary>
    public class YSQLiteDataBase : YDataBase
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public YSQLiteDataBase()
        { 
        }

        /// <summary>
        /// 析构函数，断开数据库连接。
        /// </summary>
        ~YSQLiteDataBase()
        {
            this.disconnectDataBase();
        }

        #region YDataBase 成员

        /// <summary>
        /// 数据库类型。
        /// </summary>
        protected DataBaseType _databaseType = DataBaseType.SQlite;
        /// <summary>
        /// 数据库类型。
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
        /// 错误信息。
        /// </summary>
        protected string _errorText = "";

        /// <summary>
        /// 错误信息。
        /// </summary>
        public string errorText
        {
            get { return this._errorText; }
        }

        /// <summary>
        /// 程序版本号。
        /// </summary>
        public string version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); ; }
        }

        /// <summary>
        /// 数据库文件路径。
        /// </summary>
        protected string _filePath = "";

        /// <summary>
        /// 数据库文件路径。
        /// </summary>
        public string filePaht 
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
        /// 数据库事务对象。
        /// </summary>
        protected SQLiteTransaction _transaction = null;

        /// <summary>
        /// SQLite数据库连接对象。
        /// </summary>
        protected SQLiteConnection _connection = new SQLiteConnection();

        /// <summary>
        /// 连接数据库。
        /// </summary>
        /// <returns></returns>
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
                if (ConnectionState.Closed == this._connection.State)
                {
                    //组织连接字符串
                    //Provider=Microsoft.Jet.OLEDB.4.0; Data Source=d:\Northwind.mdb;User ID=Admin;Password=;
                    this._connection.ConnectionString = "Data Source=" + this._filePath + ";";

                    //连接
                    this._connection.Open();
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
        /// 断开数据库连接。
        /// </summary>
        /// <returns></returns>
        public bool disconnectDataBase()
        {
            try
            {
                if (ConnectionState.Closed != this._connection.State)
                {
                    this._connection.Close();
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
        /// 开启事务。
        /// </summary>
        /// <returns></returns>
        public bool beginTransaction()
        {
            try
            {
                if (this._transaction == null)
                {
                    this._transaction = this._connection.BeginTransaction();
                    if (this._transaction == null)
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
        /// 提交事务。
        /// </summary>
        /// <returns></returns>
        public bool commitTransaction()
        {
            try
            {
                if (this._transaction != null)
                {
                    this._transaction.Commit();
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
        /// 回滚事务。
        /// </summary>
        /// <returns></returns>
        public bool rollbackTransaction()
        {
            try
            {
                if (this._transaction != null)
                {
                    this._transaction.Rollback();
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
        /// 执行带数据集返回的SQL语句。
        /// </summary>
        /// <param name="sql">SQL语句。</param>
        /// <returns>返回数据集，失败返回null。</returns>
        public System.Data.DataTable executeSqlReturnDt(string sql)
        {
            if (sql == null || sql == "")
            {
                this._errorText = "未设置执行语句！";
                return null;
            }

            try
            {
                SQLiteCommand command = this._connection.CreateCommand();

                //是否使用事务
                if (this._transaction != null)
                {
                    command.Transaction = this._transaction;
                }

                //设置执行语句
                command.CommandText = sql;

                //获取数据集
                SQLiteDataReader dr = command.ExecuteReader();

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
        /// 执行不带数据集返回的SQL语句。
        /// </summary>
        /// <param name="sql">SQL语句。</param>
        /// <returns>返回响应行数，失败返回-1。</returns>
        public int executeSqlWithOutDs(string sql)
        {
            if (sql == null || sql == "")
            {
                this._errorText = "未设置执行语句！";
                return -1;
            }

            try
            {
                SQLiteCommand command = this._connection.CreateCommand();

                //是否使用事务
                if (this._transaction != null)
                {
                    command.Transaction = this._transaction;
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
