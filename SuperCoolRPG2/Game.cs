using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCoolRPG2
{
    class Game
    {
        public static readonly List<Location> Locations = new List<Location>();
        public static readonly List<Monster> Monsters = new List<Monster>();
        public static readonly List<Weapon> Weapons = new List<Weapon>();

        public static int stepCounter;

        //Location IDs
        public const int LOCATION_ID_START = 1;
        public const int LOCATION_ID_CASTLE_WOODBERRY = 2;
        public const int LOCATION_ID_FOREST = 3;
        public const int LOCATION_ID_TOWN_WOODBERRY = 4;

        //Monster IDs

        public const int MONSTER_ID_KAPPA = 1;
        public const int MONSTER_ID_ORC = 2;
        public const int MONSTER_ID_DONKEY = 3;

        //Weapon IDs

        public const int WEAPON_ID_NEWBIE_SWORD = 1;

        static Game()
        {

            GenerateMonsters();
            GenerateLocations();
        }

        private static void GenerateWeapons()
        {
            Weapon newbieSword = new Weapon(WEAPON_ID_NEWBIE_SWORD, "Newbie Sword", 1, 2, 4, WeaponTypes.OneHandBlade);
        }

        private static void GenerateMonsters()
        {
            //Create dem monsters.

            Monster kappa = new Monster(MONSTER_ID_KAPPA, "Kappa", 1, MonsterClass.Warrior);
            Monster orc = new Monster(MONSTER_ID_ORC, "Orc", 2, MonsterClass.Warrior);
            Monster donkey = new Monster(MONSTER_ID_DONKEY, "Donkey", 3, MonsterClass.Mage);

            Monsters.Add(kappa);
            Monsters.Add(orc);
            Monsters.Add(donkey);

        }

        private static void GenerateLocations()
        {
            //Create dem locations

            Location start = new Location(LOCATION_ID_START, "Swirling vortex of nothingness", "You are standing in a swirling pool nothingness. Maybe you will acheive great things, maybe you will die, who knows?");

            Location castleWoodberry = new Location(LOCATION_ID_CASTLE_WOODBERRY, "Castle Woodberry", "This is the castle of Woodberry, yep, it's tall and castle like");

            Location forest = new Location(LOCATION_ID_FOREST, "Forest", "This is a forest, it has no name. Why? Probably because the creator of this world is lazy af.");

            Location townWoodberry = new Location(LOCATION_ID_TOWN_WOODBERRY, "Woodberry", "This is Woodberry, not to be confused with the town on Walking Dead. This has absolutely nothing to do with that town...serious.");

            //Link locations together

            start.NorthLocation = castleWoodberry;

            castleWoodberry.SouthLocation = start;
            castleWoodberry.EastLocation = townWoodberry;

            townWoodberry.WestLocation = castleWoodberry;
            townWoodberry.NorthLocation = forest;

            forest.SouthLocation = townWoodberry;

            Locations.Add(start);
            Locations.Add(castleWoodberry);
            Locations.Add(forest);
            Locations.Add(townWoodberry);

        }

        public static Location LocationByID(int id)
        {
            foreach (Location location in Locations)
            {
                if (location.ID == id)
                {
                    return location;
                }
            }

            return null;
        }

        public static Monster MonsterByID(int id)
        {
            foreach (Monster monster in Monsters)
            {
                if (monster.ID == id)
                {
                    return monster;
                }
            }

            return null;
        }

        public static Item ItembyID(int id)
        {
            foreach (Item item in Weapons)
            {
                if (item.ID == id)
                {
                    return item;
                }
            }

            return null;
        }

        public static void GenerateRandomMonster(int level, Location currentLocation)
        {
            if (Game.stepCounter > RNG.NumberBetween(1, 10))
            {
                foreach (Monster monster in Monsters)
                {
                    if (monster.Level == level)
                    {
                        currentLocation.AreaMonsterList.Add(monster);
                        Game.stepCounter = 0;
                    }
                }
            }
        }
    }
}