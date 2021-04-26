using System;
using System.Collections.Generic;
using System.Text;


using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using System.Runtime.Serialization;

namespace 调试者
{
    /// <summary>
    /// 配置文件的读写操作
    /// </summary>
    public class XMLHelper
    {
        #region 文件操作
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="filepath">文件路径，如果为空，就是当前路径</param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        static string SaveFile(string filepath, string fileName)
        {
            string temp = "";
            if (filepath != "")//路径不为空
            {
                if (!Directory.Exists(filepath))//找到这个路径
                {
                    try
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    catch (System.Exception ex)
                    {
                        throw new Exception("路径不可写入，请重新选择路径。" + filepath);
                    }
                }
                //创建完毕，添加一个斜杠
                if (filepath[filepath.Length-1] != '\\')
                {
                    filepath +='\\';
                }
            }
            else//路径为空
            {
                filepath = System.AppDomain.CurrentDomain.BaseDirectory + "\\";
            }
            //路径处理完毕，
            if (!File.Exists(filepath + fileName))
            {
                try
                {
                    using(File.Create(filepath + fileName));
                    temp = filepath + fileName;
                }
                catch (System.Exception ex)
                {
                    throw new Exception("文件创建失败。路径：" + temp + " 详细信息：" + ex.Message.ToString());
                }
            }
            else
            {
                temp = filepath + fileName;//如果已经存在
            }
            return temp;
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filepath">文件路径，如果为空，就是当前路径</param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        static string ReadFile(string filepath, string fileName)
        {
            string temp = "";
            if (filepath != "" && File.Exists(filepath + "\\" + fileName))
            {
                temp = filepath + "\\" + fileName;
            }
            if (filepath == "" && File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName))
            {
                temp = System.AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName;
            }
            return temp;
        }
        #endregion

