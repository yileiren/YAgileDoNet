using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

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
        /// <returns>数据库实例，失败返回null。</returns>
        static public YDataBase createDataBase(string configFile, string nodeName)
        {
            YDataBase retDb = null;
            try
            {
                //配置文件
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configFile);
                XmlNode dbConfig = xmlDoc.SelectSingleNode("DataBaseConfig");

                if (dbConfig != null)
                {
                    XmlNode orgDB = dbConfig.SelectSingleNode(nodeName);

                    //数据库类型。
                    DataBaseType dbType = DataBaseType.MSSQL;
                    if(!string.IsNullOrEmpty(orgDB.Attributes["databaseType"].Value))
                    {
                        if (orgDB.Attributes["databaseType"].Value == "SQL2000")
                        {
                            dbType = DataBaseType.SQL2000;
                        }
                        else if (orgDB.Attributes["databaseType"].Value == "SQL2005")
                        {
                            dbType = DataBaseType.SQL2005;
                        }
                        else if (orgDB.Attributes["databaseType"].Value == "SQL2008")
                        {
                            dbType = DataBaseType.SQL2008;
                        }
                        else
                        {
                            dbType = DataBaseType.MSSQL;
                        }
                    }

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
                                XmlNode serverName = orgDB.SelectSingleNode("ServerName");
                                if (serverName != null)
                                {
                                    db.serverName = serverName.InnerXml;
                                }
                                else
                                {
                                    //没配置则返回失败。
                                    break;
                                }

                                //获取端口，获取没配置使用默认端口。
                                XmlNode serverPort = orgDB.SelectSingleNode("ServerPort");
                                if (serverPort != null)
                                {
                                    db.port = Convert.ToUInt32(serverPort.InnerXml);
                                }

                                //数据库实例，不配置使用默认实例。
                                XmlNode example = orgDB.SelectSingleNode("Example");
                                if (example != null)
                                {
                                    db.example = example.InnerXml;
                                }

                                //数据库名
                                XmlNode dataBaseName = orgDB.SelectSingleNode("DataBaseName");
                                if (dataBaseName != null)
                                {
                                    db.databaseName = dataBaseName.InnerXml;
                                }
                                else
                                {
                                    //没配置则返回失败。
                                    break;
                                }

                                //用户id
                                XmlNode userID = orgDB.SelectSingleNode("UserID");
                                if (userID != null)
                                {
                                    db.userID = userID.InnerXml;
                                }
                                else
                                {
                                    //没配置则返回失败。
                                    break;
                                }

                                //登陆密码
                                XmlNode userPassword = orgDB.SelectSingleNode("UserPassword");
                                if (userPassword != null)
                                {
                                    db.userPassword = userPassword.InnerXml;
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

                    

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retDb;
        }
    }
}
