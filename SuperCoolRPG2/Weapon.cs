using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCoolRPG2
{
    public enum WeaponTypes
    {
        OneHandBlade,
        Axe,
    }

    public class Weapon : Item
    {
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public WeaponTypes WeaponType { get; set; }

        public Weapon(int id, string name, int lvl, int min, int max, WeaponTypes type)
            :base(id, name, lvl)
        {
            MinDamage = min;
            MaxDamage = max;
            WeaponType = type;
        }
    }

}
