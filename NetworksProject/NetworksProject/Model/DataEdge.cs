using GraphX.PCL.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworksProject
{
    //Edge data object
    public class DataEdge : EdgeBase<DataVertex>
    {
        public DataEdge(DataVertex source, DataVertex target, double weight = 1)
            : base(source, target, weight)
        {
        }

        public DataEdge()
            : base(null, null, 1)
        {
        }

        public string Text { get; set; }

        public int RegionId { get; set; }
        public bool IsSatelite { get; set; }
        public bool IsDuplex { get; set; }
        public bool IsEnabled { get; set; } = true;

        public string GetEdgeType()
        {
            var res = "";
            res += "Weigth: " + Text + Environment.NewLine;
            if (IsSatelite)
                res += "Type: Satellite";
            else
                res += "Type: Reginonal";
            if (IsDuplex)
                res += "\r\n         Duplex";
            else
                res += "\r\n         Half-duplex";
            return res;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
