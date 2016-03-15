using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtil
{
    /// <summary>
    /// 定时器
    /// </summary>
    public class Time_Task
    {
        public event System.Timers.ElapsedEventHandler ExecuteTask;

        private static readonly Time_Task _task = null;
        private System.Timers.Timer _timer = null;


        //定义间隔时间
        private int _interval = 1000;
        public int Interval
        {
            set
            {
                _interval = value;
            }
            get
            {
                return _interval;
            }
        }

        static Time_Task()
        {
            _task = new Time_Task();
        }

        public static Time_Task Instance()
        {
            return _task;
        }

        //开始
        public void Start()
        {
            if (_timer == null)
            {
                _timer = new System.Timers.Timer(_interval);
                _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
                _timer.Enabled = true;
                _timer.Start();
            }
        }

        protected void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (null != ExecuteTask)
            {
                ExecuteTask(sender, e);
            }
        }

        //停止
        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }

        // 在Global应用程序启动时运行的代码  
        //  Time_Task.Instance().ExecuteTask += new System.Timers.ElapsedEventHandler(Global_ExecuteTask);//需要定时执行的方法名
        //  Time_Task.Instance().Interval = 1000*10;//表示间隔
        //  Time_Task.Instance().Start();

    }
}
