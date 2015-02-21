using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCoolRPG2
{
    public class Location
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Location NorthLocation { get; set; }
        public Location SouthLocation { get; set; }
        public Location WestLocation { get; set; }
        public Location EastLocation { get; set; }
        public List<Monster> AreaMonsterList = new List<Monster>();

        public Location(int id, string name, string desc)
        {
            ID = id;
            Name = name;
            Description = desc;
        }
    }

}
