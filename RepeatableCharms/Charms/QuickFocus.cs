using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modding;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace RepeatableCharms.Charms
{
    internal class QuickFocus : CharmRepeat
    {
        public new int charmID = 7;

        const float MPDrainDecrease = (float)18 / 27;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_7 = true;

            controller.spellControl.FsmVariables.GetFsmFloat("Time Per MP Drain CH").Value = Mathf.Pow(MPDrainDecrease, charms[7]) * 0.027f;

            // SLUG!!
            ModifySlugFSM(Array.Find(controller.spellControl.FsmStates, (m) => m.Name == "Slug Speed"), controller.spellControl, charms[7]);
        }

        void ModifySlugFSM(FsmState slugFSM, PlayMakerFSM spellControl, int charmAmount) // slug :)
        {
            float multiplier = charmAmount + 1;

            FloatMultiply multiplyActionL = new FloatMultiply();
            multiplyActionL.multiplyBy = multiplier;
            multiplyActionL.floatVariable = spellControl.FsmVariables.GetFsmFloat("Slug Speed L");

            FloatMultiply multiplyActionR = new FloatMultiply();
            multiplyActionR.multiplyBy = multiplier;
            multiplyActionR.floatVariable = spellControl.FsmVariables.GetFsmFloat("Slug Speed R");

            slugFSM.Actions[6] = multiplyActionL;
            slugFSM.Actions[7] = multiplyActionR;
        }
    }
}
