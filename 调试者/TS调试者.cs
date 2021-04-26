#region 驱动
using System;
using System.Drawing;
using System.Windows.Forms;
using Lz.串口;
using Sunisoft.IrisSkin;
using System.IO;
using Lz.算法;
using System.Threading;
using System.Collections.Generic;
using System.Text;
#endregion

namespace 调试者
{
    public partial class TS调试者 : Form
    {
        string fileconfig = "ChuanKouTiaoShiZhe.txt";
        int DefX = 1507;
        int tmpDefX = 0;

        #region 变量
        COM com = new COM();
        #endregion

        #region 重要的函数
        private void comdk打开()
        {
            try
            {
                com.PortName = commz名字.Text;
                com.BaudRate = Convert.ToInt32(combt波特率.Text);
                com.Parity = COM.StringToParity(comjy校验位.Text);
                com.DataBits = Convert.ToInt16(comsj数据位.Text);
                com.StopBits = COM.StingToStopBits(comtz停止位.Text);
                com.Open();
                if (com.IsOpen)
                {
                    but打开.Text = "关闭";
                    labzt状态.BackColor = Color.Red;
                    labckx串口信息();
                    throw new Exception("串口打开成功！");
                }
                else
                {
                    throw new Exception("串口打开失败！请检查是否存在此串口。");
                }
            }
            catch (Exception ex)
            {
                fill状态条2(ex.Message);
            }
        }
        private void comgb关闭()
        {
            try
            {
                if (com.IsOpen)
                {
                    com.Close();
                    if (!com.IsOpen)
                    {
                        but打开.Text = "打开";
                        labzt状态.BackColor = Color.Blue;
                        throw new Exception("串口关闭成功！");
                    }
                    else
                    {
                        throw new Exception("串口关闭失败！");
                    }
                }
                else
                {
                    //串口已经被关闭了。我的笔记本上用的是usb--com转换器，经常需要重新插一下，松动时就非法关闭了，会出现这种情况。
                    //throw new Exception("不会出现这种情况。");

                    but打开.Text = "打开";
                    labzt状态.BackColor = Color.Blue;
                    throw new Exception("串口信息出错，请检查是否存在这个串口。");
                }
            }
            catch (Exception ex)
            {
                fill状态条2(ex.Message);
            }
        }
        private void comcx重新打开()
        {
            comgb关闭();
            comdk打开();
        }
        private void fs发送(string _fs)
        {
            if (_fs.Length > 0)
            {
                fs发送(TiaoShiQi.StringToBytes(_fs, checkfs16进制发送.Checked));
            }
        }
        private void fs发送(byte[] _fs)
        {
            if (_fs.Length > 0)
            {
                try
                {
                    if (checkBox3.Checked)
                    {
                        byte[] tmp = new byte[_fs.Length + 1];
                        Array.Copy(_fs, tmp, _fs.Length);
                        tmp[tmp.Length - 1] = 0x0d;
                        _fs = tmp;
                    }
                    if (checkBox4.Checked)
                    {
                        _fs = CRC16.YTAddCRC(_fs);
                    }
                    com.Write(_fs, 0, _fs.Length);
                    TiaoShiQi.sjcl数据处理(this, richfsq发送区, _fs, checkfs16xs发送区16进制显示.Checked, checkfszfxs发送区字符显示.Checked, checkfsxhxs发送区新行显示.Checked);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
            else
            {
                throw new Exception("发送失败，你没有填写任何数据。");
            }
        }
        private void com接收(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                //Thread.Sleep(50);
                byte[] bt = new byte[com.BytesToRead];
                com.Read(bt, 0, com.BytesToRead);
                if (bt.Length > 0)
                {
                    TiaoShiQi.sjcl数据处理(this, richjsq接收区, bt, checkjs16xs接收区16进制显示.Checked, checkjszfxs接收区字符显示.Checked, checkjsxhxs接收区新行显示.Checked);                    
                }
            }
            catch (Exception ex)
            {
                fill状态条2(ex.Message.ToString());
            }
        }
        #endregion

        #region 系统生成
        
        PortDataXml da = new PortDataXml();
        private void TS调试者_Load(object sender, EventArgs e)
        {
            tbfs1.TextChanged -= textBox2_TextChanged;
            tbfs2.TextChanged -= textBox2_TextChanged;
            tbfs3.TextChanged -= textBox2_TextChanged;
            tbfs4.TextChanged -= textBox2_TextChanged;
            tbfs5.TextChanged -= textBox2_TextChanged;
            tbfs6.TextChanged -= textBox2_TextChanged;
            tbfs7.TextChanged -= textBox2_TextChanged;
            tbfs8.TextChanged -= textBox2_TextChanged;
            tbfs9.TextChanged -= textBox2_TextChanged;
            tbfs10.TextChanged -= textBox2_TextChanged;

            tbJg1.TextChanged -= textBox2_TextChanged;
            tbJg2.TextChanged -= textBox2_TextChanged;
            tbJg3.TextChanged -= textBox2_TextChanged;
            tbJg4.TextChanged -= textBox2_TextChanged;
            tbJg5.TextChanged -= textBox2_TextChanged;
            tbJg6.TextChanged -= textBox2_TextChanged;
            tbJg7.TextChanged -= textBox2_TextChanged;
            tbJg8.TextChanged -= textBox2_TextChanged;
            tbJg9.TextChanged -= textBox2_TextChanged;
            tbJg10.TextChanged -= textBox2_TextChanged;
            tmpDefX = Width;
            //da.SetFile("", fileconfig);
            PortDataXml.ReadCreat(ref da);
            tbfs1.Text = da.Str1;
            tbfs2.Text = da.Str2;
            tbfs3.Text = da.Str3;
            tbfs4.Text = da.Str4;
            tbfs5.Text = da.Str5;
            tbfs6.Text = da.Str6;
            tbfs7.Text = da.Str7;
            tbfs8.Text = da.Str8;
            tbfs9.Text = da.Str9;
            tbfs10.Text = da.Str10;

            tbJg1.Text = da.Strjg1;
            tbJg2.Text = da.Strjg2;
            tbJg3.Text = da.Strjg3;
            tbJg4.Text = da.Strjg4;
            tbJg5.Text = da.Strjg5;
            tbJg6.Text = da.Strjg6;
            tbJg7.Text = da.Strjg7;
            tbJg8.Text = da.Strjg8;
            tbJg9.Text = da.Strjg9;
            tbJg10.Text = da.Strjg10;

            tbfs1.TextChanged += textBox2_TextChanged;
            tbfs2.TextChanged += textBox2_TextChanged;
            tbfs3.TextChanged += textBox2_TextChanged;
            tbfs4.TextChanged += textBox2_TextChanged;
            tbfs5.TextChanged += textBox2_TextChanged;
            tbfs6.TextChanged += textBox2_TextChanged;
            tbfs7.TextChanged += textBox2_TextChanged;
            tbfs8.TextChanged += textBox2_TextChanged;
            tbfs9.TextChanged += textBox2_TextChanged;
            tbfs10.TextChanged += textBox2_TextChanged;

            tbJg1.TextChanged += textBox2_TextChanged;
            tbJg2.TextChanged += textBox2_TextChanged;
            tbJg3.TextChanged += textBox2_TextChanged;
            tbJg4.TextChanged += textBox2_TextChanged;
            tbJg5.TextChanged += textBox2_TextChanged;
            tbJg6.TextChanged += textBox2_TextChanged;
            tbJg7.TextChanged += textBox2_TextChanged;
            tbJg8.TextChanged += textBox2_TextChanged;
            tbJg9.TextChanged += textBox2_TextChanged;
            tbJg10.TextChanged += textBox2_TextChanged;

            com.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(com接收);//串口接到数据会激发这个事件
            commz名字.Items.AddRange(COM.AllCom());
            commz名字.SelectedIndex = 0;
            foreach (int i in COM.AllBaudRate())
            {
                combt波特率.Items.Add(i);
            }
            combt波特率.SelectedIndex = 6;
            foreach (string s in COM.AllParity())
            {
                comjy校验位.Items.Add(s);
            }
            comjy校验位.SelectedIndex = 0;
            foreach (int i in COM.AllDataBits())
            {
                comsj数据位.Items.Add(i);
            }
            comsj数据位.SelectedIndex = 2;
            foreach (string s in COM.AllStopBits())
            {
                comtz停止位.Items.Add(s);
            }
            comtz停止位.SelectedIndex = 0;
            duqu();//读取皮肤文件
        }
        private void but打开_Click(object sender, EventArgs e)//打开按钮
        {
            try
            {
                if (but打开.Text == "打开")
                {
                    comdk打开();
                }
                else
                {
                    comgb关闭();
                }
            }
            catch (Exception ex)
            {
                fill状态条2(ex.Message.ToString());
            }
        }
        private void butfs发送_Click(object sender, EventArgs e)//发送按钮
        {
            try
            {
                if (com.IsOpen)
                {
                    try
                    {
                        fs发送(richfs发送.Text.ToString().Trim()+"\r\n");
                        throw new Exception("发送成功。");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message.ToString());
                    }
                }
                else
                {
                    throw new Exception("串口没有打开。");
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel2.Text = ex.Message.ToString();
            }
        }
        private void butjs接收区清空_Click(object sender, EventArgs e)//发送区清空
        {
            richjsq接收区.Text = "";
        }
        private void butfs发送区清空_Click(object sender, EventArgs e)//发送区清空
        {
            richfsq发送区.Text = "";
        }
        private void butfsq发送清空_Click(object sender, EventArgs e)//发送清空
        {
            richfs发送.Text = "";
        }
        private void timer1_Tick(object sender, EventArgs e)//自动发送时间控制
        {
            try
            {
                if (com.IsOpen)
                {
                    fs发送(richfs发送.Text.ToString());
                    throw new Exception("发送成功。");
                }
                else
                {
                    throw new Exception("串口没有打开。");
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel2.Text = ex.Message.ToString();
            }
        }
        private void checkzd自动发送_CheckedChanged(object sender, EventArgs e)//自动发送选项
        {
            if (checkzd自动发送.Checked)
            {
                checkBox1.Checked = false;
                timer1.Interval = Convert.ToInt16(textZq周期.Text.ToString());
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
            }
        }
        private void commz名字_SelectedIndexChanged(object sender, EventArgs e)//重新设置串口
        {
            comcx重新打开();
        }
        #endregion

        #region 换皮肤
        private SkinEngine skinEngine1 = new SkinEngine();
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            skinEngine1.SkinFile = comboBox1.Text.ToString();
        }
        public void duqu()
        {
            string[] a = Directory.GetFiles(System.Windows.Forms.Application.StartupPath+"\\pi");
            foreach (string file in a)
            {
                comboBox1.Items.Add(file);
            }
        }
        #endregion

        #region 无用
        public TS调试者()
        {
            InitializeComponent();
        }
        private void TS调试者_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (com.IsOpen)
                {
                    com.Close();
                }
            }
            catch (Exception ex)
            {
                fill状态条2(ex.Message);
            }
        }
        private void fill状态条2(string s)
        {
            toolStripStatusLabel2.Text = s.ToString();
        }
        private void labckx串口信息()
        {
            textBox1.Text = com.PortName.ToString() + "," + com.BaudRate.ToString() + "," + COM.ParityToString(com.Parity) + "," + com.DataBits.ToString() + "," +COM.StopBitsToSting(com.StopBits);
        }
        //Icon i1 = Resource1.I1;
        //Icon i2 = Resource1.I2;
        private void timer2_Tick(object sender, EventArgs e)
        {
            //if (Icon==i1)
            //{
            //    Icon =i2;
            //}
            //else
            //{
            //    Icon = i1;
            //}
        }
        private void 复制_Click(object sender, EventArgs e)
        {
            this.contextMenuStrip1.SourceControl.Select();//先获取焦点，防止点两下才运行
            RichTextBox rtb = (RichTextBox)this.contextMenuStrip1.SourceControl;
            rtb.Copy();
        }
        private void 粘贴_Click(object sender, EventArgs e)
        {
            this.contextMenuStrip1.SourceControl.Select();
            RichTextBox rtb = (RichTextBox)this.contextMenuStrip1.SourceControl;
            rtb.Paste();
        }
        private void 剪切_Click(object sender, EventArgs e)
        {
            this.contextMenuStrip1.SourceControl.Select();
            RichTextBox rtb = (RichTextBox)this.contextMenuStrip1.SourceControl;
            rtb.Cut();
        }
        private void 删除_Click(object sender, EventArgs e)
        {
            this.contextMenuStrip1.SourceControl.Select();
            RichTextBox rtb = (RichTextBox)this.contextMenuStrip1.SourceControl;
            rtb.SelectedText = "";
        }
        private void 全选_Click(object sender, EventArgs e)
        {
            this.contextMenuStrip1.SourceControl.Select();
            RichTextBox rtb = (RichTextBox)this.contextMenuStrip1.SourceControl;
            rtb.SelectAll();
        }
        private void 撤销_Click(object sender, EventArgs e)
        {
            this.contextMenuStrip1.SourceControl.Select();
            RichTextBox rtb = (RichTextBox)this.contextMenuStrip1.SourceControl;
            rtb.Undo();
        }
        #endregion

