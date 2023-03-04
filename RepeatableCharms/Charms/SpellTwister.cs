using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Modding;

namespace RepeatableCharms.Charms
{
    internal class SpellTwister : CharmRepeat
    {
        public new int charmID = 33;

        //const float costDecrease = 24f / 33f;
        const float costDecrease = (1f / 24f) - (1f / 33f);
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_33 = true;

            //controller.spellControl.FsmVariables.GetFsmInt("MP Cost").Value = Mathf.RoundToInt(33f * Mathf.Pow(costDecrease, charms[33]));
            controller.spellControl.FsmVariables.GetFsmInt("MP Cost").Value = Mathf.RoundToInt(1f / ((costDecrease * charms[33]) + (1f / 33f)));
            controller.proxyFSM.FsmVariables.GetFsmGameObject("Charm Effects").Value.LocateMyFSM("Set Spell Cost").FsmStates[3].Actions[0].Enabled = false; //this disables the games own way of changing the cost, so we can use our own in the line above
        }
    }
}
