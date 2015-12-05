using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworksProject.Data
{
    public class Node
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public int Power
        {
            get
            {
                return Nodes.Count;
            }
            set
            {
                Power = value;
            }
        }

        public ObservableCollection<Node> Nodes = new ObservableCollection<Node>();

        public string NetworkName { get; set; }

        public Node()
        {

        }

        public override string ToString()
        {
            return base.ToString();
        }

    }
}
