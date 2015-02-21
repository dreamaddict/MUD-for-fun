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

namespace SuperCoolRPG2
{
    /// <summary>
    /// Interaction logic for EquipmentManager.xaml
    /// </summary>
    public partial class EquipmentManager : Window
    {
        Player _player;

        public EquipmentManager(Player player)
        {
            _player = player;

            InitializeComponent();
        }
    }
}
