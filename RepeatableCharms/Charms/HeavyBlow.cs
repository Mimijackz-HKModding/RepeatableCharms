using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace RepeatableCharms.Charms
{
    internal class HeavyBlow : CharmRepeat
    {
        public new int charmID = 15;

        FsmFloat normalMulti = 1;
        FsmFloat greatSlashMulti = 2;
        FsmFloat sideMulti = 2.5f;

        FsmInt stunDecrease = 1;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_15 = true;

            normalMulti.Value = (charms[15] * 0.75f) + 1;
            greatSlashMulti.Value = (charms[15] * 1f) + 1.5f;
            sideMulti.Value = (charms[15] * 0.5f) + 2;

            stunDecrease.Value = charms[15];
        }

        public HeavyBlow() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
        }

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "Enemy Recoil Up")
            {
                (self.FsmStates[1].Actions[0] as SetFsmFloat).setValue = normalMulti;
                (self.FsmStates[1].Actions[1] as SetFsmFloat).setValue = normalMulti;
                (self.FsmStates[1].Actions[2] as SetFsmFloat).setValue = normalMulti;
                (self.FsmStates[1].Actions[3] as SetFsmFloat).setValue = normalMulti;
                (self.FsmStates[1].Actions[4] as SetFsmFloat).setValue = greatSlashMulti;
                (self.FsmStates[1].Actions[5] as SetFsmFloat).setValue = sideMulti;
                (self.FsmStates[1].Actions[6] as SetFsmFloat).setValue = sideMulti;
            }else if (self.FsmName == "Stun Control")
            {
                (self.FsmStates[10].Actions[1] as IntOperator).integer2 = stunDecrease;
                (self.FsmStates[10].Actions[2] as IntOperator).integer2 = stunDecrease;
            }
        }
    }
}
