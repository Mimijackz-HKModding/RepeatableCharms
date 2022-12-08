using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepeatableCharms.Charms
{
    internal class Longnail : CharmRepeat
    {
        public new int charmID = 18;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_18 = true;

            MarkOfPride.longNailAmount = charms[18]; //logic continues in MarkOfPride class
        }
    }
}
