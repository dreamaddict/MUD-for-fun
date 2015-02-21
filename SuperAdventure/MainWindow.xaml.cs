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
using Engine;

namespace SuperAdventure
{
    public partial class MainWindow : Window
    {
        private Player _player;
        private Monster _currentMonster;

        public MainWindow()
        {
            InitializeComponent();

            _player = new Player(10, 10, 20, 0, 1);

            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            _player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));

            lblHitPoints.Content = _player.CurrentHitPoints.ToString();
            lblGold.Content = _player.Gold.ToString();
            lblExperience.Content = _player.ExperiencePoints.ToString();
            lblLevel.Content = _player.Level.ToString();

            dgvQuests.DataContext = _player.Quests;
            dgvInventory.DataContext = _player.Inventory;
        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
        }

        private void MoveTo(Location newLocation)
        {
            //Does the location have any required items
            if (!_player.HasRequiredItemToEnterThisLocation(newLocation))
            {
                 SendTextToTextBox("You must have a " + newLocation.ItemRequiredToEnter.Name + " to enter this location." + Environment.NewLine);
                return;
            }

            // Update the player's current location
            _player.CurrentLocation = newLocation;

            // Show/hide available movement buttons
            btnNorth.Visibility = (newLocation.LocationToNorth == null ? Visibility.Hidden : Visibility.Visible);

            btnWest.Visibility = (newLocation.LocationToWest == null ? Visibility.Hidden : Visibility.Visible);

            btnEast.Visibility = (newLocation.LocationToEast == null ? Visibility.Hidden : Visibility.Visible);

            btnSouth.Visibility = (newLocation.LocationToSouth == null ? Visibility.Hidden : Visibility.Visible);

            // Display current location name and description
            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text += newLocation.Description + Environment.NewLine;

            // Completely heal the player
            _player.CurrentHitPoints = _player.MaximumHitPoints;

            // Update Hit Points in UI
            lblHitPoints.Content = _player.CurrentHitPoints.ToString();

            // Does the location have a quest?
            if (newLocation.QuestAvailableHere != null)
            {
                // See if the player already has the quest, and if they've completed it
                bool playerAlreadyHasQuest = _player.HasThisQuest(newLocation.QuestAvailableHere);
                bool playerAlreadyCompletedQuest = _player.CompletedThisQuest(newLocation.QuestAvailableHere);

                // See if the player already has the quest
                if (playerAlreadyHasQuest)
                {
                    // If the player has not completed the quest yet
                    if (!playerAlreadyCompletedQuest)
                    {
                        // See if the player has all the items needed to complete the quest
                        bool playerHasAllItemsToCompleteQuest = _player.HasAllQuestCompletionItems(newLocation.QuestAvailableHere);

                        // The player has all items required to complete the quest
                        if (playerHasAllItemsToCompleteQuest)
                        {
                            // Display message
                            SendTextToTextBox(Environment.NewLine +"You complete the '" + newLocation.QuestAvailableHere.Name + "' quest." + Environment.NewLine);

                            // Remove quest items from inventory
                            _player.RemoveQuestCompletionItems(newLocation.QuestAvailableHere);

                            // Give quest rewards
                            SendTextToTextBox("You receive: " + Environment.NewLine
                                +newLocation.QuestAvailableHere.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine
                                    +newLocation.QuestAvailableHere.RewardGold.ToString() + " gold" + Environment.NewLine
                                       +newLocation.QuestAvailableHere.RewardItem.Name + Environment.NewLine);


                            _player.ExperiencePoints += newLocation.QuestAvailableHere.RewardExperiencePoints;
                            _player.Gold += newLocation.QuestAvailableHere.RewardGold;

                            // Add the reward item to the player's inventory
                            _player.AddItemToInventory(newLocation.QuestAvailableHere.RewardItem);

                            // Mark the quest as completed
                            _player.MarkQuestCompleted(newLocation.QuestAvailableHere);
                        }
                    }
                }
                else
                {
                    // The player does not already have the quest

                    // Display the messages
                    SendTextToTextBox("You receive the " + newLocation.QuestAvailableHere.Name + " quest." + Environment.NewLine
                        +newLocation.QuestAvailableHere.Description + Environment.NewLine +"To complete it, return with:" + Environment.NewLine);
                    foreach (QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
                    {
                        if (qci.Quantity == 1)
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.Name + Environment.NewLine;
                        }
                        else
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.NamePlural + Environment.NewLine;
                        }
                    }
                    rtbMessages.Text += Environment.NewLine;

                    // Add the quest to the player's quest list
                    _player.Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
                }
            }

            // Does the location have a monster?
            if (newLocation.MonsterLivingHere != null)
            {
                SendTextToTextBox("You see a " + newLocation.MonsterLivingHere.Name + Environment.NewLine);

                // Make a new monster, using the values from the standard monster in the World.Monster list
                Monster standardMonster = World.MonsterByID(newLocation.MonsterLivingHere.ID);

                _currentMonster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage,
                    standardMonster.RewardExperiencePoints, standardMonster.RewardGold, standardMonster.CurrentHitPoints, standardMonster.MaximumHitPoints);

                foreach (LootItem lootItem in standardMonster.LootTable)
                {
                    _currentMonster.LootTable.Add(lootItem);
                }

                cboWeapons.Visibility = Visibility.Visible;
                cboPotions.Visibility = Visibility.Visible;
                btnUseWeapon.Visibility = Visibility.Visible;
                btnUsePotion.Visibility = Visibility.Visible;
            }
            else
            {
                _currentMonster = null;

                cboWeapons.Visibility = Visibility.Hidden;
                cboPotions.Visibility = Visibility.Hidden;
                btnUseWeapon.Visibility = Visibility.Hidden;
                btnUsePotion.Visibility = Visibility.Hidden;
            }

            // Refresh player's inventory list
            dgvInventory.Items.Refresh();

            // Refresh player's quest list
            dgvQuests.Items.Refresh();

            // Refresh player's weapons combobox
            UpdateWeaponListInUI();

            // Refresh player's potions combobox
            UpdatePotionListInUI();
        }


        private void UpdateWeaponListInUI()
        {
            List<Weapon> weapons = new List<Weapon>();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Details is Weapon)
                {
                    if (inventoryItem.Quantity > 0)
                    {
                        weapons.Add((Weapon)inventoryItem.Details);
                    }
                }
            }

            if (weapons.Count == 0)
            {
                // The player doesn't have any weapons, so hide the weapon combobox and "Use" button
                cboWeapons.Visibility = Visibility.Hidden;
                btnUseWeapon.Visibility = Visibility.Hidden;
            }
            else
            {
                cboWeapons.DataContext = weapons;
                cboWeapons.DisplayMemberPath = "Name";
                cboWeapons.SelectedValuePath = "ID";

                cboWeapons.SelectedIndex = 0;
            }
        }

        private void UpdatePotionListInUI()
        {
            List<HealingPotion> healingPotions = new List<HealingPotion>();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Details is HealingPotion)
                {
                    if (inventoryItem.Quantity > 0)
                    {
                        healingPotions.Add((HealingPotion)inventoryItem.Details);
                    }
                }
            }

            if (healingPotions.Count == 0)
            {
                // The player doesn't have any potions, so hide the potion combobox and "Use" button
                cboPotions.Visibility = Visibility.Hidden;
                btnUsePotion.Visibility = Visibility.Hidden;
            }
            else
            {
                cboPotions.DataContext = healingPotions;
                cboPotions.DisplayMemberPath = "Name";
                cboPotions.SelectedValuePath = "ID";

                cboPotions.SelectedIndex = 0;
            }
        }


        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            // Get the currently selected weapon from the cboWeapons ComboBox
            Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;
            // Determine the amount of damage to do to the monster
            int damageToMonster = RandomNumberGenerator.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);
            // Apply the damage to the monster's CurrentHitPoints
            _currentMonster.CurrentHitPoints -= damageToMonster;
            // Display message
            SendTextToTextBox(Environment.NewLine + "You hit the " + _currentMonster.Name + " for " + damageToMonster.ToString() + " points." + Environment.NewLine);
            // Check if the monster is dead
            if (_currentMonster.CurrentHitPoints <= 0)
            {
                // Monster is dead
                SendTextToTextBox(Environment.NewLine +"You defeated the " + _currentMonster.Name + Environment.NewLine);
                // Give player experience points for killing the monster
                _player.ExperiencePoints += _currentMonster.RewardExperiencePoints;
                SendTextToTextBox("You receive " + _currentMonster.RewardExperiencePoints.ToString() + " experience points" + Environment.NewLine);
                // Give player gold for killing the monster 
                _player.Gold += _currentMonster.RewardGold;
                SendTextToTextBox("You receive " + _currentMonster.RewardGold.ToString() + " gold" + Environment.NewLine);
                // Get random loot items from the monster
                List<InventoryItem> lootedItems = new List<InventoryItem>();
                // Add items to the lootedItems list, comparing a random number to the drop percentage
                foreach (LootItem lootItem in _currentMonster.LootTable)
                {
                    if (RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage)
                    {
                        lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                    }
                }
                // If no items were randomly selected, then add the default loot item(s).
                if (lootedItems.Count == 0)
                {
                    foreach (LootItem lootItem in _currentMonster.LootTable)
                    {
                        if (lootItem.IsDefaultItem)
                        {
                            lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                        }
                    }
                }
                // Add the looted items to the player's inventory
                foreach (InventoryItem inventoryItem in lootedItems)
                {
                    _player.AddItemToInventory(inventoryItem.Details);
                    if (inventoryItem.Quantity == 1)
                    {
                        SendTextToTextBox("You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.Name + Environment.NewLine);
                    }
                    else
                    {
                        SendTextToTextBox("You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.NamePlural + Environment.NewLine);
                    }
                }
                // Refresh player information and inventory controls
                lblHitPoints.Content = _player.CurrentHitPoints.ToString();
                lblGold.Content = _player.Gold.ToString();
                lblExperience.Content = _player.ExperiencePoints.ToString();
                lblLevel.Content = _player.Level.ToString();
                dgvInventory.Items.Refresh();
                UpdateWeaponListInUI();
                UpdatePotionListInUI();
                // Add a blank line to the messages box, just for appearance.
                SendTextToTextBox(Environment.NewLine);
                // Move player to current location (to heal player and create a new monster to fight)
                MoveTo(_player.CurrentLocation);
            }
            else
            {
                // Monster is still alive
                // Determine the amount of damage the monster does to the player
                int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);
                // Display message
                SendTextToTextBox("The " + _currentMonster.Name + " did " + damageToPlayer.ToString() + " points of damage." + Environment.NewLine);
                // Subtract damage from player
                _player.CurrentHitPoints -= damageToPlayer;
                // Refresh player data in UI
                lblHitPoints.Content = _player.CurrentHitPoints.ToString();
                if (_player.CurrentHitPoints <= 0)
                {
                    // Display message
                    SendTextToTextBox("The " + _currentMonster.Name + " killed you." + Environment.NewLine);
                    // Move player to "Home"
                    MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
                }
            }
        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            // Get the currently selected potion from the combobox
            HealingPotion potion = (HealingPotion)cboPotions.SelectedItem;
            // Add healing amount to the player's current hit points
            _player.CurrentHitPoints = (_player.CurrentHitPoints + potion.AmountToHeal);
            // CurrentHitPoints cannot exceed player's MaximumHitPoints
            if (_player.CurrentHitPoints > _player.MaximumHitPoints)
            {
                _player.CurrentHitPoints = _player.MaximumHitPoints;
            }
            // Remove the potion from the player's inventory
            foreach (InventoryItem ii in _player.Inventory)
            {
                if (ii.Details.ID == potion.ID)
                {
                    ii.Quantity--;
                    break;
                }
            }
            // Display message
            SendTextToTextBox("You drink a " + potion.Name + Environment.NewLine);
            // Monster gets their turn to attack
            // Determine the amount of damage the monster does to the player
            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);
            // Display message
            SendTextToTextBox("The " + _currentMonster.Name + " did " + damageToPlayer.ToString() + " points of damage." + Environment.NewLine);
            // Subtract damage from player
            _player.CurrentHitPoints -= damageToPlayer;
            if (_player.CurrentHitPoints <= 0)
            {
                // Display message
                SendTextToTextBox("The " + _currentMonster.Name + " killed you." + Environment.NewLine);
                // Move player to "Home"
                MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            }
            // Refresh player data in UI
            lblHitPoints.Content = _player.CurrentHitPoints.ToString();
            dgvInventory.Items.Refresh();
            UpdatePotionListInUI();
        }

        public void SendTextToTextBox(string text)
        {
            rtbMessages.Text += text;
            rtbMessages.ScrollToEnd();
        }
    }
}