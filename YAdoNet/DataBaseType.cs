namespace YLR.YAdoNet
{
    /// <summary>
    /// 所要操作的数据库类型
    /// </summary>
    public enum DataBaseType
    {
        /// <summary>
        /// SQLServer通用类型
        /// </summary>
        MSSQL,
        /// <summary>
        /// SQLServer2000数据库
        /// </summary>
        SQL2000,
        /// <summary>
        /// SQLServer2005数据库
        /// </summary>
        SQL2005,
        /// <summary>
        /// SQLServer2008数据库
        /// </summary>
        SQL2008,
        /// <summary>
        /// 微软Access数据库
        /// </summary>
        Access,
        /// <summary>
        /// 微软Access2007数据库。
        /// </summary>
        Access2007,
        /// <summary>
        /// SQLite数据库。
        /// </summary>
        SQlite,
        /// <summary>
        /// 位置类型。
        /// </summary>
        Unknown
    }
}