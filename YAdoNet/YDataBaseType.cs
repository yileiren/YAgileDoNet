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
    public interface YDataBaseType
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
        /// 执行不带数据集返回的sql语句
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <returns>返回响应函数，-1表式执行失败</returns>
        int executeSqlWithOutDs(string sql);
    }
}
