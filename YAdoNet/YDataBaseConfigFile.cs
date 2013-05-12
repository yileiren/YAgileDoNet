using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using YLR.YCrypto;

namespace YLR.YAdoNet
{
    /// <summary>
    /// 数据库配置文件操作类。
    /// </summary>
    public class YDataBaseConfigFile
    {
        /// <summary>
        /// 创建数据库实例。
        /// </summary>
        /// <param name="configFile">配置文件。</param>
        /// <param name="nodeName">配置文件节点名称。</param>
        /// <param name="key">使用的加密密码，目前支持AES和DES两种加密算法，如果没有密码使用空字符串。</param>
        /// <returns>数据库实例，失败返回null。</returns>
        static public YDataBase createDataBase(string configFile, string nodeName,string key)
        {
            YDataBase retDb = null;
            try
            {
                //配置节点
                XmlNode orgDB = YDataBaseConfigFile.getConfigNode(configFile,nodeName);
                if (orgDB != null)
                {
                    //数据库类型。
                    DataBaseType dbType = YDataBaseConfigFile.getDataBaseType(orgDB);

                    //创建数据库实例
                    switch (dbType)
                    {
                        case DataBaseType.MSSQL:
                        case DataBaseType.SQL2000:
                        case DataBaseType.SQL2005:
                        case DataBaseType.SQL2008:
                            {
                                //SQLServer数据库
                                retDb = YDataBaseConfigFile.createMSSQLDataBase(orgDB,key);
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retDb;
        }

        /// <summary>
        /// 创建SqlServer数据库实例。
        /// </summary>
        /// <param name="configNode">配置节点。</param>
        /// <param name="key">使用的加密密码，目前支持AES和DES两种加密算法，如果没有密码使用空字符串。</param>
        /// <returns>成功返回SqlServer实例，否则返回false。</returns>
        static public YMSSQLDataBase createMSSQLDataBase(XmlNode configNode,string key)
        {
            YMSSQLDataBase retDb = null;

            DataBaseType dbType = YDataBaseConfigFile.getDataBaseType(configNode);
            //创建数据库实例
            switch (dbType)
            {
                case DataBaseType.MSSQL:
                case DataBaseType.SQL2000:
                case DataBaseType.SQL2005:
                case DataBaseType.SQL2008:
                    {
                        //SQLServer数据库
                        YMSSQLDataBase db = new YMSSQLDataBase(dbType);

                        //获取验证方式
                        XmlNode integratedSecurity = configNode.SelectSingleNode("IntegratedSecurity");
                        if (integratedSecurity != null)
                        {
                            if ("true" == integratedSecurity.InnerXml)
                            {
                                db.integratedSecurity = true;
                            }
                            else
                            {
                                db.integratedSecurity = false;
                            }
                        }

                        //获取服务器
                        XmlNode serverName = configNode.SelectSingleNode("ServerName");
                        if (serverName != null)
                        {
                            if (null == serverName.Attributes["Crypto"] || "" == serverName.Attributes["Crypto"].Value || "NO" == serverName.Attributes["Crypto"].Value)
                            {
                                db.serverName = serverName.InnerXml;
                            }
                            else if ("AES" == serverName.Attributes["Crypto"].Value)
                            {
                                db.serverName = Encoding.UTF8.GetString(AESEncrypt.decrypt(Convert.FromBase64String(serverName.InnerXml), key)).Replace("\0","");
                            }
                            else if ("DES" == serverName.Attributes["Crypto"].Value)
                            {
                                db.serverName = Encoding.UTF8.GetString(DESEncrypt.decrypt(Convert.FromBase64String(serverName.InnerXml), key)).Replace("\0", "");
                            }
                            else
                            {
                                db.serverName = serverName.InnerXml;
                            }
                        }
                        else
                        {
                            //没配置则返回失败。
                            break;
                        }

                        //获取端口，获取没配置使用默认端口。
                        XmlNode serverPort = configNode.SelectSingleNode("ServerPort");
                        if (serverPort != null)
                        {
                            if (null == serverPort.Attributes["Crypto"] || "" == serverPort.Attributes["Crypto"].Value || "NO" == serverPort.Attributes["Crypto"].Value)
                            {
                                db.port = Convert.ToUInt32(serverPort.InnerXml);
                            }
                            else if ("AES" == serverPort.Attributes["Crypto"].Value)
                            {
                                db.port = Convert.ToUInt32(Encoding.UTF8.GetString(AESEncrypt.decrypt(Convert.FromBase64String(serverPort.InnerXml), key)).Replace("\0", ""));
                            }
                            else if ("DES" == serverPort.Attributes["Crypto"].Value)
                            {
                                db.port = Convert.ToUInt32((Encoding.UTF8.GetString(DESEncrypt.decrypt(Convert.FromBase64String(serverPort.InnerXml), key))).Replace("\0", ""));
                            }
                            else
                            {
                                db.port = Convert.ToUInt32(serverPort.InnerXml);
                            }
                            
                        }

                        //数据库实例，不配置使用默认实例。
                        XmlNode example = configNode.SelectSingleNode("Example");
                        if (example != null)
                        {
                            if (null == example.Attributes["Crypto"] || "" == example.Attributes["Crypto"].Value || "NO" == example.Attributes["Crypto"].Value)
                            {
                                db.example = example.InnerXml;
                            }
                            else if ("AES" == example.Attributes["Crypto"].Value)
                            {
                                db.example = Encoding.UTF8.GetString(AESEncrypt.decrypt(Convert.FromBase64String(example.InnerXml), key)).Replace("\0", "");
                            }
                            else if ("DES" == example.Attributes["Crypto"].Value)
                            {
                                db.example = Encoding.UTF8.GetString(DESEncrypt.decrypt(Convert.FromBase64String(example.InnerXml), key)).Replace("\0", "");
                            }
                            else
                            {
                                db.example = example.InnerXml;
                            }
                        }

                        //数据库名
                        XmlNode dataBaseName = configNode.SelectSingleNode("DataBaseName");
                        if (dataBaseName != null)
                        {
                            if (null == dataBaseName.Attributes["Crypto"] || "" == dataBaseName.Attributes["Crypto"].Value || "NO" == dataBaseName.Attributes["Crypto"].Value)
                            {
                                db.databaseName = dataBaseName.InnerXml;
                            }
                            else if ("AES" == dataBaseName.Attributes["Crypto"].Value)
                            {
                                db.databaseName = Encoding.UTF8.GetString(AESEncrypt.decrypt(Convert.FromBase64String(dataBaseName.InnerXml), key)).Replace("\0", "");
                            }
                            else if ("DES" == dataBaseName.Attributes["Crypto"].Value)
                            {
                                db.databaseName = Encoding.UTF8.GetString(DESEncrypt.decrypt(Convert.FromBase64String(dataBaseName.InnerXml), key)).Replace("\0", "");
                            }
                            else
                            {
                                db.databaseName = dataBaseName.InnerXml;
                            }
                        }
                        else
                        {
                            //没配置则返回失败。
                            break;
                        }

                        //用户id
                        XmlNode userID = configNode.SelectSingleNode("UserID");
                        if (userID != null)
                        {
                            if (null == userID.Attributes["Crypto"] || "" == userID.Attributes["Crypto"].Value || "NO" == userID.Attributes["Crypto"].Value)
                            {
                                db.userID = userID.InnerXml;
                            }
                            else if ("AES" == userID.Attributes["Crypto"].Value)
                            {
                                db.userID = Encoding.UTF8.GetString(AESEncrypt.decrypt(Convert.FromBase64String(userID.InnerXml), key)).Replace("\0", "");
                            }
                            else if ("DES" == userID.Attributes["Crypto"].Value)
                            {
                                db.userID = Encoding.UTF8.GetString(DESEncrypt.decrypt(Convert.FromBase64String(userID.InnerXml), key)).Replace("\0", "");
                            }
                            else
                            {
                                db.userID = userID.InnerXml;
                            }
                        }

                        //登陆密码
                        XmlNode userPassword = configNode.SelectSingleNode("UserPassword");
                        if (userPassword != null)
                        {
                            if (null == userPassword.Attributes["Crypto"] || "" == userPassword.Attributes["Crypto"].Value || "NO" == userPassword.Attributes["Crypto"].Value)
                            {
                                db.userPassword = userPassword.InnerXml;
                            }
                            else if ("AES" == userPassword.Attributes["Crypto"].Value)
                            {
                                db.userPassword = Encoding.UTF8.GetString(AESEncrypt.decrypt(Convert.FromBase64String(userPassword.InnerXml), key)).Replace("\0", "");
                            }
                            else if ("DES" == userPassword.Attributes["Crypto"].Value)
                            {
                                db.userPassword = Encoding.UTF8.GetString(DESEncrypt.decrypt(Convert.FromBase64String(userPassword.InnerXml), key)).Replace("\0", "");
                            }
                            else
                            {
                                db.userPassword = userPassword.InnerXml;
                            }
                        }

                        //获取连接超时时间。
                        XmlNode connectTimeout = configNode.SelectSingleNode("ConnectTimeout");
                        if (connectTimeout != null)
                        {
                            db.connectTimeout = Convert.ToInt32(connectTimeout.InnerXml);
                        }

                        //获取存活的最短时间。
                        XmlNode loadBalanceTimeout = configNode.SelectSingleNode("LoadBalanceTimeout");
                        if (loadBalanceTimeout != null)
                        {
                            db.loadBalanceTimeout = Convert.ToInt32(loadBalanceTimeout.InnerXml);
                        }

                        //获取连接字符串连接池中所允许的最大连接数。
                        XmlNode maxPoolSize = configNode.SelectSingleNode("MaxPoolSize");
                        if (maxPoolSize != null)
                        {
                            db.maxPoolSize = Convert.ToInt32(maxPoolSize.InnerXml);
                        }

                        //获取特定连接字符串连接池中所允许的最小连接数。
                        XmlNode minPoolSize = configNode.SelectSingleNode("MinPoolSize");
                        if (minPoolSize != null)
                        {
                            db.minPoolSize = Convert.ToInt32(minPoolSize.InnerXml);
                        }

                        //获取指示每次请求连接时该连接是汇入连接池还是显式打开。
                        XmlNode pooling = configNode.SelectSingleNode("Pooling");
                        if (pooling != null)
                        {
                            if ("true" == pooling.InnerXml)
                            {
                                db.pooling = true;
                            }
                            else
                            {
                                db.pooling = false;
                            }
                        }

                        retDb = db;
                        break;
                    }
            }

            return retDb;
        }

        /// <summary>
        /// 创建微软SqlServer数据库实例。
        /// </summary>
        /// <param name="configFile">配置文件路径。</param>
        /// <param name="nodeName">配置节点名称。</param>
        /// <param name="key">使用的加密密码，目前支持AES和DES两种加密算法，如果没有密码使用空字符串。</param>
        /// <returns>成功返回SqlServer实例，否则返回false。</returns>
        static public YMSSQLDataBase createMSSQLDataBase(string configFile, string nodeName,string key)
        {
            YMSSQLDataBase retDb = null;

            try
            {
                XmlNode configNode = YDataBaseConfigFile.getConfigNode(configFile, nodeName);
                if (configNode != null)
                {
                    retDb = YDataBaseConfigFile.createMSSQLDataBase(configNode,key);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retDb;
        }

        /// <summary>
        /// 获取配置节点。
        /// </summary>
        /// <param name="configFile">配置文件。</param>
        /// <param name="nodeName">节点名称。</param>
        /// <returns>成功返回节点，否则返回null。</returns>
        static public XmlNode getConfigNode(string configFile, string nodeName)
        {
            XmlNode retNode = null; //返回的节点对象。
            try
            {
                //配置文件
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configFile);
                XmlNode dbConfig = xmlDoc.SelectSingleNode("DataBaseConfig");

                if (dbConfig != null)
                {
                    retNode = dbConfig.SelectSingleNode(nodeName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retNode;
        }

        /// <summary>
        /// 获取数据库类型。
        /// </summary>
        /// <param name="configNode">配置节点。</param>
        /// <returns>数据库类型。</returns>
        static public DataBaseType getDataBaseType(XmlNode configNode)
        {
            DataBaseType type = DataBaseType.Unknown;
            try
            {
                if (null != configNode.Attributes["databaseType"] && !string.IsNullOrEmpty(configNode.Attributes["databaseType"].Value))
                {
                    if (configNode.Attributes["databaseType"].Value == "SQL2000")
                    {
                        type = DataBaseType.SQL2000;
                    }
                    else if (configNode.Attributes["databaseType"].Value == "SQL2005")
                    {
                        type = DataBaseType.SQL2005;
                    }
                    else if (configNode.Attributes["databaseType"].Value == "SQL2008")
                    {
                        type = DataBaseType.SQL2008;
                    }
                    else if (configNode.Attributes["databaseType"].Value == "MSSQL")
                    {
                        type = DataBaseType.MSSQL;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return type;
        }

        /// <summary>
        /// 获取数据库类型。
        /// </summary>
        /// <param name="configFile">配置文件。</param>
        /// <param name="nodeName">节点名称。</param>
        /// <returns>数据库类型。</returns>
        static public DataBaseType getDataBaseType(string configFile, string nodeName)
        {
            DataBaseType type = DataBaseType.Unknown;
            try
            {
                XmlNode configNode = YDataBaseConfigFile.getConfigNode(configFile, nodeName);
                if (configNode != null)
                {
                    type = YDataBaseConfigFile.getDataBaseType(configNode);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return type;
        }

        
    }
}
