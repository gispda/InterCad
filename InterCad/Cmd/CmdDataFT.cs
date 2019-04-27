using System;
using System.Collections.Generic;
using System.Text;
using InterDesignCad;
using InterDesignCad.Cmd;

namespace InterDesignCad.Cmd
{
    class CmdDataFT 
    {

        public CmdDataFT()
        {
            IronRectPipePara = new CmdSubParaIronRectPipe();
        }
        // 龙骨离量取线距离
        private double distanceDragonBoneToSelectLine;

        public double DistanceDragonBoneToSelectLine
        {
            get { return distanceDragonBoneToSelectLine; }
            set { distanceDragonBoneToSelectLine = value; }
        }

        private CmdSubParaIronRectPipe ironRectPipePara;

        public CmdSubParaIronRectPipe IronRectPipePara
        {
            get { return ironRectPipePara; }
            set { ironRectPipePara = value; }
        }



        //public override CmdData GetCmdData()
        //{
        //    throw new NotImplementedException();
        //}
    }

    //钢矩管参数
    class CmdSubParaIronRectPipe
    { 
       
        // 高度
        public double height { set; get; }

       //是否有芯筒
        public bool isDrawXinTong { set; get; }

       //60*40*4
       public double length{set;get;}

       public double width{set;get;}

       public double thick { set; get; }

       
        /// <summary>
        /// 60*40*4 
        /// </summary>
        /// <param name="sspec"></param>

       public void parseSpec(string sspec)
       {
           string[] sArray = sspec.Split('*');


           if (sArray != null)
           {
               length = Convert.ToDouble(sArray[0]);
               width = Convert.ToDouble(sArray[1]);
               thick = Convert.ToDouble(sArray[2]);
           }
       }

    }

}