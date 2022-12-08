using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Modding;

namespace RepeatableCharms.Charms
{
    internal class Dashmaster : CharmRepeat
    {
        public new int charmID = 31;

        const float cooldownDecrease = (float)4 / 6;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_31 = true;
            controller.DASH_COOLDOWN_CH = Mathf.Pow(cooldownDecrease, charms[31]) * 0.6f;
            
        }
    }
}
