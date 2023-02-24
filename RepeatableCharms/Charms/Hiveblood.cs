using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace RepeatableCharms.Charms
{
    internal class Hiveblood : CharmRepeat
    {
        public new int charmID = 29;

        private FsmFloat hiveRegenTime = new FsmFloat("Recover Time") { Value = 5f };
        private FsmFloat lifeRegenTime = new FsmFloat("Recover Time") { Value = 10f };

        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_29 = true;

            hiveRegenTime.Value = 5f / charms[29];
            lifeRegenTime.Value = 10f / (charms[29]);
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
                for (int i = 0; i < self.FsmVariables.FloatVariables.Length; i++)
                {
                    if (self.FsmVariables.FloatVariables[i].Name == "Recover Time")
                    {
                        self.FsmVariables.FloatVariables[i] = hiveRegenTime;
                    }
                }
                (self.FsmStates[7].Actions[1] as FloatCompare).float2 = hiveRegenTime;
                (self.FsmStates[8].Actions[3] as FloatCompare).float2 = hiveRegenTime;
            }
            else if (self.FsmName == "blue_health_display")
            {
                bool success = false;
                for (int i = 0; i < self.FsmVariables.FloatVariables.Length; i++)
                {
                    if (self.FsmVariables.FloatVariables[i].Name == "Recover Time")
                    {
                        self.FsmVariables.FloatVariables[i] = lifeRegenTime;
                        success = true;
                    }
                }

                if (success)
                {
                    (self.FsmStates[9].Actions[3] as FloatCompare).float2 = lifeRegenTime;
                    (self.FsmStates[11].Actions[3] as FloatCompare).float2 = lifeRegenTime;
                }
            }
        }
    }
}
