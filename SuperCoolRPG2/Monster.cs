using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCoolRPG2
{
    public enum MonsterClass
    {
        Warrior,
        Mage,
    }
    public class Monster
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int Level { get; set; }
        public int XPReward { get; set; }
        public int RewardGold { get; set; }
        public int HP { get; set; }
        MonsterClass MClass { get; set; }

        public Monster(int id, string name, int level, MonsterClass mclass)
        {
            ID = id;
            Name = name;
            Level = level;
            HP = GetHP(level, mclass);
            XPReward = GetXPReward(level);
            RewardGold = GetRewardGold(level);
            MClass = mclass;
        }

        static public int GetXPReward(int level)
        {
            if (level * (int)1.5 <= 0)
            {
                return 1;
            }
            else
            {
                return level * (int)1.5;
            }
        }

        static public int GetRewardGold(int level)
        {
            return level * 2;
        }

        static public int GetHP(int level, MonsterClass mclass)
        {
            switch (mclass)
            {
                case MonsterClass.Warrior:
                    return 3 * level;
                case MonsterClass.Mage:
                    return 1 * level;
                default:
                    return 1;
            }
        }
    }

}
