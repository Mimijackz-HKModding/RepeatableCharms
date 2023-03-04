using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RepeatableCharms.Charms
{
    internal class StalwartShell : CharmRepeat
    {
        public new int charmID = 4;

        //const float recoilDecrease = 0.08f / 0.2f;
        const float recoilDecrease = ((1 / 0.08f) - (1 / 0.2f));
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_4 = true;

            controller.INVUL_TIME_STAL = 1.3f + (charms[4] * 0.45f);
            //controller.RECOIL_DURATION_STAL = 0.2f * Mathf.Pow(recoilDecrease, charms[4]);
            controller.RECOIL_DURATION_STAL = 1 / (recoilDecrease * charms[4] + (1 / 0.2f));
        }
    }
}
