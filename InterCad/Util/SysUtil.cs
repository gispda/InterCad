using System;
using System.Collections.Generic;
using System.Text;

namespace InterDesignCad.Util
{
    class SysUtil
    {
        public static string getCfgPath()
        {
            string sPath = Environment.GetEnvironmentVariable("INTERCAD");
            if (sPath == null || sPath == "")
                sPath = "F:\\project\\InterCad\\InterCad\\";
            return sPath;
        }
    }
}
