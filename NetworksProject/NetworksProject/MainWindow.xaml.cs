﻿using GraphX;
using QuickGraph;
using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GraphX.PCL.Logic.Algorithms.OverlapRemoval;
using GraphX.PCL.Common.Enums;
using GraphX.Controls;

namespace NetworksProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window, IDisposable
    {

        private DataEdge Test;
        private DataVertex _selected;
        private NetworkGXLogicCore logicCore;
        private NetworkGraph dataGraph;
        private string Routing;

        private List<DataVertex> _vertexes = new List<DataVertex>();
        private Dictionary<DataVertex, List<DataEdge>>  _VertexEdgeMapping = new Dictionary<DataVertex, List<DataEdge>>();
        private Dictionary<DataVertex, string> color = new Dictionary<DataVertex, string>();
        private Dictionary<DataVertex, DataVertex> parent = new Dictionary<DataVertex, DataVertex>();
        private List<List<DataVertex>> paths = new List<List<DataVertex>>();
        private List<List<DataVertex>> paths1 = new List<List<DataVertex>>();

        public MainWindow()
        {
            InitializeComponent();
            Routing = "";
            //Customize Zoombox a bit
            //Set minimap (overview) window to be visible by default
            ZoomControl.SetViewFinderVisibility(zoomctrl, Visibility.Visible);
            //Set Fill zooming strategy so whole graph will be always visible
            zoomctrl.ZoomToFill();

            //Lets setup GraphArea settings
            GraphAreaExample_Setup();


            //Adding new vertex
            zoomctrl.MouseDoubleClick += ((o, s) => { VertexInputBox.Visibility = Visibility.Visible; });

            //Adding new edge
            Area.VertexSelected += ((h, j) => {
                _selected = (DataVertex)j.VertexControl.Vertex;
                EdgeInputBox.Visibility = Visibility.Visible; });

            Area.EdgeMouseEnter += ((h,j) => { j.EdgeControl.ToolTip = j.EdgeControl.Edge.ToString(); });

            //Vertex tooltip
            Area.VertexMouseEnter += ((h,j) => { j.VertexControl.ToolTip = j.VertexControl.Vertex.ToString(); } );

            gg_but_randomgraph.Click += gg_but_randomgraph_Click;

            gg_but_relayout.Click += gg_but_relayout_Click;
            
            Loaded += MainWindow_Loaded;

            SearchShortestWay(true);
        }


        //private void ff(DataVertex root)
        //{

        //    foreach (DataVertex vertex in _vertexes)
        //    {
        //        color.Add(vertex, "white");
        //        parent.Add(vertex, null);
        //    }


        //    Queue<DataVertex> queue = new Queue<DataVertex>();
        //    queue.Enqueue(root);

        //    var asd = new List<DataVertex>();
        //    while (queue.Count != 0)
        //    {
        //        DataVertex temp = queue.Dequeue();
                
        //        foreach (DataEdge edge in _VertexEdgeMapping[temp])
        //        {
        //            if (color[edge.Target] == "white")
        //            {
        //                color[edge.Target] = "gray";
        //                parent[edge.Target] = temp;
        //                queue.Enqueue(edge.Target);
        //                asd.Add(edge.Source);
        //                asd.Add(edge.Target);
        //            }
                   

        //        }

        //        //MessageBox.Show(temp.Text);
        //        //asd.Add(temp);
        //        color[temp] = "black";
        //        Console.WriteLine("Vertex {0} has been found!", temp.Text);
        //        paths.Add(new List<DataVertex>(asd));
                

        //    }
        //}


        private void ff(DataVertex root)
        {

            foreach (DataVertex vertex in _vertexes)
            {
                color.Add(vertex, "white");
                parent.Add(vertex, null);
            }


            Queue<DataVertex> queue = new Queue<DataVertex>();
            queue.Enqueue(root);

            //var asd = new List<DataVertex>();
            while (queue.Count != 0)
            {
                DataVertex temp = queue.Dequeue();

                foreach (DataEdge edge in _VertexEdgeMapping[temp])
                {
                    if (color[edge.Target] == "white")
                    {
                        var asd = new List<DataVertex>();
                        color[edge.Target] = "gray";
                        parent[edge.Target] = temp;
                        queue.Enqueue(edge.Target);

                        asd.Add(edge.Source);

                        asd.Add(edge.Target);

                        paths.Add(new List<DataVertex>(asd));
                    }
                }

                color[temp] = "black";
                
                Console.WriteLine("Vertex {0} has been found!", temp.Text);
            }
        }

        private void SearchShortestWay(bool mode)
        {

            //int vertexCount = Area.LogicCore.Graph.VertexCount;
            //var HopesMatrix = new int[vertexCount, vertexCount];
            //var ChannelMatrix = new int[vertexCount, vertexCount];

            //foreach (var item in Area.LogicCore.Graph.Edges)
            //{
            //    HopesMatrix[item.Source.ID, item.Target.ID] = 1;
            //    ChannelMatrix[item.Source.ID, item.Target.ID] = (int)item.Weight;
            //}



            _vertexes = Area.LogicCore.Graph.Vertices.ToList();
            foreach (var item in Area.LogicCore.Graph.Vertices)
            {
                _VertexEdgeMapping.Add(item, item.Edges);
            }

            DataVertex tmp = null;
            var temp = new List<DataVertex>();
            foreach (var item in Area.LogicCore.Graph.Vertices)
            {

                ff(item);

                tmp = item;





                break;
            }

            var la = new List<List<DataVertex>>();
            foreach (var item in paths)
            {

                foreach (var itemss in paths)
                {
                    if (item[1] == itemss[0])
                    {
                        paths1.Add(new List<DataVertex>(item.Concat(itemss)));
                        la.Add(itemss);
                    }
                }
            }



            foreach (var itemz in paths1)
            {
                if (itemz.First() != tmp)
                {
                    itemz.Insert(0, tmp);
                }

            }


            foreach (var item in la)
            {
                paths.Remove(item);
            }

            paths1 = new List<List<DataVertex>>(paths1.Concat(paths));
            paths.Clear();

            foreach (var item in paths1)
            {
                paths.Add(item.Distinct().ToList());
            }

            Console.WriteLine();

        }




        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //lets create graph
            //Note that you can't create it in class constructor as there will be problems with visuals
            gg_but_randomgraph_Click(null, null);

        }

        void gg_but_relayout_Click(object sender, RoutedEventArgs e)
        {
            //This method initiates graph relayout process which involves consequnet call to all selected algorithms.
            //It behaves like GenerateGraph() method except that it doesn't create any visual object. Only update existing ones
            //using current Area.Graph data graph.
            Area.RelayoutGraph();
            zoomctrl.ZoomToFill();
        }
        
        void gg_but_randomgraph_Click(object sender, RoutedEventArgs e)
        {
            //Lets generate configured graph using pre-created data graph assigned to LogicCore object.
            //Optionaly we set first method param to True (True by default) so this method will automatically generate edges
            //  If you want to increase performance in cases where edges don't need to be drawn at first you can set it to False.
            //  You can also handle edge generation by calling manually Area.GenerateAllEdges() method.
            //Optionaly we set second param to True (True by default) so this method will automaticaly checks and assigns missing unique data ids
            //for edges and vertices in _dataGraph.
            //Note! Area.Graph property will be replaced by supplied _dataGraph object (if any).
            Area.GenerateGraph(true, true);
            /* 
             * After graph generation is finished you can apply some additional settings for newly created visual vertex and edge controls
             * (VertexControl and EdgeControl classes).
             * 
             */
            //This method sets the dash style for edges. It is applied to all edges in Area.EdgesList. You can also set dash property for
            //each edge individually using EdgeControl.DashStyle property.
            //For ex.: Area.EdgesList[0].DashStyle = GraphX.EdgeDashStyle.Dash;
            Area.SetEdgesDashStyle(EdgeDashStyle.Solid);

            //This method sets edges arrows visibility. It is also applied to all edges in Area.EdgesList. You can also set property for
            //each edge individually using property, for ex: Area.EdgesList[0].ShowArrows = true;
            //Area.ShowAllEdgesArrows(false);


            foreach (var item in Area.EdgesList.Values)
            {
                item.AlignLabelsToEdges = true;
                item.LabelVerticalOffset = 10;
                

            }

            foreach (var item in Area.EdgesList.Keys)
            {
                if (item.IsSatelite)
                    Area.EdgesList[item].Foreground = Brushes.Green;
            }



            //!!!!
            //Area.EdgesList[Test].DashStyle = EdgeDashStyle.Dot;
            //Area.EdgesList[Test].Foreground = Brushes.Red;

            //foreach (var item in Area.EdgesList.Values)
            //{
            //    item.DashStyle = EdgeDashStyle.Dot;
            //}

            //This method sets edges labels visibility. It is also applied to all edges in Area.EdgesList. You can also set property for
            //each edge individually using property, for ex: Area.EdgesList[0].ShowLabel = true;
            Area.ShowAllEdgesLabels(true);

            zoomctrl.ZoomToFill();
        }
        
        private NetworkGraph GraphExample_Setup()
        {
            //Lets make new data graph instance
            dataGraph = new NetworkGraph();
            
            //Now we need to create edges and vertices to fill data graph
            //This edges and vertices will represent graph structure and connections
            //Lets make some vertices
            for (int i = 0; i < 9; i++)
            {
                //Create new vertex with specified Text. Also we will assign custom unique ID.
                //This ID is needed for several features such as serialization and edge routing algorithms.
                //If you don't need any custom IDs and you are using automatic Area.GenerateGraph() method then you can skip ID assignment
                //because specified method automaticaly assigns missing data ids (this behavior controlled by method param).
                var dataVertex = new DataVertex("Node " + i) { ID = i};
                //Add vertex to data graph
                dataGraph.AddVertex(dataVertex);
            }
        
            //Now lets make some edges that will connect our vertices
            //get the indexed list of graph vertices we have already added
            var vlist = dataGraph.Vertices.ToList();
            //Then create two edges optionaly defining Text property to show who are connected
            var dataEdge = new DataEdge(vlist[0], vlist[1]) { Text = "1", Weight = 1, IsSatelite = false };
            vlist[0].Edges.Add(dataEdge);
            vlist[1].Edges.Add(dataEdge);
            Test = dataEdge;
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

            dataEdge = new DataEdge( vlist[5], vlist[6]) { Text = "21", Weight = 21, IsSatelite = false };
            vlist[6].Edges.Add(dataEdge);
            vlist[5].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            dataEdge = new DataEdge( vlist[6], vlist[7]) { Text = "26", Weight = 26, IsSatelite = true };
            vlist[6].Edges.Add(dataEdge);
            vlist[7].Edges.Add(dataEdge);
            dataGraph.AddEdge(dataEdge);

            


            return dataGraph;
        }

        private NetworkGraph GraphRandom_Setup()
        {
            //Lets make new data graph instance
            dataGraph = new NetworkGraph();
            return dataGraph;
        }



        private void GraphAreaExample_Setup()
        {
            //Lets create logic core and filled data graph with edges and vertices
            logicCore = new NetworkGXLogicCore() { Graph = GraphExample_Setup() };
            //This property sets layout algorithm that will be used to calculate vertices positions
            //Different algorithms uses different values and some of them uses edge Weight property.
            logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;
            //Now we can set parameters for selected algorithm using AlgorithmFactory property. This property provides methods for
            //creating all available algorithms and algo parameters.
            logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);
            //Unfortunately to change algo parameters you need to specify params type which is different for every algorithm.
            ((KKLayoutParameters)logicCore.DefaultLayoutAlgorithmParams).MaxIterations = 100;
        
            //This property sets vertex overlap removal algorithm.
            //Such algorithms help to arrange vertices in the layout so no one overlaps each other.
            logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;
            //Default parameters are created automaticaly when new default algorithm is set and previous params were NULL
            logicCore.DefaultOverlapRemovalAlgorithmParams.HorizontalGap = 50;
            logicCore.DefaultOverlapRemovalAlgorithmParams.VerticalGap = 50;
        
            //This property sets edge routing algorithm that is used to build route paths according to algorithm logic.
            //For ex., SimpleER algorithm will try to set edge paths around vertices so no edge will intersect any vertex.
            //Bundling algorithm will try to tie different edges that follows same direction to a single channel making complex graphs more appealing.
            logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;
        
            //This property sets async algorithms computation so methods like: Area.RelayoutGraph() and Area.GenerateGraph()
            //will run async with the UI thread. Completion of the specified methods can be catched by corresponding events:
            //Area.RelayoutFinished and Area.GenerateGraphFinished.
            logicCore.AsyncAlgorithmCompute = false;
            //Finally assign logic core to GraphArea object
            Area.LogicCore = logicCore;
        }
        
        public void Dispose()
        {
            //If you plan dynamicaly create and destroy GraphArea it is wise to use Dispose() method
            //that ensures that all potential memory-holding objects will be released.
            Area.Dispose();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            VertexInputBox.Visibility = Visibility.Collapsed;

            if(!string.IsNullOrWhiteSpace(VertexTextBox.Text))
            {
                logicCore.Graph.AddVertex(new DataVertex(VertexTextBox.Text));
                gg_but_randomgraph_Click(null, null);
            }

            VertexTextBox.Text = null;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            VertexInputBox.Visibility = Visibility.Collapsed;
            VertexTextBox.Text = null;
        }

        private void btnSubmitEdge_Click(object sender, RoutedEventArgs e)
        {
            EdgeInputBox.Visibility = Visibility.Collapsed;

            if (!string.IsNullOrWhiteSpace(EdgeTextBox.Text) && !string.IsNullOrWhiteSpace(NVertexTextBox.Text))
            {
                int weight;
                if (int.TryParse(EdgeTextBox.Text, out weight))
                {
                    var ver = logicCore.Graph.Vertices.Where(x => x.Text == NVertexTextBox.Text);
                    if (ver.Count() != 0)
                    {
                        if(isRegionalCheck.IsChecked.Value)
                            logicCore.Graph.AddEdge(new DataEdge(_selected, ver.First()) { Text = EdgeTextBox.Text, Weight = weight, IsSatelite = true });
                        else
                            logicCore.Graph.AddEdge(new DataEdge(_selected, ver.First()) { Text = EdgeTextBox.Text, Weight = weight, IsSatelite = false });

                        gg_but_randomgraph_Click(null, null);
                    }
                }
                else
                {
                    WarningBox.Visibility = Visibility.Visible;
                }
            }
            else
            {
                WarningBox.Visibility = Visibility.Visible;
            }
            NVertexTextBox.Text = null;
            EdgeTextBox.Text = null;
        }

        private void btnCancelEdge_Click(object sender, RoutedEventArgs e)
        {
            EdgeInputBox.Visibility = Visibility.Collapsed;
            EdgeTextBox.Text = null;
        }

        private void btnCustom_Click(object sender, RoutedEventArgs e)
        {

            Area.LogicCore.Dispose();
            Area.ClearLayout();

            logicCore = new NetworkGXLogicCore() { Graph = GraphRandom_Setup() };
            //This property sets layout algorithm that will be used to calculate vertices positions
            //Different algorithms uses different values and some of them uses edge Weight property.
            logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;
            //Now we can set parameters for selected algorithm using AlgorithmFactory property. This property provides methods for
            //creating all available algorithms and algo parameters.
            logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);
            //Unfortunately to change algo parameters you need to specify params type which is different for every algorithm.
            ((KKLayoutParameters)logicCore.DefaultLayoutAlgorithmParams).MaxIterations = 100;

            logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;

            logicCore.DefaultOverlapRemovalAlgorithmParams.HorizontalGap = 50;
            logicCore.DefaultOverlapRemovalAlgorithmParams.VerticalGap = 50;

            logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;
            logicCore.AsyncAlgorithmCompute = false;
            Area.LogicCore = logicCore;

            gg_but_randomgraph_Click(null, null);

        }

        private void btnSubmitWarning_Click(object sender, RoutedEventArgs e)
        {
            WarningBox.Visibility = Visibility.Collapsed;
        }
    }
}
