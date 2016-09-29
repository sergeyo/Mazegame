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
        private int _maxLifePoints;

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
            _maxLifePoints = lifePoints;
            Strength = strength;
            Party = new Party();
        }

        public int Agility { get; private set; }
        public int LifePoints { get; private set; }
        public string Name { get; private set; }
        public int Strength { get; private set; }

        public int AC
        {
            get { return Agility + (Armor?.Bonus ?? 0) + (Shield?.Bonus ?? 0); }
        }

        public void RecieveDamage(int damage)
        {
            LifePoints -= damage;
        }

        public void Heal(int lifePoints)
        {
            LifePoints += lifePoints;
            if (LifePoints > _maxLifePoints)
            {
                LifePoints = _maxLifePoints;
            }
        }
    }
}