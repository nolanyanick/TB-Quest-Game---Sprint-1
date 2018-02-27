using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB_QuestGame
{
    public class Universe
    {
        private List<IslandLocation> _islandLocation;

        public List<IslandLocation> IslandLocation
        {
            get { return _islandLocation; }
            set { _islandLocation = value; }
        }

    }
}
