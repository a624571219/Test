using System;
using System.Collections.Generic;
using System.Text;

namespace Lz.算法
{
    /// <summary>
    /// 新开普CRC校验
    /// </summary>
    public class CRC16
    {
        ushort[] crctab = new ushort[256]{
                    0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50a5, 0x60c6, 0x70e7,
                    0x8108, 0x9129, 0xa14a, 0xb16b, 0xc18c, 0xd1ad, 0xe1ce, 0xf1ef,
                    0x1231, 0x0210, 0x3273, 0x2252, 0x52b5, 0x4294, 0x72f7, 0x62d6,
                    0x9339, 0x8318, 0xb37b, 0xa35a, 0xd3bd, 0xc39c, 0xf3ff, 0xe3de,
                    0x2462, 0x3443, 0x0420, 0x1401, 0x64e6, 0x74c7, 0x44a4, 0x5485,
                    0xa56a, 0xb54b, 0x8528, 0x9509, 0xe5ee, 0xf5cf, 0xc5ac, 0xd58d,
                    0x3653, 0x2672, 0x1611, 0x0630, 0x76d7, 0x66f6, 0x5695, 0x46b4,
                    0xb75b, 0xa77a, 0x9719, 0x8738, 0xf7df, 0xe7fe, 0xd79d, 0xc7bc,
                    0x48c4, 0x58e5, 0x6886, 0x78a7, 0x0840, 0x1861, 0x2802, 0x3823,
                    0xc9cc, 0xd9ed, 0xe98e, 0xf9af, 0x8948, 0x9969, 0xa90a, 0xb92b,
                    0x5af5, 0x4ad4, 0x7ab7, 0x6a96, 0x1a71, 0x0a50, 0x3a33, 0x2a12,
                    0xdbfd, 0xcbdc, 0xfbbf, 0xeb9e, 0x9b79, 0x8b58, 0xbb3b, 0xab1a,
                    0x6ca6, 0x7c87, 0x4ce4, 0x5cc5, 0x2c22, 0x3c03, 0x0c60, 0x1c41,
                    0xedae, 0xfd8f, 0xcdec, 0xddcd, 0xad2a, 0xbd0b, 0x8d68, 0x9d49,
                    0x7e97, 0x6eb6, 0x5ed5, 0x4ef4, 0x3e13, 0x2e32, 0x1e51, 0x0e70,
                    0xff9f, 0xefbe, 0xdfdd, 0xcffc, 0xbf1b, 0xaf3a, 0x9f59, 0x8f78,
                    0x9188, 0x81a9, 0xb1ca, 0xa1eb, 0xd10c, 0xc12d, 0xf14e, 0xe16f,
                    0x1080, 0x00a1, 0x30c2, 0x20e3, 0x5004, 0x4025, 0x7046, 0x6067,
                    0x83b9, 0x9398, 0xa3fb, 0xb3da, 0xc33d, 0xd31c, 0xe37f, 0xf35e,
                    0x02b1, 0x1290, 0x22f3, 0x32d2, 0x4235, 0x5214, 0x6277, 0x7256,
                    0xb5ea, 0xa5cb, 0x95a8, 0x8589, 0xf56e, 0xe54f, 0xd52c, 0xc50d,
                    0x34e2, 0x24c3, 0x14a0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405,
                    0xa7db, 0xb7fa, 0x8799, 0x97b8, 0xe75f, 0xf77e, 0xc71d, 0xd73c,
                    0x26d3, 0x36f2, 0x0691, 0x16b0, 0x6657, 0x7676, 0x4615, 0x5634,
                    0xd94c, 0xc96d, 0xf90e, 0xe92f, 0x99c8, 0x89e9, 0xb98a, 0xa9ab,
                    0x5844, 0x4865, 0x7806, 0x6827, 0x18c0, 0x08e1, 0x3882, 0x28a3,
                    0xcb7d, 0xdb5c, 0xeb3f, 0xfb1e, 0x8bf9, 0x9bd8, 0xabbb, 0xbb9a,
                    0x4a75, 0x5a54, 0x6a37, 0x7a16, 0x0af1, 0x1ad0, 0x2ab3, 0x3a92,
                    0xfd2e, 0xed0f, 0xdd6c, 0xcd4d, 0xbdaa, 0xad8b, 0x9de8, 0x8dc9,
                    0x7c26, 0x6c07, 0x5c64, 0x4c45, 0x3ca2, 0x2c83, 0x1ce0, 0x0cc1,
                    0xef1f, 0xff3e, 0xcf5d, 0xdf7c, 0xaf9b, 0xbfba, 0x8fd9, 0x9ff8,
                    0x6e17, 0x7e36, 0x4e55, 0x5e74, 0x2e93, 0x3eb2, 0x0ed1, 0x1ef0
                    };
        /// <summary>
        /// CRC校验公式
        /// </summary>
        /// <param name="crc">CRC</param>
        /// <param name="cp">发送的数据序列</param>
        /// <returns>新CRC</returns>
        ushort xcrc(ushort crc, byte cp)
        {
            ushort t1 = 0, t2 = 0, t3 = 0, t4 = 0, t5 = 0, t6 = 0;
            t1 = (ushort)(crc >> 8);
            t2 = (ushort)(t1 & 0xff);
            t3 = (ushort)(cp & 0xff);
            t4 = (ushort)(crc << 8);
            t5 = (ushort)(t2 ^ t3);
            t6 = (ushort)(crctab[t5] ^ t4);
            return t6;
        }

