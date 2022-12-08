using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepeatableCharms.Charms
{
    internal class BaldurShell : CharmRepeat
    {
        public new int charmID = 5;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_5 = true;

            data.blockerHits = charms[5] * 3;
        }
    }
}
