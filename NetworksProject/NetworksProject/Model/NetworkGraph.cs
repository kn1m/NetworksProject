using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworksProject
{
    //Graph data class
    public class NetworkGraph : BidirectionalGraph<DataVertex, DataEdge>
    {
        public static NetworkGraph GetDefaultGraph()
        {
            //Lets make new data graph instance
            var dataGraph = new NetworkGraph();

            //Now we need to create edges and vertices to fill data graph
            //This edges and vertices will represent graph structure and connections
            //Lets make some vertices
            for (int i = 0; i < 9; i++)
            {
                //Create new vertex with specified Text. Also we will assign custom unique ID.
                //This ID is needed for several features such as serialization and edge routing algorithms.
                //If you don't need any custom IDs and you are using automatic Area.GenerateGraph() method then you can skip ID assignment
                //because specified method automaticaly assigns missing data ids (this behavior controlled by method param).
                var dataVertex = new DataVertex("Node " + i) { ID = i };
                //Add vertex to data graph
                dataGraph.AddVertex(dataVertex);
            }


            dataGraph.AddVertex(new DataVertex("Test") { ID = dataGraph.VertexCount});
            //Now lets make some edges that will connect our vertices
            //get the indexed list of graph vertices we have already added
            var vlist = dataGraph.Vertices.ToList();
            //Then create two edges optionaly defining Text property to show who are connected
            var dataEdge = new DataEdge(vlist[0], vlist[1]) { Text = "1", Weight = 1, IsSatelite = false };
            vlist[0].Edges.Add(dataEdge);
            vlist[1].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);
            dataEdge = new DataEdge(vlist[0], vlist[2]) { Text = "2", Weight = 2, IsSatelite = false };
            vlist[0].Edges.Add(dataEdge);
            vlist[2].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[1], vlist[2]) { Text = "3", Weight = 3, IsSatelite = false };
            vlist[1].Edges.Add(dataEdge);
            vlist[2].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[2], vlist[3]) { Text = "5", Weight = 5, IsSatelite = false };
            vlist[2].Edges.Add(dataEdge);
            vlist[3].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[3], vlist[4]) { Text = "7", Weight = 7, IsSatelite = false };
            vlist[4].Edges.Add(dataEdge);
            vlist[3].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[0], vlist[8]) { Text = "12", Weight = 12, IsSatelite = false };
            vlist[0].Edges.Add(dataEdge);
            vlist[8].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[0], vlist[5]) { Text = "12", Weight = 12, IsSatelite = false };
            vlist[0].Edges.Add(dataEdge);
            vlist[5].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[2], vlist[4]) { Text = "8", Weight = 8, IsSatelite = true };
            vlist[2].Edges.Add(dataEdge);
            vlist[4].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[7], vlist[8]) { Text = "12", Weight = 12, IsSatelite = false };
            vlist[7].Edges.Add(dataEdge);
            vlist[8].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[4], vlist[5]) { Text = "15", Weight = 15, IsSatelite = false };
            vlist[4].Edges.Add(dataEdge);
            vlist[5].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[5], vlist[6]) { Text = "21", Weight = 21, IsSatelite = false };
            vlist[6].Edges.Add(dataEdge);
            vlist[5].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[6], vlist[7]) { Text = "26", Weight = 26, IsSatelite = true };
            vlist[6].Edges.Add(dataEdge);
            vlist[7].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[8], vlist[9]) { Text = "26", Weight = 26, IsSatelite = true };
            vlist[8].Edges.Add(dataEdge);
            vlist[9].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);


            return dataGraph;
        }

    }
}
