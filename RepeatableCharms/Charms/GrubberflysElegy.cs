using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace RepeatableCharms.Charms
{
    internal class GrubberflysElegy : CharmRepeat
    {
        public new int charmID = 35;

        FsmFloat speedMultiplier = 1f;
        int charmAmount = 0;
        FsmFloat beamMultiplier = 0.5f;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_35 = true;

            beamMultiplier.Value = (0.5f * charms[35]);
            speedMultiplier.Value = charms[35];
            charmAmount = charms[35];
        }

        public GrubberflysElegy() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
        }

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "Control" && self.name.StartsWith("Grubberfly Beam"))
            {
                (self.FsmStates[0].Actions[7] as SetVelocity2d).vector.Value = newVelocity((self.FsmStates[0].Actions[7] as SetVelocity2d).vector.Value);
            }else if (self.FsmName == "Set Slash Damage")
            {
                (self.FsmStates[5].Actions[1] as FloatOperator).float2 = beamMultiplier;
            }
        }

        Vector2 newVelocity(Vector2 old) => Vector2.ClampMagnitude(old, 30) * charmAmount;
    }
}
