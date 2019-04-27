using System;
using System.Collections.Generic;

using System.Text;
using HomeDesignCAD.FT.Cmd;

namespace HomeDesignCAD.Cmd
{

    public enum CommandDataType
    {
        FT = 1,
        A4 = 2
    };


    public abstract class CmdData
    {
        // 得到命令数据参数
        public abstract CmdData GetCmdData();
    };


    /// <summary>
    /// 获得命令数据参数的帮助类
    /// </summary>
    public class CmdHelper
    {


        protected  static CmdData gcmddata;
        protected static CommandDataType cmdtype;


        /// <summary>
        /// 获得命令数据参数方法，根据命令类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static CmdData GetCmdData(CommandDataType type)
        {
          

            if (type==CommandDataType.FT)
            {
                if (gcmddata == null)
                    gcmddata = new CmdDataFT();
                else
                {
                    if (type != cmdtype)
                    {
                        gcmddata = new CmdDataFT();
                    }
                }
            }
            else if (type==CommandDataType.A4)
            {
               
            }
            cmdtype = type;
            return gcmddata;
        }


     
  

      


   
    }
}
