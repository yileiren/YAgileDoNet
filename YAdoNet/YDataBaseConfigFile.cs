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

                        //获取服务器
                        XmlNode serverName = configNode.SelectSingleNode("ServerName");
                        if (serverName != null)
                        {
                            string serverNameIsCrypto = serverName.Attributes["Crypto"].Value;
                            if (!string.IsNullOrEmpty(serverNameIsCrypto) || "NO" == serverNameIsCrypto)
                            {
                                db.serverName = serverName.InnerXml;
                            }
                            else if ("AES" == serverNameIsCrypto)
                            {
                                db.serverName = Encoding.UTF8.GetString(AESEncrypt.decrypt(Convert.FromBase64String(serverName.InnerXml), key));
                            }
                            else if ("DES" == serverNameIsCrypto)
                            {
                                db.serverName = Encoding.UTF8.GetString(DESEncrypt.decrypt(Convert.FromBase64String(serverName.InnerXml), key));
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
                            string serverPortIsCrypto = serverPort.Attributes["Crypto"].Value;
                            if (!string.IsNullOrEmpty(serverPortIsCrypto) || "NO" == serverPortIsCrypto)
                            {
                                db.port = Convert.ToUInt32(serverPort.InnerXml);
                            }
                            else if ("AES" == serverPortIsCrypto)
                            {
                                db.port = Convert.ToUInt32(Encoding.UTF8.GetString(AESEncrypt.decrypt(Convert.FromBase64String(serverPort.InnerXml), key)));
                            }
                            else if ("DES" == serverPortIsCrypto)
                            {
                                db.port = Convert.ToUInt32((Encoding.UTF8.GetString(DESEncrypt.decrypt(Convert.FromBase64String(serverPort.InnerXml), key))));
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
                            string exampleIsCrypto = example.Attributes["Crypto"].Value;
                            if (!string.IsNullOrEmpty(exampleIsCrypto) || "NO" == exampleIsCrypto)
                            {
                                db.example = example.InnerXml;
                            }
                            else if ("AES" == exampleIsCrypto)
                            {
                                db.example = Encoding.UTF8.GetString(AESEncrypt.decrypt(Convert.FromBase64String(example.InnerXml), key));
                            }
                            else if ("DES" == exampleIsCrypto)
                            {
                                db.example = Encoding.UTF8.GetString(DESEncrypt.decrypt(Convert.FromBase64String(example.InnerXml), key));
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
                            string dataBaseNameIsCrypto = dataBaseName.Attributes["Crypto"].Value;
                            if (!string.IsNullOrEmpty(dataBaseNameIsCrypto) || "NO" == dataBaseNameIsCrypto)
                            {
                                db.databaseName = dataBaseName.InnerXml;
                            }
                            else if ("AES" == dataBaseNameIsCrypto)
                            {
                                db.databaseName = Encoding.UTF8.GetString(AESEncrypt.decrypt(Convert.FromBase64String(dataBaseName.InnerXml), key));
                            }
                            else if ("DES" == dataBaseNameIsCrypto)
                            {
                                db.databaseName = Encoding.UTF8.GetString(DESEncrypt.decrypt(Convert.FromBase64String(dataBaseName.InnerXml), key));
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
                            string userIDIsCrypto = userID.Attributes["Crypto"].Value;
                            if (!string.IsNullOrEmpty(userIDIsCrypto) || "NO" == userIDIsCrypto)
                            {
                                db.userID = userID.InnerXml;
                            }
                            else if ("AES" == userIDIsCrypto)
                            {
                                db.userID = Encoding.UTF8.GetString(AESEncrypt.decrypt(Convert.FromBase64String(userID.InnerXml), key));
                            }
                            else if ("DES" == userIDIsCrypto)
                            {
                                db.userID = Encoding.UTF8.GetString(DESEncrypt.decrypt(Convert.FromBase64String(userID.InnerXml), key));
                            }
                            else
                            {
                                db.userID = userID.InnerXml;
                            }
                        }
                        else
                        {
                            //没配置则返回失败。
                            break;
                        }

                        //登陆密码
                        XmlNode userPassword = configNode.SelectSingleNode("UserPassword");
                        if (userPassword != null)
                        {
                            string userPasswordIsCrypto = userPassword.Attributes["Crypto"].Value;
                            if (!string.IsNullOrEmpty(userPasswordIsCrypto) || "NO" == userPasswordIsCrypto)
                            {
                                db.userPassword = userPassword.InnerXml;
                            }
                            else if ("AES" == userPasswordIsCrypto)
                            {
                                db.userPassword = Encoding.UTF8.GetString(AESEncrypt.decrypt(Convert.FromBase64String(userPassword.InnerXml), key));
                            }
                            else if ("DES" == userPasswordIsCrypto)
                            {
                                db.userPassword = Encoding.UTF8.GetString(DESEncrypt.decrypt(Convert.FromBase64String(userPassword.InnerXml), key));
                            }
                            else
                            {
                                db.userPassword = userPassword.InnerXml;
                            }
                        }
                        else
                        {
                            //没配置则返回失败。
                            break;
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
                if (!string.IsNullOrEmpty(configNode.Attributes["databaseType"].Value))
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
