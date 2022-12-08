using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RepeatableCharms.Charms
{
    internal class Quickslash : CharmRepeat
    {
        public new int charmID = 32;

        const float cooldownDecrease = (float)25 / 41;
        const float durationDecrease = (float)28 / 35;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_32 = true;
            controller.ATTACK_COOLDOWN_TIME_CH = Mathf.Pow(cooldownDecrease, charms[32]) * 0.41f;
            controller.ATTACK_DURATION_CH = Mathf.Pow(durationDecrease, charms[32]) * 0.35f;
        }
    }
}
