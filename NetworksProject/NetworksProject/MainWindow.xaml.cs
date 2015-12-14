using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using GraphX.PCL.Common.Enums;
using GraphX.Controls;
using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;

namespace NetworksProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window, IDisposable
    {

        
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
            //Customize Zoombox a bit
            //Set minimap (overview) window to be visible by default
            ZoomControl.SetViewFinderVisibility(zoomctrl, Visibility.Visible);
            //Set Fill zooming strategy so whole graph will be always visible
            zoomctrl.ZoomToFill();

            //Lets setup GraphArea settings
            GraphAreaExample_Setup();

            //Adding new vertex
            //zoomctrl.MouseDoubleClick += ((o, s) => { VertexInputBox.Visibility = Visibility.Visible; });

            zoomctrl.MouseRightButtonUp += ((o, s) => { VertexInputBox.Visibility = Visibility.Visible; });

            //Adding new edge
            Area.VertexSelected += ((h, j) => {
                _selected = (DataVertex)j.VertexControl.Vertex;
                EdgeInputBox.Visibility = Visibility.Visible; });

            
            Area.EdgeMouseEnter += ((h,j) => { j.EdgeControl.ToolTip = j.EdgeControl.Edge.ToString(); });

            //Vertex tooltip
            Area.VertexMouseEnter += ((h,j) => 
            {
                foreach(var item in logicCore.Graph.Vertices)
                {
                    if(item.Text == j.VertexControl.Vertex.ToString())
                    {
                        j.VertexControl.ToolTip = item.Text + "\n\n"+ item.Routing;
                    }
                }
                //j.VertexControl.ToolTip = j.VertexControl.Vertex.ToString();
            });

            gg_but_randomgraph.Click += gg_but_randomgraph_Click;

            gg_but_relayout.Click += gg_but_relayout_Click;
            
            Loaded += MainWindow_Loaded;

            SearchShortestWay(true);
        }

        private void FloodFill(DataVertex root)
        {

            foreach (DataVertex vertex in _vertexes)
            {
                color.Add(vertex, "white");
                parent.Add(vertex, null);
            }

            Queue<DataVertex> queue = new Queue<DataVertex>();
            queue.Enqueue(root);

            while (queue.Count != 0)
            {
                DataVertex temp = queue.Dequeue();

                foreach (DataEdge edge in _VertexEdgeMapping[temp])
                {
                    if (edge.Target != root)
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

                        if (edge.Target == temp)
                        {
                            if (color[edge.Source] == "white")
                            {
                                var asd = new List<DataVertex>();
                                color[edge.Source] = "gray";
                                parent[edge.Source] = temp;
                                queue.Enqueue(edge.Source);

                                asd.Add(edge.Target);
                                asd.Add(edge.Source);

                                paths.Add(new List<DataVertex>(asd));
                            }
                        }

                    }
                    else
                    if (edge.Target == temp)
                    {
                        if (color[edge.Source] == "white")
                        {
                            var asd = new List<DataVertex>();
                            color[edge.Source] = "gray";
                            parent[edge.Source] = temp;
                            queue.Enqueue(edge.Source);

                            asd.Add(edge.Target);
                            asd.Add(edge.Source);

                            paths.Add(new List<DataVertex>(asd));
                        }
                    }
                }
                color[temp] = "black";
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

            _vertexes = new List<DataVertex>();
            _VertexEdgeMapping = new Dictionary<DataVertex, List<DataEdge>>();
            Routing = "Routings:\n\n";

            _vertexes = Area.LogicCore.Graph.Vertices.ToList();
            foreach (var item in Area.LogicCore.Graph.Vertices)
            {
                _VertexEdgeMapping.Add(item, item.Edges);
            }

            var temp = new List<DataVertex>();
            foreach (var item in Area.LogicCore.Graph.Vertices)
            {

                FloodFill(item);
                FinalizeRouting(item);

                color = new Dictionary<DataVertex, string>();
                parent = new Dictionary<DataVertex, DataVertex>();
                paths = new List<List<DataVertex>>();
                paths1 = new List<List<DataVertex>>();


            }

            //rt.Text = Routing;
        }

        private void FinalizeRouting(DataVertex currentVertex)
        {

            //Concatenating steps for each vertex
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

            //Fixing result of concatenation
            foreach (var itemz in paths1)
            {
                if (itemz.First() != currentVertex)
                {
                    itemz.Insert(0, currentVertex);
                    foreach (var item in paths)
                    {
                        if (paths.Find(x => x[0] == currentVertex && x[1] == itemz[1]) == null)
                        {       
                            if (item[0] == currentVertex && item[1].AdjVert.Contains(itemz[1]))
                            {
                                itemz.Insert(1, item[1]);
                                break;
                            }
                        }
                    }
                }

            }

            //Cleaning already added pathes
            foreach (var item in la)
            {
                paths.Remove(item);
            }

            //Resulting pathes
            paths1 = new List<List<DataVertex>>(paths1.Concat(paths));
            paths.Clear();

            //Removing duplicates for better output
            foreach (var item in paths1)
            {
                paths.Add(item.Distinct().ToList());
            }

            currentVertex.Routing = "";
            Routing += currentVertex.ToString();
            Routing += Environment.NewLine;
            foreach (var item in paths)
            {
                Routing += "\t";
                foreach (var items in item)
                {
                    if (items != item.Last())
                    {
                        Routing = Routing + items.Text + " -> ";
                        currentVertex.Routing += items.Text + " -> ";
                    }
                    else
                    {
                        Routing = Routing + items.Text + Environment.NewLine;
                        currentVertex.Routing += items.Text + Environment.NewLine;
                    }
                }

            }
        }


        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            gg_but_randomgraph_Click(null, null);

        }

        void gg_but_relayout_Click(object sender, RoutedEventArgs e)
        {
            Area.RelayoutGraph();
            zoomctrl.ZoomToFill();
        }
        
        void gg_but_randomgraph_Click(object sender, RoutedEventArgs e)
        {
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
            dataGraph = NetworkGraph.GetDefaultGraph();
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
                        if (isRegionalCheck.IsChecked.Value)
                        {
                            var newEdge = new DataEdge(_selected, ver.First()) { Text = EdgeTextBox.Text, Weight = weight, IsSatelite = true };
                            _selected.Edges.Add(newEdge);
                            ver.First().Edges.Add(newEdge);
                            logicCore.Graph.AddEdge(newEdge);
                        }
                        else
                        {
                            var newEdge = new DataEdge(_selected, ver.First()) { Text = EdgeTextBox.Text, Weight = weight, IsSatelite = false };
                            _selected.Edges.Add(newEdge);
                            ver.First().Edges.Add(newEdge);
                            logicCore.Graph.AddEdge(newEdge);
                        }

                        SearchShortestWay(true);
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
