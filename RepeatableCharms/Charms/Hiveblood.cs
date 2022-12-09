using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HutongGames.PlayMaker;

namespace RepeatableCharms.Charms
{
    internal class Hiveblood : CharmRepeat
    {
        public new int charmID = 29;

        private FsmFloat hiveRegenTime = 5;

        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_29 = true;

            hiveRegenTime.Value = 5f / charms[29];
        }

        public Hiveblood() : base() 
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
        }

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "Hive Health Regen")
            {
                hiveRegenTime = self.FsmVariables.GetFsmFloat("Recover Time");
            }
        }
    }
}
