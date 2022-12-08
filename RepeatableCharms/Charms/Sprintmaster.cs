using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepeatableCharms.Charms
{
    internal class Sprintmaster : CharmRepeat
    {
        public new int charmID = 37;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_37 = true;

            controller.RUN_SPEED_CH = charms[37] * 1.7f + 8.3f;
            controller.RUN_SPEED_CH_COMBO = charms[37] * (1.7f + (charms[31] * 1.5f)) + 8.3f; //Dashmaster-Sprintmaster combo
        }
    }
}
