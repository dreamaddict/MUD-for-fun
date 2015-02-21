using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SuperCoolRPG2
{
    public abstract class Player :IPlayer, INotifyPropertyChanged
    {

        public int level { get { return exp / 1; } set { level = 1; } }

        public List<Item> Inventory = new List<Item>();

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }

        public abstract string ClassString { get; }

        public Location CurrentLocation { get; set; }

        public Weapon EquippedWeapon { get; set; }

        public int exp { get { return exp; } set { exp = value; NotifyPropertyChanged(); } }

        private void NotifyPropertyChanged([CallerMemberName] string property = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(property));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
