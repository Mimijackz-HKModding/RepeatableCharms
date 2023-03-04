using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepeatableCharms.Charms
{
    internal class Grubsong : CharmRepeat
    {
        public new int charmID = 3;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_3 = true;
            controller.GRUB_SOUL_MP = charms[3] * 15;
            controller.GRUB_SOUL_MP_COMBO = (charms[3] * 15) + (10 * charms[35]);
        }
    }
}