        /// <summary>
        /// 添加CRC校验字,输入0000，返回0000 0101,不在元数据上追加,校验全部的长度
        /// </summary>
        /// <param name="bufin">需要添加crc的数组</param>
        /// <returns>需要crc后的数组</returns>
        public byte[] AddCRC(byte[] bufin)
        {
            byte[] bts = new byte[bufin.Length + 2];
            for (int i = 0; i < bufin.Length; i++)
            {
                bts[i] = bufin[i];
            }
            byte[] bts1 = ConCRC(bufin);
            bts[bufin.Length] = bts1[0];
            bts[bufin.Length + 1] = bts1[1];
            return bts;
        }

        /// <summary>
        /// 添加CRC校验字,输入0000，返回0000 0101,不在元数据上追加,校验指定的长度，如果长度不够，就报错
        /// </summary>
        /// <param name="bufin">需要添加crc的数组</param>
        /// <param name="offset">起始地址</param>
        /// <param name="count">数量</param>
        /// <returns>需要crc后的数组</returns>
        public byte[] AddCRC(byte[] bufin, int offset, int count)
        {
            if (bufin.Length < count)
            {
                throw new Exception("需要CRC校验的数组位数不够，无法进行CRC校验。");
            }
            else
            {
                byte[] bts = new byte[count + 2];
                for (int i = offset; i < count; i++)
                {
                    bts[i] = bufin[i];
                }
                byte[] bts1 = ConCRC(bufin, offset, count);
                bts[count] = bts1[0];
                bts[count + 1] = bts1[1];
                return bts;
            }
        }

        /// <summary>
        /// 计算CRC校验字
        /// </summary>
        /// <param name="bufin">需要计算crc的数组</param>
        /// <returns>计算后的crc值</returns>
        public byte[] ConCRC(byte[] bufin)
        {
            return ConCRC(bufin, 0, bufin.Length);
        }

        /// <summary>
        /// 计算CRC校验字
        /// </summary>
        /// <param name="bufin">需要计算crc的数组</param>
        /// <param name="offset">起始地址</param>
        /// <param name="count">数量</param>
        /// <returns>计算后的crc值</returns>
        public byte[] ConCRC(byte[] bufin,int offset ,int count)
        {
            if (bufin.Length<count)
            {
                throw new Exception("需要CRC校验的数组位数不够，无法进行CRC校验。");
            }
            else
            {
                ushort crc16 = 0;
                int i;
                for (i = offset; i < offset + count; i++)
                {
                    crc16 = xcrc(crc16, bufin[i]);
                }
                byte[] bts = new byte[2];
                bts[0] = (byte)(crc16 >> 8);
                bts[1] = (byte)(crc16 & 0xff);
                return bts;
            }
        }

        public static byte[] YTCRC(byte[] data)
        {
            byte crcgao = 0xff, crcdi = 0xff;//16位的CRC寄存器
            for (int i = 0; i < data.Length; i++)
            {
                crcdi = Convert.ToByte(crcdi ^ data[i]); //与低八位进行异或
                for (int n = 0; n <= 7; n++)//八位2进制数全部进行校验
                {
                    bool bl = false;
                    if ((crcdi & 0x1) == 0x1)//移出位为1，需要与多项式异或
                        bl = true;
                    crcdi = Convert.ToByte(crcdi / 2);//低八位右移1位
                    if ((crcgao & 0x1) == 0x1)//高八位的最后一位为1，低八位的最高位应该为1
                    {
                        crcdi = Convert.ToByte(crcdi | 0x80);//低八位的最高位设置为1
                    }
                    crcgao = Convert.ToByte(crcgao / 2);//高八位右移1位
                    if (bl)//需要与多项式异或
                    {
                        crcgao = Convert.ToByte(crcgao ^ 0xa0);//与多项式异或
                        crcdi = Convert.ToByte(crcdi ^ 0x1);
                    }
                }
            }
            byte[] bt = { crcdi, crcgao };
            return bt;
        }
        public static byte[] YTAddCRC(byte[] data)
        {
            byte[] bt = YTCRC(data);
            byte[] btt=new byte[data.Length+2];
            for (int i = 0; i < data.Length;i++ )
            {
                btt[i] = data[i];
            }
            btt[data.Length] = bt[0];
            btt[data.Length + 1] = bt[1];
            return btt;
        }

        /// <summary>
        /// 输入字符串，输出CRC码,出错返回空字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TDCRC(string str)
        {
            try
            {
                byte d_lrc = 0;
                for (int i = 0; i < str.Length; i += 2)
                {
                    d_lrc = (byte)(d_lrc + Convert.ToByte(str.Substring(i, 2), 16));
                }
                if (d_lrc > 0xff)
                {
                    d_lrc = (byte)(d_lrc % 0x100);
                }
                string h_lrc = (0xff - d_lrc + 1).ToString("X").PadLeft(2, '0');
                return str + h_lrc.ToString();
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 返回“：+str+crc(str)”字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TDCRCWanZheng(string str)
        {
            return ":" + TDCRC(str) + "\r\n";
        }
        /// <summary>
        /// 校验数据的完整性，包括第一位的：和最后两位的crc
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool TDIsCRCWanZheng(string str)
        {
            if (str[0] == ':' &&
                str[str.Length - 1] == '\n' &&
                str[str.Length - 2] == '\r' &&
                str == TDCRCWanZheng(str.Substring(1, str.Length - 5))
                )
            {
                return true;
            }
            return false;
        }


    }
}
