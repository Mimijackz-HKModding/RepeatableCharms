using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMaker;
using UnityEngine;

namespace RepeatableCharms.Charms
{
    internal class SharpShadow : CharmRepeat
    {
        public new int charmID = 16;

        private FsmFloat damageMulti = 1, masterDamageMulti = 1.5f;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_16 = true;

            controller.DASH_SPEED_SHARP = 20 + (8 * charms[16]);
            damageMulti.Value = charms[16];
            masterDamageMulti.Value = 1 + (0.5f * charms[31]);
        }

        public SharpShadow() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
        }

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "Set Sharp Shadow Damage")
            {
                FsmState checkState = self.FsmStates[1];
                FsmStateAction[] oldActions = checkState.Actions;
                FloatMultiply multiAction = new FloatMultiply
                {
                    floatVariable = (oldActions[1] as ConvertIntToFloat).floatVariable,
                    multiplyBy = damageMulti,
                };
                multiAction.Init(checkState);
                checkState.Actions = new FsmStateAction[]
                {
                    oldActions[0],
                    multiAction,
                    oldActions[1],
                    oldActions[2]
                };

                (self.FsmStates[3].Actions[1] as FloatMultiplyV2).multiplyBy = masterDamageMulti;
            }
        }
    }
}
