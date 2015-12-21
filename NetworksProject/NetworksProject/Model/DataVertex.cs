using GraphX.PCL.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworksProject
{
    //Vertex data object
    public class DataVertex : VertexBase
    {
        /// <summary>
        /// Some string property for example purposes
        /// </summary>
        public string Text { get; set; }

        public bool IsEnabled { get; set; } = true;

        public List<DataEdge> Edges { get; set; } = new List<DataEdge>();

        public string Routing { get; set; } = "";

        public int RecievedTCPControlPackets { get; set; } = 0;
        public int RecievedTCPDataPackets { get; set; } = 0;

        public int SendedTCPControlPackets { get; set; } = 0;
        public int SendedTCPDataPackets { get; set; } = 0;

        public int SendedUDPPackets { get; set; } = 0;
        public int RecivedUDPPackets { get; set; } = 0;

        public override string ToString()
        {
            return Text;
        }

        public string GetRouting()
        {
            return Environment.NewLine + Routing;
        }

        public List<DataVertex> AdjVert
        {
            get
            {
                var res = new List<DataVertex>();

                foreach (var item in Edges)
                {
                    if(item.Source == this)
                    {
                        res.Add(item.Target);
                    }
                    else res.Add(item.Source);
                }

                return res;

            }
        }

        public DataVertex(string text)
        {
            Text = text;
        }
    }
}
