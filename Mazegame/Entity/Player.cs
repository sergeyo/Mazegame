///////////////////////////////////////////////////////////
//  Player.cs
//  Implementation of the Class Player
//  Generated by Enterprise Architect
//  Created on:      28-Apr-2014 10:13:37 PM
//  Original author: Gsimmons
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;

namespace Mazegame.Entity {
	public class Player : Character {

		public Location Location { get; set; }
        public Location PreviousLocation { get; set; }

        public List<Item> Backpack { get; set; }
        public int MaxWeight => Strength * 2;

        public int Gold { get; set; }

        public Player(string name, int agility, int lifePoints, int strength) 
            : base(name, agility, lifePoints, strength)
        {
            Backpack = new List<Item>();   
		}

        public int GetCurrentWeight()
        {
            var equippedItems = new Item[] {
                    Weapon,
                    Armor,
                    Shield
                }.Where(item => item != null)
                 .ToList();

            equippedItems.AddRange(Backpack);

            return equippedItems.Sum(item => item.Weight);
        }

        public void Equip(Weapon weapon)
        {
            if (Backpack.Contains(weapon))
            {
                Backpack.Remove(weapon);
            }
            if (Weapon != null)
            {
                AddItemToBackpack(Weapon);
            }
            Weapon = weapon;
        }

        public void Equip(Armor armor)
        {
            if (Backpack.Contains(armor))
            {
                Backpack.Remove(armor);
            }
            if (Armor != null)
            {
                AddItemToBackpack(Armor);
            }
            Armor = armor;
        }

        public void Equip(Shield shield)
        {
            if (Backpack.Contains(shield))
            {
                Backpack.Remove(shield);
            }
            if (Shield != null)
            {
                AddItemToBackpack(Shield);
            }
            Shield = shield;
        }

        public void AddItemToBackpack(Item item)
        {
            if (item is Gold)
            {
                Gold += ((Gold)item).CoinsCount;
                return;
            }
            Backpack.Add(item);
        }
    }
}