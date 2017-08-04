using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace LookMyNet
{
    public partial class LookNetBar : Form
    {
        public LookNetBar()
        {
            InitializeComponent();
        }
        private MyNetWorkMatchClass[] m_MNWMadapters;
        private MyNetWorkMonitor monitor;
        private void LookNetBar_Load(object sender, EventArgs e)
        {
            monitor = new MyNetWorkMonitor();
            //获得控制器MyNetWorkMonitor上所有计算机的适配器列表
            m_MNWMadapters = monitor.Adapters;
            if (m_MNWMadapters.Length == 0)
            {
                AdapterList.Enabled = false;
                MessageBox.Show("在计算机上没有找到网络适配器");
                return;
            }

            AdapterList.Items.AddRange(m_MNWMadapters);
        }

        //选中其中一个适配器
        private void AdepterList_SelectedIndexChanged(object sender, EventArgs e)
        {
            monitor.StopMonitoring();
            //控制该适配器开始工作
            monitor.StartMonitoring(m_MNWMadapters[AdapterList.SelectedIndex]);
            //计时开始
            this.Timepiece.Start();
        }

        //计时开始,用于每秒钟改变显示速度
        private void Timepiece_Tick(object sender, EventArgs e)
        {
            //该适配器
            MyNetWorkMatchClass adapter = m_MNWMadapters[AdapterList.SelectedIndex];
            //得到该适配器的下载速度
            textBox_Down.Text = String.Format("{0:n} kbps", adapter.DownloadSpeedKbps);
            //得到该适配器的上传速度
            textBox_Up.Text = String.Format("{0:n} kbps", adapter.UploadSpeedKbps);
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
        
        #endregion

        #region 缩小到状态栏
        
        #endregion
    }
    public class MyNetWorkMonitor
    {
        private System.Timers.Timer Monitor_Timer;              // 计时器事件执行每秒钟刷新值在适配器。
        private ArrayList m_AdaptersList;        //该计算机的适配器列表。
        private ArrayList m_MonitoredAdapters;      //目前控制的适配器列表

        public MyNetWorkMonitor()
        {
            m_AdaptersList = new ArrayList();          //用来保存获取到的计算机的适配器列表
            m_MonitoredAdapters = new ArrayList();         //运行的有效的适配器列表


            ShowNetAdapter();                                //列举出安装在该计算机上面的适配器
            Monitor_Timer = new System.Timers.Timer(1000);
            Monitor_Timer.Elapsed += new ElapsedEventHandler(timer_ElapsedClick);
        }
        private void timer_ElapsedClick(object sender, ElapsedEventArgs e)     //用于每秒钟刷新速度      
        {
            foreach (MyNetWorkMatchClass adapter in m_MonitoredAdapters)       //每秒钟遍历有效的网络适配器
            {
                adapter.CaculateAndRefresh();                                           //刷新上传下载速度                        
            }

        }


        private void ShowNetAdapter()    //列举出安装在该计算机上面的适配器方法
        {
            PerformanceCounterCategory PCCCategory = new PerformanceCounterCategory("Network Interface");
            foreach (string InstanceName in PCCCategory.GetInstanceNames())
            {
                if (InstanceName == "MS TCP Loopback interface")
                    continue;
                //  创建一个实例Net workAdapter类别，并创建性能计数器它。
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

        public void StartMonitoring(MyNetWorkMatchClass myMNWMadapter)     //控制该适配器开始工作
        {
            if (!m_MonitoredAdapters.Contains(myMNWMadapter))
            {
                m_MonitoredAdapters.Add(myMNWMadapter);
                myMNWMadapter.Start();                           //该适配器调用自己函数开始工作      
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

        public MyNetWorkMatchClass[] Adapters                //该控制类所控制的找出所有适配器的适配器列表
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
        public double DownloadSpeedKbps
        {
            get
            {
                return this.m_lUpLoadSpeed / 1024.0;
            }
        }

        public double UploadSpeedKbps
        {
            get
            {
                return this.m_lUpLoadSpeed / 1024.0;
            }
        }
        private long m_lDownLoadNetValues1;           //当前的下载速度,字节计算
        private long m_lUploadNetValues1;             //当前的上传速度
        private long m_lDownLoadNetValues2;           //一秒前的下载速度,字节计算
        private long m_lUploadNetValues2;             //一秒前的上传速度

        private string m_strMatchName;     //此适配器的名字
        internal PerformanceCounter m_Performance_Down;    //控制下载速度的流量计算中心
        internal PerformanceCounter m_Performance_Up;     // //控制上传速度的流量计算中心

        private long m_lDownLoadSpeed;      //每秒钟下载速度
        private long m_lUpLoadSpeed;          //每秒钟上传速度
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

        public void CaculateAndRefresh()                //计算速度
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
