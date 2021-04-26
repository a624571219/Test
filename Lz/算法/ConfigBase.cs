using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Configuration; 
using System.Xml;
using System.Xml.Serialization;

namespace 调试者
{
    /// <summary>
    /// 配置文件基类
    /// </summary>
    /// <typeparam name="T">具体基类</typeparam>
    [Serializable]
    public abstract class ConfigBase<T>:ICloneable
    {
        /// <summary>
        /// 配置文件存放的路径
        /// </summary>
        [XmlIgnore]
        public string ConfigPath = "";
        /// <summary>
        /// 配置文件的文件名
        /// </summary>
        [XmlIgnore]
        public string ConfigFile = "DefCfg.xml";

        /// <summary>
        /// 配置文件基类
        /// </summary>
        public ConfigBase()
        {

        }
        /// <summary>
        /// 设置配置文件的路径和文件名
        /// </summary>
        /// <param name="path">保存路径</param>
        /// <param name="file">文件名</param>
        public abstract void SetFile(string path, string file);

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="t">必须初始化</param>
        /// <returns></returns>
        public static bool Read(ref T t)
        {
            try
            {
                t = (T)XMLHelper.ReadFile2Object(typeof(T),
                    (t as ConfigBase<T>).ConfigPath,
                    (t as ConfigBase<T>).ConfigFile, true, false);
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 读取和创建，如果无法读取，则把当前的参数进行保存。
        /// </summary>
        /// <param name="t">必须初始化，否则无法保存</param>
        /// <returns></returns>
        public static bool ReadCreat(ref T t)
        {
            T t1 =(T)(t as ConfigBase<T>).Clone();
            if (Read(ref t1))
            {
                t = t1;
                return true;
            }
            else
            {
                if (!t.Equals(default(T)))//当前输入的t不为空，则保存
                {
                    return Save(t);
                }
                return false;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="t">必须初始化</param>
        /// <returns></returns>
        public static bool Save(T t)
        {
            ConfigBase<T> bc = t as ConfigBase<T>;
            return XMLHelper.SaveObject2File(t,
                    (t as ConfigBase<T>).ConfigPath,
                    (t as ConfigBase<T>).ConfigFile, true, false);
        }

        #region ICloneable 成员

        public object Clone()
        {
            return this;
        }

        #endregion
    }
}


//#region 配置文件示例
///// <summary>
///// 系统配置
///// </summary>
//public class SysConfig : ConfigBase<SysConfig>
//{
//    public SqlConStrHelper sqlcon = new SqlConStrHelper();
//    public SysConfig()
//    {
//        ConfigPath = "./xml/";
//        ConfigFile = "sysconfig.xml";
//    }
//    public override void SetFile(string path, string file)
//    {
//        ConfigPath = path;
//        ConfigFile = file;
//    }
//    /// <summary>
//    /// 数据库连接字符串密文
//    /// </summary>
//    public string DBConStr = "";
//    /// <summary>
//    /// 数据库连接字符串
//    /// </summary>
//    [XmlIgnoreAttribute]//明文不保存
//    public string DBConString
//    {
//        get
//        {
//            if (DBConStr != "" && sqlcon.FromMi(DBConStr))
//            {
//                return sqlcon.ConStrMing;
//            }
//            return "";
//        }
//        set
//        {
//            sqlcon.FromMing(value);
//            DBConStr = sqlcon.ConStrMi;
//        }
//    }
//}
//class Program
//{
//    static void Main(string[] args)
//    {
//        SysConfig sys = new SysConfig();
//        if (!SysConfig.ReadCreat(ref sys))
//        {
//            Console.WriteLine("文件夹没有写入权限。");
//            return;
//        }
//        Console.WriteLine("连接字符串：" + sys.DBConString);
//        SqlConStrHelper sqlcon = new SqlConStrHelper();
//        sqlcon.FromTestBox("\"sss\"", "aaa", "ttttt", "rrrrrrr");
//        string str = DESHelper.EncryptStr(sqlcon.ConStrMing);
//        string s1 = str;
//        sys.DBConString = sqlcon.ConStrMing;
//        SysConfig.Save(sys);
//        Console.ReadLine();
//    }
//}
//#endregion