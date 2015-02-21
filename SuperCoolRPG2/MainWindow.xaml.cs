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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SuperCoolRPG2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Player _player;

        


        public MainWindow(Player _player)
        {

            InitializeComponent();

            this._player = _player;
            MoveTo(Game.LocationByID(Game.LOCATION_ID_START));
            _player.Inventory.Add(Game.ItembyID(Game.WEAPON_ID_NEWBIE_SWORD));

            lblName.DataContext = _player;
            lblClass.DataContext = _player;
            lblExperience.DataContext = _player;
        }

        public void MoveTo(Location newLocation)
        {
            _player.CurrentLocation = newLocation;

            btnNorth.Visibility = (newLocation.NorthLocation == null ? Visibility.Hidden : Visibility.Visible);

            btnWest.Visibility = (newLocation.WestLocation == null ? Visibility.Hidden : Visibility.Visible);

            btnEast.Visibility = (newLocation.EastLocation == null ? Visibility.Hidden : Visibility.Visible);

            btnSouth.Visibility = (newLocation.SouthLocation == null ? Visibility.Hidden : Visibility.Visible);

            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text += newLocation.Description + Environment.NewLine;

            Game.GenerateRandomMonster(_player.level, _player.CurrentLocation);

            if (_player.CurrentLocation.AreaMonsterList.Count != 0)
            {
                foreach (Monster monster in _player.CurrentLocation.AreaMonsterList)
                {
                    SendTextToTextBox("A " + monster.Name + " is here." + Environment.NewLine);
                }
            }

            UpdateMonsterListInUI();
        }

        private void btnNorth_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBox();
            MoveTo(_player.CurrentLocation.NorthLocation);
            IncreaseStepCounter();
        }

        private void btnWest_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBox();
            MoveTo(_player.CurrentLocation.WestLocation);
            IncreaseStepCounter();
        }

        private void btnSouth_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBox();
            MoveTo(_player.CurrentLocation.SouthLocation);
            IncreaseStepCounter();
        }

        private void btnEast_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBox();
            MoveTo(_player.CurrentLocation.EastLocation);
            IncreaseStepCounter();
        }

        public void SendTextToTextBox(string text)
        {
            rtbMessages.Text += text;
            rtbMessages.ScrollToEnd();
        }

        public void IncreaseStepCounter()
        {
            ++Game.stepCounter;
        }

        public void ClearTextBox()
        {
            rtbMessages.Text = String.Empty;
        }

        private void UpdateMonsterListInUI()
        {

            if (_player.CurrentLocation.AreaMonsterList.Count == 0)
            {
                // The area doesn't have any monsters, no use showing the stuff.
                cboMonsters.Visibility = Visibility.Hidden;
                btnUseWeapon.Visibility = Visibility.Hidden;
            }
            else
            {
                cboMonsters.Visibility = Visibility.Visible;
                btnUseWeapon.Visibility = Visibility.Visible;
                cboMonsters.DataContext = _player.CurrentLocation.AreaMonsterList;
                cboMonsters.DisplayMemberPath = "Name";
                cboMonsters.SelectedValuePath = "ID";

                cboMonsters.SelectedIndex = 0;
            }
        }

        private void btnUseWeapon_Click(object sender, RoutedEventArgs e)
        {
            int damageToMonster = RNG.NumberBetween(1, 2);

            Monster _currentMonster = (Monster)cboMonsters.SelectedItem;

            _currentMonster.HP -= damageToMonster;

            SendTextToTextBox(Environment.NewLine + "You hit the " + _currentMonster.Name + " for " + damageToMonster.ToString() + " points." + Environment.NewLine);

            if(_currentMonster.HP <= 0)
            {
                ClearTextBox();
                MoveTo(_player.CurrentLocation);

                SendTextToTextBox(Environment.NewLine + "You defeated the " + _currentMonster.Name + Environment.NewLine);

                //reward xp
                _player.exp += _currentMonster.XPReward;

                _player.CurrentLocation.AreaMonsterList.Remove(_currentMonster);
                

                SendTextToTextBox("You receive " + _currentMonster.XPReward.ToString() + " experience points" + Environment.NewLine);

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
