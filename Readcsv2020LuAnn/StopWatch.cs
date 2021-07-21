using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Readcsv2020LuAnn
{
    public class StopWatch
    {
        /// <summary>
        /// 專門做時間記錄的物件
        /// </summary>
        private static StopWatch stopWatch;
        /// <summary>
        /// 官方提供記錄時間的物件
        /// </summary>
        private Stopwatch stopwatch;
        private StopWatch()
        {
            stopwatch = new Stopwatch();
        }
        /// <summary>
        /// 取得唯一的時間記錄物件
        /// </summary>
        /// <returns>時間記錄物件</returns>
        public static StopWatch GetStopWatch()
        {
            if (stopWatch == null)
            {
                stopWatch = new StopWatch();
            }
            return stopWatch;

        }
        /// <summary>
        /// 時間開始
        /// </summary>
        public void Start()
        {
            stopwatch.Restart();
        }
        /// <summary>
        /// 時間結束
        /// </summary>
        /// <returns>所費時間字串</returns>
        public string End()
        {
            stopwatch.Stop();
            string timeDisplay = stopwatch.Elapsed.ToString(@"hh\:mm\:ss\:fff");
            return timeDisplay;
        }
    }
}
