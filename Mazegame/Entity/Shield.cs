///////////////////////////////////////////////////////////
//  Shield.cs
//  Implementation of the Class Shield
//  Generated by Enterprise Architect
//  Created on:      28-Apr-2014 10:13:37 PM
//  Original author: Gsimmons
///////////////////////////////////////////////////////////

namespace Mazegame.Entity {
	public class Shield : Item {

		public Shield(){
        }

        public int Bonus { get; set; }

        public override bool Equip(Player player)
        {
            player.Equip(this);
            return true;
        }
    }//end Shield

}//end namespace Entity