using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;



namespace HomeDesignCAD.Util
{
    public struct HdCAD_lParam
    {

        public int i;

        public string s;

    };
    class MsgUtil
    {

        public const int USER = 0x500;

        public const int HDCAD_MESSAGE = USER + 1;


        [DllImport("User32.dll", EntryPoint = "SendMessage")]

        private static extern int SendMessage(

        IntPtr hWnd,        // 信息发往的窗口的句柄

        int Msg,            // 消息ID

        int wParam,         // 参数1

        ref HdCAD_lParam lParam

        );

        [DllImport("User32.dll", EntryPoint = "FindWindow")]

        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        public static void SendMsg(string lpWindowName, string msg)
        {
            IntPtr ptr = FindWindow(null, lpWindowName);//获取接收消息的窗体句柄

            //消息构建

            HdCAD_lParam m = new HdCAD_lParam();

            m.s = msg;

            m.i = m.s.Length;

            SendMessage(ptr, HDCAD_MESSAGE, 1, ref m);//发送消息
        }
    }
}
