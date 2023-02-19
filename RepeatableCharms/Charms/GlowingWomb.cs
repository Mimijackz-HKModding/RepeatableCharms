using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HutongGames.PlayMaker;

namespace RepeatableCharms.Charms
{
    internal class GlowingWomb : CharmRepeat
    {
        public new int charmID = 22;

        private FsmInt maxHatchlings = 4;
        private FsmFloat HatchlingSpawnSpeed = 4;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_22 = true;

            maxHatchlings.Value = 4 * charms[22];
            HatchlingSpawnSpeed.Value = 4f / charms[22];
        }
        public GlowingWomb() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
        }

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "Hatchling Spawn")
            {
                maxHatchlings = self.FsmVariables.GetFsmInt("Hatchling Max");
                HatchlingSpawnSpeed = self.FsmVariables.GetFsmFloat("Hatch Time");
            }
        }
    }
}
