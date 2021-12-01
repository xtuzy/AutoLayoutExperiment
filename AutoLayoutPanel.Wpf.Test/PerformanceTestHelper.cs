using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Helper.Tools
{
    public static class PerformanceTestHelper
    {

        static Stopwatch timer;
        //static StringBuilder result = new StringBuilder();

        public static void CreateTest()
        {
            timer = new Stopwatch();
            timer.Start();
            timer.Stop();
        }

        public static void StartRecord()
        {
            if(timer != null)
                timer.Restart();
        }

        public static void OutputRecord(string message)
        {
            if(timer==null)
                return;
            timer.Stop();
            Debug.WriteLine("SpendTime:" + $"{timer.Elapsed.TotalMilliseconds * 1000:n3}μs" + " RecordMessage:" + message);
        }
    }
}