        int num = 0;
        private void toolStripStatusLabel4_Click(object sender, EventArgs e)
        {
            num += 1;
            if (num > 10)
            {
                关于 guan = new 关于();
                guan.ShowDialog();
                num = 0;
            }
        }

        //自动模式
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkzd自动发送.Checked = false;
                tmpDefX = Width;
                Width = 1418;
            }
            else
            {
                Width = tmpDefX;
                checkBox2.Checked = false;//取消自动发送模式
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                getfsls();
                if (jgls.Count==0)
                {
                    return;
                }
                curIdx = 0;
                timerZiDongFS.Interval = Convert.ToInt16(textZq周期.Text.ToString());
                timerZiDongFS.Enabled = true;
            }
            else
            {
                timerZiDongFS.Enabled = false;
            }
        }

        List<int> jgls = new List<int>();
        List<byte[]> nrls = new List<byte[]>();
        void getfsls()
        {
            jgls.Clear();
            nrls.Clear();
            getshujv(tbJg1, tbfs1);
            getshujv(tbJg2, tbfs2);
            getshujv(tbJg3, tbfs3);
            getshujv(tbJg4, tbfs4);
            getshujv(tbJg5, tbfs5);
            getshujv(tbJg6, tbfs6);
            getshujv(tbJg7, tbfs7);
            getshujv(tbJg8, tbfs8);
            getshujv(tbJg9, tbfs9);
            getshujv(tbJg10, tbfs10);
        }
        void getshujv(TextBox tb1, TextBox tb2)
        {
            int jg = 0;
            byte[] nr = new byte[0];
            try
            {
                jg = int.Parse(tb1.Text);
                nr = Lz.算法.GeShi转换算法._16ToBtyes(tb2.Text);
                jgls.Add(jg);
                nrls.Add(nr);
            }
            catch (Exception)
            {
            }
        }
        int curIdx = 0;
        //自动发送模式
        private void timerZiDongFS_Tick(object sender, EventArgs e)
        {
            timerZiDongFS.Stop();
            {
                try
                {
                    if (com.IsOpen)
                    {
                        fs发送(nrls[curIdx]);
                        toolStripStatusLabel2.Text="发送成功。";
                    }
                    else
                    {
                        toolStripStatusLabel2.Text="串口没有打开。";
                    }
                }
                catch (Exception ex)
                {
                    toolStripStatusLabel2.Text = ex.Message.ToString();
                }
            }
            curIdx++;
            if (curIdx == jgls.Count)
            {
                curIdx = 0;
            }
            timerZiDongFS.Interval = jgls[curIdx];
            timerZiDongFS.Start();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (richfs发送.Text=="")
            {
                toolStripStatusLabel2.Text = "数据不能为空";
                return;
            }
            string s = CRC16.TDCRCWanZheng(richfs发送.Text);
            if (s == ":\r\n")
            {
                MessageBox.Show("你填写的数据不对，请检查。必须是16进制，自动添加前面：和后面CRC");
                return;
            }
            try
            {
                if (com.IsOpen)
                {
                    try
                    {
                        fs发送(s);
                        throw new Exception("发送成功。");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message.ToString());
                    }
                }
                else
                {
                    throw new Exception("串口没有打开。");
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel2.Text = ex.Message.ToString();
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            da.Str1 = tbfs1.Text;
            da.Str2 = tbfs2.Text;
            da.Str3 = tbfs3.Text;
            da.Str4 = tbfs4.Text;
            da.Str5 = tbfs5.Text;
            da.Str6 = tbfs6.Text;
            da.Str7 = tbfs7.Text;
            da.Str8 = tbfs8.Text;
            da.Str9 = tbfs9.Text;
            da.Str10 = tbfs10.Text;

            da.Strjg1 = tbJg1.Text;
            da.Strjg2 = tbJg2.Text;
            da.Strjg3 = tbJg3.Text;
            da.Strjg4 = tbJg4.Text;
            da.Strjg5 = tbJg5.Text;
            da.Strjg6 = tbJg6.Text;
            da.Strjg7 = tbJg7.Text;
            da.Strjg8 = tbJg8.Text;
            da.Strjg9 = tbJg9.Text;
            da.Strjg10 = tbJg10.Text;
            da.chuankou = "";
            PortDataXml.Save(da);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                richfs发送.Text = richfs发送.Text + CRC16.TDCRC(richfs发送.Text);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("错误，发送数据必须16进制。" + ex.Message);
            }
        }
    }
}