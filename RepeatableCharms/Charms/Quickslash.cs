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

        const float cooldownDecrease = (1f / 0.25f) - (1f / 0.41f);
        const float durationDecrease = (1f / 0.28f) - (1f / 0.35f);
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_32 = true;

            //controller.ATTACK_COOLDOWN_TIME_CH = Mathf.Pow(cooldownDecrease, charms[32]) * 0.41f;
            //controller.ATTACK_DURATION_CH = Mathf.Pow(durationDecrease, charms[32]) * 0.35f;
            controller.ATTACK_COOLDOWN_TIME_CH = 1f / ((cooldownDecrease * charms[32]) + (1 / 0.41f));
            controller.ATTACK_DURATION_CH = 1f / ((durationDecrease * charms[32]) + (1 / 0.35f));
        }
    }
}
