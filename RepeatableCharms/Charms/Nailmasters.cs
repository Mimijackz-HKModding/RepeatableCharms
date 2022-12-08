using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RepeatableCharms.Charms
{
    internal class Nailmasters : CharmRepeat
    {
        public new int charmID = 26;

        const float chargeDecrease = (float)75 / 135;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_26 = true;
            controller.NAIL_CHARGE_TIME_CHARM = Mathf.Pow(chargeDecrease, charms[26]) * 1.35f;
        }
    }
}
