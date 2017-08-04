using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Timers;
using System.Windows.Forms;

namespace LookMyNet
{
    public partial class LookNetBar : Form
    {
        #region 初始化
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Timer Timepiece;
        private System.Windows.Forms.TextBox textBox_Down;
        private System.Windows.Forms.TextBox textBox_Up;
        private System.Windows.Forms.ContextMenuStrip RightClickMenu;
        private System.Windows.Forms.ToolStripMenuItem CloseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MinToTaskbarToolStripMenuItem;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Timepiece = new System.Windows.Forms.Timer(this.components);
            this.textBox_Down = new System.Windows.Forms.TextBox();
            this.textBox_Up = new System.Windows.Forms.TextBox();
            this.RightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MinToTaskbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.RightClickMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Timepiece
            // 
            this.Timepiece.Tick += new System.EventHandler(this.Timepiece_Tick);
            // 
            // textBox_Down
            // 
            this.textBox_Down.BackColor = System.Drawing.Color.PaleTurquoise;
            this.textBox_Down.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_Down.Enabled = false;
            this.textBox_Down.Location = new System.Drawing.Point(40, 14);
            this.textBox_Down.Name = "textBox_Down";
            this.textBox_Down.Size = new System.Drawing.Size(66, 14);
            this.textBox_Down.TabIndex = 1;
            this.textBox_Down.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LookNetBar_MouseDown);
            this.textBox_Down.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LookNetBar_MouseMove);
            // 
            // textBox_Up
            // 
            this.textBox_Up.BackColor = System.Drawing.Color.PaleTurquoise;
            this.textBox_Up.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_Up.Enabled = false;
            this.textBox_Up.Location = new System.Drawing.Point(145, 14);
            this.textBox_Up.Name = "textBox_Up";
            this.textBox_Up.Size = new System.Drawing.Size(66, 14);
            this.textBox_Up.TabIndex = 2;
            this.textBox_Up.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LookNetBar_MouseDown);
            this.textBox_Up.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LookNetBar_MouseMove);
            // 
            // RightClickMenu
            // 
            this.RightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CloseToolStripMenuItem,
            this.MinToTaskbarToolStripMenuItem});
            this.RightClickMenu.Name = "RightClickMenu";
            this.RightClickMenu.Size = new System.Drawing.Size(149, 48);
            // 
            // CloseToolStripMenuItem
            // 
            this.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
            this.CloseToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.CloseToolStripMenuItem.Text = "关闭";
            this.CloseToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // MinToTaskbarToolStripMenuItem
            // 
            this.MinToTaskbarToolStripMenuItem.Name = "MinToTaskbarToolStripMenuItem";
            this.MinToTaskbarToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.MinToTaskbarToolStripMenuItem.Text = "缩小到任务栏";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "下行";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(110, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "上行";
            // 
            // LookNetBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleTurquoise;
            this.ClientSize = new System.Drawing.Size(223, 33);
            this.ContextMenuStrip = this.RightClickMenu;
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_Up);
            this.Controls.Add(this.textBox_Down);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LookNetBar";
            this.Opacity = 0.8D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "LookNetBar";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.LookNetBar_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LookNetBar_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LookNetBar_MouseMove);
            this.RightClickMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        public LookNetBar()
        {
            InitializeComponent();
        }

        private MyNetWorkMatchClass[] m_MNWMadapters;
        private MyNetWorkMonitor monitor;
        private int adapterIndex;
        private void LookNetBar_Load(object sender, EventArgs e)
        {
            //默认位置在屏幕右下角
            this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height;

            monitor = new MyNetWorkMonitor();
            //获得控制器MyNetWorkMonitor上所有计算机的适配器列表
            m_MNWMadapters = monitor.Adapters;
            if (m_MNWMadapters.Length == 0)
            {
                textBox_Down.Text = "没找到网卡";
                return;
            }

            //选择正在使用的网卡
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    adapterIndex = -1;
                    foreach (var item in m_MNWMadapters)
                    {
                        adapterIndex += 1;
                        if (item.strMatchName == ni.Description)
                        {
                            //控制该适配器开始工作
                            monitor.StartMonitoring(item);
                            //计时开始
                            this.Timepiece.Start();
                            break;
                        }
                    }
                }
            }
        }

        //选中其中一个适配器
        private void AdepterList_SelectedIndexChanged(object sender, EventArgs e)
        {
            monitor.StopMonitoring();
        }

        //计时开始,用于每秒钟改变显示速度
        private void Timepiece_Tick(object sender, EventArgs e)
        {
            //该适配器
            MyNetWorkMatchClass adapter = m_MNWMadapters[adapterIndex];
            //得到该适配器的下载速度
            textBox_Down.Text = adapter.DownloadSpeedKbps;
            //得到该适配器的上传速度
            textBox_Up.Text = adapter.UploadSpeedKbps;
        }

        #region 窗体内可拖动
        private Point mPoint = new Point();

        private void LookNetBar_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void LookNetBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }
        #endregion



        #region 右键菜单
        //关闭
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
        //缩小到任务栏

        #endregion
    }
    public class MyNetWorkMonitor
    {
        // 计时器事件执行每秒钟刷新值在适配器。
        private System.Timers.Timer Monitor_Timer;
        //该计算机的适配器列表。
        private ArrayList m_AdaptersList;
        //目前控制的适配器列表
        private ArrayList m_MonitoredAdapters;

        public MyNetWorkMonitor()
        {
            //用来保存获取到的计算机的适配器列表
            m_AdaptersList = new ArrayList();
            //运行的有效的适配器列表
            m_MonitoredAdapters = new ArrayList();

            //列举出安装在该计算机上面的适配器
            ShowNetAdapter();
            Monitor_Timer = new System.Timers.Timer(1000);
            Monitor_Timer.Elapsed += new ElapsedEventHandler(timer_ElapsedClick);
        }

        //用于每秒钟刷新速度
        private void timer_ElapsedClick(object sender, ElapsedEventArgs e)
        {
            //每秒钟遍历有效的网络适配器
            foreach (MyNetWorkMatchClass adapter in m_MonitoredAdapters)
            {
                //刷新上传下载速度
                adapter.CaculateAndRefresh();
            }

        }

        //列举出安装在该计算机上面的适配器方法
        private void ShowNetAdapter()
        {
            PerformanceCounterCategory PCCCategory = new PerformanceCounterCategory("Network Interface");
            foreach (string InstanceName in PCCCategory.GetInstanceNames())
            {
                if (InstanceName == "MS TCP Loopback interface")
                    continue;
                // 创建一个实例Net workAdapter类别，并创建性能计数器。
                MyNetWorkMatchClass myMNWMadapter = new MyNetWorkMatchClass(InstanceName);
                myMNWMadapter.m_Performance_Down = new PerformanceCounter("Network Interface", "Bytes Received/sec", InstanceName);
                myMNWMadapter.m_Performance_Up = new PerformanceCounter("Network Interface", "Bytes Sent/sec", InstanceName);
                m_AdaptersList.Add(myMNWMadapter);
            }
        }



        public void StartMonitoring()
        {
            if (m_AdaptersList.Count > 0)
            {
                foreach (MyNetWorkMatchClass myMNWMadapter in m_AdaptersList)
                    if (!m_MonitoredAdapters.Contains(myMNWMadapter))
                    {
                        m_MonitoredAdapters.Add(myMNWMadapter);
                        myMNWMadapter.Start();
                    }

                Monitor_Timer.Enabled = true;
            }
        }

        //控制该适配器开始工作
        public void StartMonitoring(MyNetWorkMatchClass myMNWMadapter)
        {
            if (!m_MonitoredAdapters.Contains(myMNWMadapter))
            {
                m_MonitoredAdapters.Add(myMNWMadapter);
                //该适配器调用自己函数开始工作
                myMNWMadapter.Start();
            }
            Monitor_Timer.Enabled = true;
        }

        public void StopMonitoring()
        {
            m_MonitoredAdapters.Clear();
            Monitor_Timer.Enabled = false;
        }

        public void StopMonitoring(MyNetWorkMatchClass adapter)
        {
            if (m_MonitoredAdapters.Contains(adapter))
                m_MonitoredAdapters.Remove(adapter);
            if (m_MonitoredAdapters.Count == 0)
                Monitor_Timer.Enabled = false;
        }

        //该控制类所控制的找出所有适配器的适配器列表
        public MyNetWorkMatchClass[] Adapters
        {
            get
            {
                return (MyNetWorkMatchClass[])m_AdaptersList.ToArray(typeof(MyNetWorkMatchClass));
            }
        }
    }

    //一个安装在计算机上的网络适配器，该类可用于获取网络中的流量
    public class MyNetWorkMatchClass
    {
        public override string ToString()
        {
            return m_strMatchName;
        }
        public string strMatchName
        {
            get
            {
                return m_strMatchName;
            }
        }
        public long DownloadSpeed
        {
            get
            {
                return m_lDownLoadSpeed;
            }
        }
        public long UploadSpeed
        {
            get
            {
                return m_lUpLoadSpeed;
            }
        }
        public string DownloadSpeedKbps
        {
            get
            {
                if (m_lDownLoadSpeed > 1024 * 1024)
                {
                    return String.Format("{0:n} mbps", this.m_lDownLoadSpeed / 1024.0 / 1024.0);
                }
                else
                {
                    return String.Format("{0:n} kbps", this.m_lDownLoadSpeed / 1024.0);
                }
            }
        }

        public string UploadSpeedKbps
        {
            get
            {
                if (m_lUpLoadSpeed > 1024 * 1024)
                {
                    return String.Format("{0:n} mbps", this.m_lUpLoadSpeed / 1024.0 / 1024.0);
                }
                else
                {
                    return String.Format("{0:n} kbps", this.m_lUpLoadSpeed / 1024.0);
                }
            }
        }
        //当前的下载速度,字节计算
        private long m_lDownLoadNetValues1;
        //当前的上传速度
        private long m_lUploadNetValues1;
        //一秒前的下载速度,字节计算
        private long m_lDownLoadNetValues2;
        //一秒前的上传速度
        private long m_lUploadNetValues2;

        //此适配器的名字
        private string m_strMatchName;
        //控制下载速度的流量计算中心
        internal PerformanceCounter m_Performance_Down;
        // //控制上传速度的流量计算中心
        internal PerformanceCounter m_Performance_Up;

        //每秒钟下载速度
        private long m_lDownLoadSpeed;
        //每秒钟上传速度
        private long m_lUpLoadSpeed;
        public MyNetWorkMatchClass(string strComputerNetName)
        {
            m_lDownLoadNetValues1 = 0;
            m_strMatchName = strComputerNetName;
            m_lUploadNetValues1 = 0;
            m_lDownLoadNetValues2 = 0;
            m_lUploadNetValues2 = 0;

        }
        //该适配器准备控制的方法函数
        public void Start()
        {
            m_lUploadNetValues1 = m_Performance_Up.NextSample().RawValue;
            m_lDownLoadNetValues1 = m_Performance_Down.NextSample().RawValue;

        }

        //计算速度
        public void CaculateAndRefresh()
        {
            m_lDownLoadNetValues2 = m_Performance_Down.NextSample().RawValue;
            m_lUploadNetValues2 = m_Performance_Up.NextSample().RawValue;

            m_lDownLoadSpeed = m_lDownLoadNetValues2 - m_lDownLoadNetValues1;
            m_lUpLoadSpeed = m_lUploadNetValues2 - m_lUploadNetValues1;

            m_lDownLoadNetValues1 = m_lDownLoadNetValues2;
            m_lUploadNetValues1 = m_lUploadNetValues2;
        }

    }
}
