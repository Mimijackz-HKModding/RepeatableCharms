using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace RepeatableCharms.Charms
{
    internal class DeepFocus : CharmRepeat
    {
        public new int charmID = 34;

        private FsmFloat speedMultiplier = 1.65f;
        private FsmInt maskIncrease = 2;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_34 = true;

            speedMultiplier.Value = (charms[34] * 0.65f) + 1f;
            maskIncrease.Value = charms[34] + 1;
        }

        public DeepFocus() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
        }

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "Spell Control")
            {
                (self.FsmStates[61].Actions[1] as FloatMultiply).multiplyBy = speedMultiplier; //speed
                (self.FsmStates[60].Actions[2] as SetIntValue).intValue = maskIncrease; //increase
            }
        }
    }
}
