using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazegame.Entity
{
    public class Gold : Item
    {
        public int CoinsCount { get; set; }
        public override string Description
        {
            get
            {
                return base.Description + $" ({CoinsCount} coins)";
            }

            set
            {
                base.Description = value;
            }
        }
    }
}
