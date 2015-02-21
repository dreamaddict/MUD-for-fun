using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;

namespace SuperCoolRPG2
{

    /// <summary>
    /// Interaction logic for CharCreation.xaml
    /// </summary>
    public partial class CharCreation : Window
    {

        Player _player;

        public CharCreation()
        {
            Console.WriteLine("Please enter a name /n Name: ");
            _player.Name = Console.ReadLine();
            Console.WriteLine("Please enter a class (Warrior, Mage) /n Class: ");
            getCharClass(Console.ReadLine());

        }

        public void getCharClass(string input)
        {
            switch (input)
            {
                case "Warrior":
                    _player = new Warrior();
                    break;
                case "Mage":
                    _player = new Mage();
                    break;
            }
        }

    }
}
