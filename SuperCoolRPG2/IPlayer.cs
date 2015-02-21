using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCoolRPG2
{
    public interface IPlayer
    {

        string Name { get; set; }

        string ClassString { get; }

        int Exp { get; set; }

    }
}
