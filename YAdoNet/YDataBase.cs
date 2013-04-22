using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace YLR.YAdoNet
{
    /// <summary>
    /// YAdoNet为数据库持久层访问的类库封装，主要完成对数据库操作的功能，
    /// 针对不同类型的数据库设计封装类，使数据库程序开发变的更加直观。
    /// </summary>
    public interface YDataBase
    {
        /// <summary>
        /// 访问的数据库类型
        /// </summary>
        DataBaseType databaseType
        {
            get;
            set;
        }

        /// <summary>
        /// 上次操作的错误提示字符串
        /// </summary>
        string errorText
        {
            get;
        }

        /// <summary>
        /// 获取当前类库版本号。
        /// </summary>
        string version
        {
            get;
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns>成功返回true，否则返回false</returns>
        bool connectDataBase();

        /// <summary>
        /// 断开数据库连接
        /// </summary>
        /// <returns>成功返回true，否则返回false</returns>
        bool disconnectDataBase();

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns>成功返回true，否则返回false</returns>
        bool beginTransaction();

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="isolationLevel">事务隔离级别，使用ADO.NET的定义。</param>
        /// <returns>成功返回true，否则返回false</returns>
        bool beginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns>成功返回true，否则返回false</returns>
        bool commitTransaction();

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <returns>成功返回true，否则返回false</returns>
        bool rollbackTransaction();

        /// <summary>
        /// 执行带数据集返回的sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回数据集，null表式执行失败，可以通过errorText属性查看失败信息</returns>
        DataTable executeSqlReturnDt(string sql);

        /// <summary>
        /// 执行带数据集返回的sql语句，语句中含有防止数据注入使用的参数。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">使用的参数</param>
        /// <returns>返回数据集，null表式执行失败，可以通过errorText属性查看失败信息</returns>
        DataTable executeSqlReturnDt(string sql, YParameters parameters);

        /// <summary>
        /// 获取分页数据集。
        /// </summary>
        /// <param name="sql">sql语句，语句获取所有数据，方法自动为数据分页。</param>
        /// <param name="pageNum">要获取的页号，从1开始。</param>
        /// <param name="dataCount">每页显示的数据总数。</param>
        /// <returns>分页数据。</returns>
        PagerData executeSqlReturnDt(string sql,int pageNum,int dataCount);

        /// <summary>
        /// 执行不带数据集返回的sql语句
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <returns>返回响应函数，-1表式执行失败</returns>
        int executeSqlWithOutDs(string sql);

        /// <summary>
        /// 执行不带数据集返回的sql语句，语句中含有防止数据注入使用的参数。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">使用的参数</param>
        /// <returns>返回响应函数，-1表式执行失败</returns>
        int executeSqlWithOutDs(string sql, YParameters parameters);
    }
}
