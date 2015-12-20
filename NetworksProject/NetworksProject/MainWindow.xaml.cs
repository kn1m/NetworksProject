using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using GraphX.PCL.Common.Enums;
using GraphX.Controls;
using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;
using System.IO;
using GraphX.Controls.Models;

namespace NetworksProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window, IDisposable
    {

        private DataVertex _selectedVertex;
        private DataEdge _selectedEdge;
        private NetworkGXLogicCore logicCore;
        private NetworkGraph dataGraph;

        private VertexSelectedEventArgs _selectedVertexEvent;
        private EdgeSelectedEventArgs _selectedEdgeEvent;

        private NetworkGraph Graph;

        public MainWindow()
        {
            InitializeComponent();
            //Customize Zoombox a bit
            //Set minimap (overview) window to be visible by default
            ZoomControl.SetViewFinderVisibility(zoomctrl, Visibility.Visible);
            //Set Fill zooming strategy so whole graph will be always visible
            zoomctrl.ZoomToFill();

            var modes = new List<string>() { "By hopes", "By speed" };
            routingModeBox.ItemsSource = modes;
            routingModeBox.SelectedItem = modes[0];

            //Lets setup GraphArea settings
            GraphAreaExample_Setup();

            zoomctrl.MouseRightButtonUp += ((o, s) => { VertexInputBox.Visibility = Visibility.Visible; });

            //Vertex settings
            Area.VertexSelected += ((h, j) => {
                _selectedVertexEvent = j;
                _selectedVertex = (DataVertex)j.VertexControl.Vertex;
                VertexBox.Visibility = Visibility.Visible; });

            Area.EdgeSelected += ((h, j) => {
                _selectedEdgeEvent = j;
                _selectedEdge = (DataEdge)j.EdgeControl .Edge;
                EdgeBox.Visibility = Visibility.Visible;
            });

            Area.EdgeMouseEnter += ((h,j) =>
            {
                
                foreach (var item in logicCore.Graph.Edges)
                {
                    if (item.Text == j.EdgeControl.Edge.ToString())
                    {
                        j.EdgeControl.ToolTip = item.GetEdgeType();
                    }
                }

            });

            //Vertex tooltip
            Area.VertexMouseEnter += ((h,j) => 
            {
                foreach(var item in logicCore.Graph.Vertices)
                {
                    if(item.Text == j.VertexControl.Vertex.ToString())
                    {
                        if (item.IsEnabled)
                            j.VertexControl.ToolTip = item.Text + "\n\n" + item.Routing;
                        else
                            j.VertexControl.ToolTip = item.Text;
                    }
                }
            });

            gg_but_randomgraph.Click += gg_but_randomgraph_Click;

            gg_but_relayout.Click += gg_but_relayout_Click;
            
            Loaded += MainWindow_Loaded;

            Graph = NetworkGraph.GetDefaultGraph();
            
            SearchShortestWay(true);
        }


        private void btnChangeState_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedVertex.IsEnabled)
            {
                _selectedVertexEvent.VertexControl.Foreground = Brushes.Red;
                _selectedVertex.IsEnabled = false;
            }
            else
            {
                _selectedVertexEvent.VertexControl.Foreground = Brushes.Black;
                _selectedVertex.IsEnabled = true;
            }
            Graph = (NetworkGraph)logicCore.Graph;
            btnApply_Click(null, null);
            VertexBox.Visibility = Visibility.Collapsed;
        }

        private void btnAddEdge_Click(object sender, RoutedEventArgs e)
        {
            VertexBox.Visibility = Visibility.Collapsed;
            EdgeInputBox.Visibility = Visibility.Visible;
        }

        private void SearchShortestWay(bool mode)
        {
            Graph.Traverse(mode);
            foreach (var currentVertex in Area.LogicCore.Graph.Vertices)
            {
                currentVertex.Routing = "";
                foreach (var item in Area.LogicCore.Graph.Vertices)
                {
                    if (currentVertex != item)
                    {
                        var shortest_path = Graph.shortest_path(currentVertex.ToString(), item.ToString());

                        if (shortest_path != null)
                        {
                            shortest_path.Add(currentVertex.ToString());
                            shortest_path.Reverse();

                            foreach (var node in shortest_path)
                            {
                                if (node != shortest_path.Last())
                                {
                                    currentVertex.Routing += node + " -> ";
                                }
                                else
                                {
                                    currentVertex.Routing += node + Environment.NewLine;
                                }
                            }
                        }
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

            //Providing proped edge highlight by canal properties
            foreach (var item in Area.EdgesList.Keys)
            {
                if (item.IsSatelite)
                    Area.EdgesList[item].Foreground = Brushes.Green;
                if (item.IsDuplex)
                    Area.EdgesList[item].DashStyle = EdgeDashStyle.Dash;
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
                var newVertex = new DataVertex(VertexTextBox.Text);
                logicCore.Graph.AddVertex(newVertex);
                Graph = (NetworkGraph)logicCore.Graph;
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
                            if (isDuplexCheck.IsChecked.Value)
                            {
                                var newEdge = new DataEdge(_selectedVertex, ver.First()) { Text = EdgeTextBox.Text, Weight = weight, IsSatelite = true, IsDuplex = true };
                                _selectedVertex.Edges.Add(newEdge);
                                ver.First().Edges.Add(newEdge);
                                logicCore.Graph.AddEdge(newEdge);
                                Graph = (NetworkGraph)logicCore.Graph;
                            }
                            else
                            {
                                var newEdge = new DataEdge(_selectedVertex, ver.First()) { Text = EdgeTextBox.Text, Weight = weight, IsSatelite = true, IsDuplex = false };
                                _selectedVertex.Edges.Add(newEdge);
                                ver.First().Edges.Add(newEdge);
                                logicCore.Graph.AddEdge(newEdge);
                                Graph = (NetworkGraph)logicCore.Graph;
                            }
                        }
                        else
                        {
                            if (isDuplexCheck.IsChecked.Value)
                            {
                                var newEdge = new DataEdge(_selectedVertex, ver.First()) { Text = EdgeTextBox.Text, Weight = weight, IsSatelite = false,IsDuplex = true };
                                _selectedVertex.Edges.Add(newEdge);
                                ver.First().Edges.Add(newEdge);
                                logicCore.Graph.AddEdge(newEdge);
                                Graph = (NetworkGraph)logicCore.Graph;
                            }
                            else
                            {
                                var newEdge = new DataEdge(_selectedVertex, ver.First()) { Text = EdgeTextBox.Text, Weight = weight, IsSatelite = false, IsDuplex = false };
                                _selectedVertex.Edges.Add(newEdge);
                                ver.First().Edges.Add(newEdge);
                                logicCore.Graph.AddEdge(newEdge);

                                Graph = (NetworkGraph)logicCore.Graph;
                            }
                        }

                        btnApply_Click(null, null);
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

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            if (routingModeBox.SelectedItem.ToString() == "By hopes")
                SearchShortestWay(true);
            else
                SearchShortestWay(false);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            VertexBox.Visibility = Visibility.Collapsed;
        }

        private void btnCloseBox_Click(object sender, RoutedEventArgs e)
        {
            EdgeBox.Visibility = Visibility.Collapsed;
        }

        private void btnChangeEdgeState_Click(object sender, RoutedEventArgs e)
        {

            if (_selectedEdge.IsEnabled)
            {
                _selectedEdgeEvent.EdgeControl.DashStyle = EdgeDashStyle.Dash;
                _selectedEdge.IsEnabled = false;
            }
            else
            {
                _selectedEdgeEvent.EdgeControl.DashStyle = EdgeDashStyle.Solid;
                _selectedEdge.IsEnabled = true;
            }
            Graph = (NetworkGraph)logicCore.Graph;
            btnApply_Click(null, null);
            EdgeBox.Visibility = Visibility.Collapsed;
        }
    }
}
