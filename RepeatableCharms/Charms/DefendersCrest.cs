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
    internal class DefendersCrest : CharmRepeat
    {
        public new int charmID = 10;

        private FsmFloat emitFrequency = 0.75f;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_10 = true;

            emitFrequency.Value = Mathf.Pow(0.75f, charms[10]);
        }
        
        public DefendersCrest() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
        }

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "Control" && self.name == "Dung")
            {
                (self.FsmStates[1].Actions[0] as SpawnObjectFromGlobalPoolOverTime).frequency = emitFrequency;
            }
        }
    }
}