        #region 保存成文件
        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="obj">需要保存的对象，必须可序列化</param>
        /// <param name="filepath">文件路径</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="isXml">是否xml，true：xml，false：二进制bin</param>
        /// <param name="isYaSuo">是否压缩，true：压缩，false：不压缩</param>
        /// <returns></returns>
        public static bool SaveObject2File(object obj,string filepath, string fileName, bool isXml, bool isYaSuo)
        {
            if (obj==null)
            {
                throw new Exception("保存文件出错，不能保存NULL类型的对象,请先实例化对象再保存。");
            }
            string filepathfile = "";
            try
            {
                filepathfile = SaveFile(filepath, fileName);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            
            Type t = obj.GetType();
            try
            {
                #region 运算
                using (MemoryStream ms = new MemoryStream())//内存流
                {
                    if (isXml)
                    {
                        XmlSerializer xs = new XmlSerializer(t);
                        xs.Serialize(ms, obj);//序列化到内存流中
                    }
                    else
                    {
                        IFormatter formatter = new BinaryFormatter();//定义BinaryFormatter以序列化object对象
                        formatter.Serialize(ms, obj);//序列化到内存流中
                    }
                    //-------下面存储
                    using (Stream stream = new FileStream(filepathfile, FileMode.Create, FileAccess.Write, FileShare.Read))//文件流
                    {
                        if (isYaSuo)
                        {
                            using (GZipStream gzipStream = new GZipStream(stream, CompressionMode.Compress, true))//压缩流
                            {
                                gzipStream.Write(ms.ToArray(), 0, (Int32)ms.Length);//把压缩后的数据写入文件流
                                gzipStream.Close();
                            }
                        }
                        else
                        {
                            stream.Write(ms.ToArray(), 0, (Int32)ms.Length);
                            stream.Close();
                        }
                        stream.Close();
                    }
                    ms.Close();
                }
                #endregion
                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 从文件里反序列化出对象
        /// </summary>
        /// <param name="objType">对象的类型</param>
        /// <param name="filepath">文件路径</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="isXml">true：被读的文件是xml。flase：被读的文件是bin</param>
        /// <param name="isYaSuo">true：文件被压缩过。flase：没有压缩</param>
        /// <returns></returns>
        public static object ReadFile2Object(Type type, string filepath, string fileName, bool isXml, bool isYaSuo)
        {
            object obj = null;
            if (type == null)
            {
                throw new Exception("读取配置文件前，请先对配置对象obj进行初始化。");
            }
            string filepathfile = ReadFile(filepath, fileName);
            if (filepathfile == "")
            {
                throw new Exception("找不到配置文件，请先配置。");
            }
            
            using (MemoryStream ms = new MemoryStream())//定义内存流
            {
                //从文件读取数据到内存中
                using (Stream stream = new FileStream(filepathfile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] buffer = new byte[4096];//定义数据缓冲
                    int offset = 0;//定义读取位置
                    if (isYaSuo)
                    {
                        using (GZipStream gzipStream = new GZipStream(stream, CompressionMode.Decompress))//创建解压对象
                        {
                            while ((offset = gzipStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                ms.Write(buffer, 0, offset);//解压后的数据写入内存流
                            }
                            gzipStream.Close();
                        }
                    }
                    else
                    {
                        while ((offset = stream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            ms.Write(buffer, 0, offset);//解压后的数据写入内存流
                        }
                    }
                    stream.Close();
                }
                //读取完毕
                ms.Position = 0;//设置内存流的位置
                //开始解析
                try
                {
                    if (isXml)
                    {
                        XmlSerializer xs = new XmlSerializer(type);
                        obj = xs.Deserialize(ms);
                    }
                    else
                    {
                        BinaryFormatter sfFormatter = new BinaryFormatter();
                        obj = (object)sfFormatter.Deserialize(ms);//反序列化
                    }                    
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ms.Close();//一定要关闭掉
                }
            }
            return obj;
        }

        #endregion

        #region 保存成数组，用于发送
        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="obj">需要保存的对象，必须可序列化</param>
        /// <param name="fileName">文件路径</param>
        /// <param name="isXml">是否xml，true：xml，false：二进制bin</param>
        /// <param name="isYaSuo">是否压缩，true：压缩，false：不压缩</param>
        /// <returns></returns>
        public static byte[] SaveObject2Byte(object obj, bool isXml, bool isYaSuo)
        {
            if (obj == null)
            {
                throw new Exception("保存文件出错，不能保存NULL类型的对象,请先实例化对象再保存。");
            }
            byte[] bts = null;//存储解析后的数组
            Type t = obj.GetType();
            try
            {
                using (MemoryStream ms = new MemoryStream())//内存流
                {
                    if (isXml)
                    {
                        XmlSerializer xs = new XmlSerializer(t);
                        xs.Serialize(ms, obj);//序列化到内存流中                        
                    }
                    else
                    {
                        IFormatter formatter = new BinaryFormatter();//定义BinaryFormatter以序列化object对象
                        formatter.Serialize(ms, obj);//序列化到内存流中
                    }
                    //-------下面存储
                    if (isYaSuo)
                    {
                        using (MemoryStream stream = new MemoryStream())//临时内存流
                        {
                            using (GZipStream gzipStream = new GZipStream(stream, CompressionMode.Compress, true))//压缩流
                            {
                                gzipStream.Write(ms.ToArray(), 0, (Int32)ms.Length);//把压缩后的数据写入文件流
                                gzipStream.Close();
                            }
                            bts = stream.ToArray();
                            stream.Close();
                        }
                    }
                    else
                    {
                        bts = ms.ToArray();//不压缩
                    }
                    ms.Close();
                }
            }
            catch (System.Exception ex)
            {

            }

            return bts;
        }

        /// <summary>
        /// 从文件里反序列化出对象
        /// </summary>
        /// <param name="objType">对象的类型</param>
        /// <param name="fileName">文件名</param>
        /// <param name="isXml">true：被读的文件是xml。flase：被读的文件是bin</param>
        /// <param name="isYaSuo">true：文件被压缩过。flase：没有压缩</param>
        /// <returns></returns>
        public static object ReadByte2Object(byte[] byts, Type objType, bool isXml, bool isYaSuo)
        {
            Object obj = null;
            try
            {
                using (MemoryStream ms = new MemoryStream())//定义内存流
                {
                    using (MemoryStream stream = new MemoryStream(byts))
                    {
                        byte[] buffer = new byte[4096];//定义数据缓冲
                        int offset = 0;//定义读取位置
                        if (isYaSuo)
                        {
                            using (GZipStream gzipStream = new GZipStream(stream, CompressionMode.Decompress))//创建解压对象
                            {
                                while ((offset = gzipStream.Read(buffer, 0, buffer.Length)) != 0)
                                {
                                    ms.Write(buffer, 0, offset);//解压后的数据写入内存流
                                }
                                gzipStream.Close();
                            }
                        }
                        else
                        {
                            while ((offset = stream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                ms.Write(buffer, 0, offset);//解压后的数据写入内存流
                            }
                        }
                        stream.Close();
                    }
                    ms.Position = 0;//设置内存流的位置
                    if (isXml)
                    {
                        XmlSerializer xs = new XmlSerializer(objType);
                        obj = xs.Deserialize(ms);
                    }
                    else
                    {
                        BinaryFormatter sfFormatter = new BinaryFormatter();
                        obj = (object)sfFormatter.Deserialize(ms);//反序列化
                    }
                    ms.Close();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return obj;
        }
        #endregion
    }
}
