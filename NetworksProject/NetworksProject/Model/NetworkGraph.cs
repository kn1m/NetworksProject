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
            for (int i = 0; i < 33; i++)
            {
                //Create new vertex with specified Text. Also we will assign custom unique ID.
                //This ID is needed for several features such as serialization and edge routing algorithms.
                //If you don't need any custom IDs and you are using automatic Area.GenerateGraph() method then you can skip ID assignment
                //because specified method automaticaly assigns missing data ids (this behavior controlled by method param).
                var dataVertex = new DataVertex("Node " + i) { ID = i, IsEnabled = true };
                //Add vertex to data graph
                dataGraph.AddVertex(dataVertex);
            }


            //dataGraph.AddVertex(new DataVertex("Test") { ID = dataGraph.VertexCount});
            //Now lets make some edges that will connect our vertices
            //get the indexed list of graph vertices we have already added
            var vlist = dataGraph.Vertices.ToList();
            //Then create two edges optionaly defining Text property to show who are connected
            var dataEdge = new DataEdge(vlist[0], vlist[1]) { Text = "1", Weight = 1, IsSatelite = false, IsDuplex = false };
            vlist[0].Edges.Add(dataEdge);
            vlist[1].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);
            dataEdge = new DataEdge(vlist[0], vlist[2]) { Text = "2", Weight = 2, IsSatelite = false, IsDuplex = false };
            vlist[0].Edges.Add(dataEdge);
            vlist[2].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[1], vlist[2]) { Text = "3", Weight = 3, IsSatelite = false, IsDuplex = false };
            vlist[1].Edges.Add(dataEdge);
            vlist[2].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[2], vlist[3]) { Text = "5", Weight = 5, IsSatelite = false, IsDuplex = false };
            vlist[2].Edges.Add(dataEdge);
            vlist[3].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[3], vlist[4]) { Text = "7", Weight = 7, IsSatelite = false, IsDuplex = false };
            vlist[4].Edges.Add(dataEdge);
            vlist[3].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[0], vlist[8]) { Text = "12", Weight = 12, IsSatelite = false, IsDuplex = false };
            vlist[0].Edges.Add(dataEdge);
            vlist[8].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[0], vlist[5]) { Text = "12", Weight = 12, IsSatelite = false, IsDuplex = false };
            vlist[0].Edges.Add(dataEdge);
            vlist[5].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[2], vlist[4]) { Text = "8", Weight = 8, IsSatelite = false, IsDuplex = false };
            vlist[2].Edges.Add(dataEdge);
            vlist[4].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[7], vlist[8]) { Text = "12", Weight = 12, IsSatelite = false, IsDuplex = false };
            vlist[7].Edges.Add(dataEdge);
            vlist[8].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[4], vlist[5]) { Text = "15", Weight = 15, IsSatelite = false, IsDuplex = false };
            vlist[4].Edges.Add(dataEdge);
            vlist[5].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[5], vlist[6]) { Text = "21", Weight = 21, IsSatelite = false, IsDuplex = true };
            vlist[6].Edges.Add(dataEdge);
            vlist[5].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[6], vlist[7]) { Text = "26", Weight = 26, IsSatelite = false, IsDuplex = false };
            vlist[6].Edges.Add(dataEdge);
            vlist[7].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[8], vlist[9]) { Text = "26", Weight = 26, IsSatelite = true, IsDuplex = true };
            vlist[8].Edges.Add(dataEdge);
            vlist[9].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[9], vlist[10]) { Text = "2", Weight = 2, IsSatelite = false, IsDuplex = false };
            vlist[9].Edges.Add(dataEdge);
            vlist[10].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[10], vlist[11]) { Text = "1", Weight = 1, IsSatelite = false, IsDuplex = false };
            vlist[10].Edges.Add(dataEdge);
            vlist[11].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[10], vlist[12]) { Text = "8", Weight = 8, IsSatelite = false, IsDuplex = false };
            vlist[10].Edges.Add(dataEdge);
            vlist[12].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[10], vlist[13]) { Text = "26", Weight = 26, IsSatelite = false, IsDuplex = false };
            vlist[10].Edges.Add(dataEdge);
            vlist[13].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[11], vlist[12]) { Text = "21", Weight = 21, IsSatelite = false, IsDuplex = false };
            vlist[11].Edges.Add(dataEdge);
            vlist[12].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[12], vlist[13]) { Text = "15", Weight = 15, IsSatelite = false, IsDuplex = true };
            vlist[12].Edges.Add(dataEdge);
            vlist[13].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[13], vlist[14]) { Text = "12", Weight = 12, IsSatelite = false, IsDuplex = false };
            vlist[13].Edges.Add(dataEdge);
            vlist[14].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[13], vlist[19]) { Text = "8", Weight = 8, IsSatelite = true, IsDuplex = false };
            vlist[13].Edges.Add(dataEdge);
            vlist[19].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[14], vlist[15]) { Text = "3", Weight = 3, IsSatelite = false, IsDuplex = true };
            vlist[14].Edges.Add(dataEdge);
            vlist[15].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[15], vlist[16]) { Text = "7", Weight = 7, IsSatelite = false, IsDuplex = false };
            vlist[15].Edges.Add(dataEdge);
            vlist[16].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[15], vlist[17]) { Text = "2", Weight = 2, IsSatelite = false, IsDuplex = true };
            vlist[15].Edges.Add(dataEdge);
            vlist[17].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[16], vlist[17]) { Text = "5", Weight = 5, IsSatelite = false, IsDuplex = true };
            vlist[16].Edges.Add(dataEdge);
            vlist[17].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[17], vlist[18]) { Text = "1", Weight = 1, IsSatelite = false, IsDuplex = false };
            vlist[17].Edges.Add(dataEdge);
            vlist[18].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[15], vlist[18]) { Text = "3", Weight = 3, IsSatelite = false, IsDuplex = false };
            vlist[15].Edges.Add(dataEdge);
            vlist[18].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[19], vlist[20]) { Text = "5", Weight = 5, IsSatelite = false, IsDuplex = false };
            vlist[19].Edges.Add(dataEdge);
            vlist[20].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[19], vlist[21]) { Text = "2", Weight = 2, IsSatelite = false, IsDuplex = true };
            vlist[19].Edges.Add(dataEdge);
            vlist[21].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[19], vlist[22]) { Text = "1", Weight = 1, IsSatelite = false, IsDuplex = false };
            vlist[19].Edges.Add(dataEdge);
            vlist[22].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[20], vlist[22]) { Text = "7", Weight = 7, IsSatelite = false, IsDuplex = false };
            vlist[20].Edges.Add(dataEdge);
            vlist[22].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[21], vlist[22]) { Text = "12", Weight = 12, IsSatelite = false, IsDuplex = false };
            vlist[21].Edges.Add(dataEdge);
            vlist[22].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[22], vlist[23]) { Text = "8", Weight = 8, IsSatelite = false, IsDuplex = true };
            vlist[22].Edges.Add(dataEdge);
            vlist[23].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[23], vlist[24]) { Text = "15", Weight = 15, IsSatelite = false, IsDuplex = false };
            vlist[23].Edges.Add(dataEdge);
            vlist[24].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[23], vlist[25]) { Text = "21", Weight = 21, IsSatelite = false, IsDuplex = false };
            vlist[23].Edges.Add(dataEdge);
            vlist[25].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            //dataEdge = new DataEdge(vlist[24], vlist[25]) { Text = "26", Weight = 26, IsSatelite = false, IsDuplex = true };
            //vlist[24].Edges.Add(dataEdge);
            //vlist[25].Edges.Add(dataEdge);
            //dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[9], vlist[26]) { Text = "21", Weight = 21, IsSatelite = false, IsDuplex = true };
            vlist[9].Edges.Add(dataEdge);
            vlist[26].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[26], vlist[27]) { Text = "15", Weight = 15, IsSatelite = false, IsDuplex = false };
            vlist[26].Edges.Add(dataEdge);
            vlist[27].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[26], vlist[28]) { Text = "12", Weight = 12, IsSatelite = false, IsDuplex = true };
            vlist[26].Edges.Add(dataEdge);
            vlist[28].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[27], vlist[28]) { Text = "8", Weight = 8, IsSatelite = false, IsDuplex = false };
            vlist[27].Edges.Add(dataEdge);
            vlist[28].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[28], vlist[29]) { Text = "7", Weight = 7, IsSatelite = false, IsDuplex = false };
            vlist[28].Edges.Add(dataEdge);
            vlist[29].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[28], vlist[30]) { Text = "5", Weight = 5, IsSatelite = false, IsDuplex = false };
            vlist[28].Edges.Add(dataEdge);
            vlist[30].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[28], vlist[31]) { Text = "2", Weight = 2, IsSatelite = false, IsDuplex = false };
            vlist[28].Edges.Add(dataEdge);
            vlist[31].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[30], vlist[32]) { Text = "3", Weight = 3, IsSatelite = false, IsDuplex = false };
            vlist[30].Edges.Add(dataEdge);
            vlist[32].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge(vlist[29], vlist[32]) { Text = "1", Weight = 1, IsSatelite = false, IsDuplex = true };
            vlist[29].Edges.Add(dataEdge);
            vlist[32].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            return dataGraph;
        }

        
        public void Traverse(bool mode)
        {
            vertices = new Dictionary<string, Dictionary<string, double>>();

            foreach (var currentVertex in Vertices)
            {
                var currentEdge = new Dictionary<string, double>();

                if (currentVertex.IsEnabled)
                {
                    foreach (var edge in Edges)
                    {
                        if (edge.IsEnabled)
                        {
                            if (edge.Target == currentVertex && edge.Source.IsEnabled)
                                if (mode)
                                {
                                    currentEdge[edge.Source.ToString()] = 1;
                                }
                                else
                                {
                                    currentEdge[edge.Source.ToString()] = 1 / edge.Weight;
                                    if (edge.IsSatelite)
                                        currentEdge[edge.Source.ToString()] /= 3;
                                    if (edge.IsDuplex)
                                        currentEdge[edge.Source.ToString()] /= 1.5;
                                }
                            if (edge.Source == currentVertex && edge.Target.IsEnabled)
                                if (mode)
                                {
                                    currentEdge[edge.Target.ToString()] = 1;
                                }
                                else
                                {
                                    currentEdge[edge.Target.ToString()] = 1 / edge.Weight;
                                    if (edge.IsSatelite)
                                        currentEdge[edge.Target.ToString()] /= 3;
                                    if (edge.IsDuplex)
                                        currentEdge[edge.Target.ToString()] /= 1.5;
                                }
                        }
                    }
                    vertices[currentVertex.ToString()] = currentEdge;
                }
            }

        }

        private Dictionary<string, Dictionary<string, double>> vertices = new Dictionary<string, Dictionary<string, double>>();

        public void add_vertex(string name, Dictionary<string, double> edges)
        {
            vertices[name] = edges;
        }

        public List<string> shortest_path(string start, string finish)
        {
            var previous = new Dictionary<string, string>();
            var distances = new Dictionary<string, double>();
            var nodes = new List<string>();

            List<string> path = null;

            foreach (var vertex in vertices)
            {
                if (vertex.Key == start)
                {
                    distances[vertex.Key] = 0;
                }
                else
                {
                    distances[vertex.Key] = double.MaxValue;
                }

                nodes.Add(vertex.Key);
            }

            while (nodes.Count != 0)
            {

                nodes.Sort((x, y) => distances[x].CompareTo(distances[y]));

                var smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest == finish)
                {
                    path = new List<string>();
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == double.MaxValue)
                {
                    break;
                }

                foreach (var neighbor in vertices[smallest])
                {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }

            return path;
        }


    }
}
