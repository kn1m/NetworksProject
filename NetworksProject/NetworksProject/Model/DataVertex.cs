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

        public List<DataEdge> Edges { get; set; } = new List<DataEdge>();

        public List<List<DataVertex>> Paths
        {
            get
            {
                var res = new List<List<DataVertex>>();



                return res;
            }
        }



        public override string ToString()
        {
            return Text;
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

        public static List<DataVertex> GetDefaultSetUp()
        {
            var predifined = new List<DataVertex>();


            return predifined;
        }

    }
}
