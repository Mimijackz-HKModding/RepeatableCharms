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

        //const float cooldownDecrease = (float)4 / 6;
        const float cooldownDecrease = (1f / 0.4f) - (1f / 0.6f);
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_31 = true;
            //controller.DASH_COOLDOWN_CH = Mathf.Pow(cooldownDecrease, charms[31]) * 0.6f;
            controller.DASH_COOLDOWN_CH = 1f / ((cooldownDecrease * charms[31]) + (1f / 0.6f));
        }
    }
}
