using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace RepeatableCharms.Charms
{
    internal class BaldurShell : CharmRepeat
    {
        public new int charmID = 5;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_5 = true;

            data.blockerHits = (charms[5] * 4) - 1;
        }

        public BaldurShell() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
        }

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "Control" && self.gameObject.name == "Blocker Shield")
            {
                FsmState blockerHitState = self.FsmStates[7];

                IntCompare intCompare = new IntCompare();
                intCompare.integer1 = self.FsmVariables.GetFsmInt("Blocks");
                intCompare.integer2 = 3;
                intCompare.greaterThan = (blockerHitState.Actions[6] as IntSwitch).sendEvent[2];
                intCompare.Init(blockerHitState);

                blockerHitState.Actions = blockerHitState.Actions.Append(intCompare).ToArray();
            }
        }
    }
}
