///////////////////////////////////////////////////////////
//  Character.cs
//  Implementation of the Class Character
//  Generated by Enterprise Architect
//  Created on:      28-Apr-2014 10:13:36 PM
//  Original author: Gsimmons
///////////////////////////////////////////////////////////

using System;

namespace Mazegame.Entity
{
    public class Character
    {
        public Dice Dice { get; set; }
        public Party Party { get; set; }
        public Item Item { get; set; }
        public Shield Shield { get; set; }
        public Weapon Weapon { get; set; }
        public Armor Armor { get; set; }

        public Character(String name, int agility, int lifePoints, int strength)
        {
            Name = name;
            Agility = agility;
            LifePoints = lifePoints;
            Strength = strength;
        }

        public int Agility { get; private set; }
        public int LifePoints { get; private set; }
        public String Name { get; private set; }
        public int Strength { get; private set; }


    } //end Character
} //end namespace Entity