using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworksProject.Data
{
    public class Network
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public NetworkType NetworkType { get; set; }

        public ObservableCollection<Network> SubNetworks
        {
            get
            {
                if (NetworkType == NetworkType.Global)
                    return SubNetworks;
                else
                    return null;
            }
            set
            {
                SubNetworks = value;
            }
        }

        private ObservableCollection<Network> _subNetworks;



        public Network(NetworkType networkType)
        {

        }

    }

    public enum NetworkType
    {
        Local, Global
    }
}
