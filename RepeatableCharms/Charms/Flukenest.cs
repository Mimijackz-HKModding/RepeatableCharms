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
    internal class Flukenest : CharmRepeat
    {
        public new int charmID = 11;

        private FsmInt flukeBlackAmount = 16, flukeAmount = 9;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_11 = true;

            flukeAmount.Value = charms[11] * 9;
            flukeBlackAmount.Value = charms[11] * 16;
        }
        
        public Flukenest() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
        }

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "Fireball Cast")
            {
                FsmInt amount = self.name.StartsWith("Fireball2") ? flukeBlackAmount : flukeAmount;

                (self.FsmStates[5].Actions[0] as FlingObjectsFromGlobalPool).spawnMin = amount;
                (self.FsmStates[5].Actions[0] as FlingObjectsFromGlobalPool).spawnMax = amount;
            }
        }
    }
}
