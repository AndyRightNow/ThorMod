﻿using GTA;
using System;
using System.Windows.Forms;

namespace CaptainAmerica
{
    public class CaptainAmerica : Script
    {
        private int previousPedHash;
        private bool abilityHasBeenTurnedOff;
        private Ability ability;

        public CaptainAmerica()
        {
            Tick += OnTick;
            Interval = 0;
            ability = Ability.Instance;
            abilityHasBeenTurnedOff = true;
            previousPedHash = -1;
        }

        void OnTick(object sender, EventArgs e)
        {
            HandleAbilityToggle();
            if (!abilityHasBeenTurnedOff)
            {
                HandleAbilityTransfer();
                ability.OnTick();
            }
            else if (ability.IsAttachedToPed)
            {
                ability.RemoveAbility();
            }
        }

        private void HandleAbilityToggle()
        {
            if (Game.IsControlPressed(0, GTA.Control.VehicleSubDescend) &&
               Game.IsControlPressed(0, GTA.Control.ScriptPadDown) &&
               Game.IsKeyPressed(Keys.O))
            {
                abilityHasBeenTurnedOff = false;
                UI.Notify("The Captain America ability has been turned on.");
            }
            else if (Game.IsControlPressed(0, GTA.Control.VehicleSubDescend) &&
               Game.IsControlPressed(0, GTA.Control.ScriptPadDown) &&
               Game.IsControlPressed(0, GTA.Control.VehicleExit))
            {
                abilityHasBeenTurnedOff = true;
                UI.Notify("The Captain America ability has been turned off.");
            }
        }

        private void HandleAbilityTransfer()
        {
            if (previousPedHash != -1 &&
                  Game.Player.Character != null &&
                  Game.Player.Character.GetHashCode() != previousPedHash &&
                  ability.IsAttachedToPed)
            {
                ability.RemoveAbility();
            }

            previousPedHash = Game.Player.Character.GetHashCode();

            if (!ability.IsAttachedToPed)
            {
                ability.ApplyOn(Game.Player.Character);
            }
        }
    }
}
