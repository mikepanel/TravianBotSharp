﻿using System;
using System.Windows.Forms;
using TravBotSharp.Files.Helpers;
using TravBotSharp.Files.Models.AccModels;

namespace TravBotSharp.Views
{
    public partial class HeroUc : UserControl
    {
        ControlPanel main;
        public HeroUc()
        {
            InitializeComponent();
        }
        public void UpdateTab()
        {
            var acc = getSelectedAcc();
            buyAdventuresCheckBox.Checked = acc.Hero.Settings.BuyAdventures;
            checkBoxAutoSendToAdventures.Checked = acc.Hero.Settings.AutoSendToAdventure;
            minHeroHealthUpDown.Value = acc.Hero.Settings.MinHealth;
            autoReviveHero.Checked = acc.Hero.Settings.AutoReviveHero;

            var heroUpgrade = acc.Hero.Settings.Upgrades;
            strength.Value = heroUpgrade[0];
            offBonus.Value = heroUpgrade[1];
            deffBonus.Value = heroUpgrade[2];
            resources.Value = heroUpgrade[3];
            autoSetHeroPoints.Checked = acc.Hero.Settings.AutoSetPoints;
            maxDistanceUpDown.Value = acc.Hero.Settings.MaxDistance;
            LimitHeroPoints();

            SupplyResVillageComboBox.Items.Clear();
            foreach (var vill in acc.Villages)
            {
                SupplyResVillageComboBox.Items.Add(vill.Name);
            }
            if (SupplyResVillageComboBox.Items.Count > 0)
            {
                SupplyResVillageComboBox.SelectedIndex = 0;
                SupplyResVillageSelected.Text = "Selected: " + AccountHelper.GetHeroReviveVillage(acc).Name;
            }
        }
        public void Init(ControlPanel _main)
        {
            main = _main;
        }
        private Account getSelectedAcc()
        {
            return main != null ? main.GetSelectedAcc() : null;
        }

        private void checkBoxAutoSendToAdventures_CheckedChanged(object sender, EventArgs e)
        {
            getSelectedAcc().Hero.Settings.AutoSendToAdventure = checkBoxAutoSendToAdventures.Checked;
        }

        private void buyAdventuresCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            getSelectedAcc().Hero.Settings.BuyAdventures = buyAdventuresCheckBox.Checked;
        }

        private void minHeroHealthUpDown_ValueChanged(object sender, EventArgs e)
        {
            getSelectedAcc().Hero.Settings.MinHealth = (int)minHeroHealthUpDown.Value;
        }

        private void strength_ValueChanged(object sender, EventArgs e)
        {
            LimitHeroPoints();
        }

        private void offBonus_ValueChanged(object sender, EventArgs e)
        {
            LimitHeroPoints();
        }

        private void deffBonus_ValueChanged(object sender, EventArgs e)
        {
            LimitHeroPoints();
        }

        private void resources_ValueChanged(object sender, EventArgs e)
        {
            LimitHeroPoints();
        }
        private int HeroPointsUSer()
        {
            int str = (int)strength.Value;
            int off = (int)offBonus.Value;
            int deff = (int)deffBonus.Value;
            int res = (int)resources.Value;
            return str + off + deff + res;
        }
        private void LimitHeroPoints()
        {
            int lockPoints = HeroPointsUSer();
            strength.Maximum = strength.Value + 4 - lockPoints;
            offBonus.Maximum = offBonus.Value + 4 - lockPoints;
            deffBonus.Maximum = deffBonus.Value + 4 - lockPoints;
            resources.Maximum = resources.Value + 4 - lockPoints;
            var acc = getSelectedAcc();
            var vals = new byte[] { (byte)strength.Value, (byte)offBonus.Value, (byte)deffBonus.Value, (byte)resources.Value };
            acc.Hero.Settings.Upgrades = vals;
        }

        private void autoSetHeroPoints_CheckedChanged(object sender, EventArgs e)
        {
            getSelectedAcc().Hero.Settings.AutoSetPoints = autoSetHeroPoints.Checked;
        }

        private void maxDistanceUpDown_ValueChanged(object sender, EventArgs e)
        {
            getSelectedAcc().Hero.Settings.MaxDistance = (int)maxDistanceUpDown.Value;
        }

        private void autoReviveHero_CheckedChanged(object sender, EventArgs e)
        {
            getSelectedAcc().Hero.Settings.AutoReviveHero = autoReviveHero.Checked;
        }

        private void SupplyResourcesButton_Click(object sender, EventArgs e)
        {
            var acc = getSelectedAcc();
            var vill = acc.Villages[SupplyResVillageComboBox.SelectedIndex];
            acc.Hero.ReviveInVillage = vill.Id;
            SupplyResVillageSelected.Text = "Selected: " + vill.Name;
        }
    }
}
