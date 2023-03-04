using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HutongGames.PlayMaker;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using UnityEngine;

namespace RepeatableCharms.Charms
{
    internal class GlowingWomb : CharmRepeat
    {
        public new int charmID = 22;

        private FsmInt maxHatchlings = 4;
        private FsmFloat HatchlingSpawnSpeed = 4;

        int charmAmount = 1;
        int furyAmount = 0;
        int dungAmount = 0;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_22 = true;

            maxHatchlings.Value = 4 * charms[22];
            HatchlingSpawnSpeed.Value = 4f / charms[22];

            charmAmount = charms[22];
            furyAmount = charms[6];
        }
        public GlowingWomb() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
            On.KnightHatchling.Explode += HatchlingExplode;
            IL.KnightHatchling.OnEnable += HatchlingEnable;
        }

        private void HatchlingEnable(MonoMod.Cil.ILContext il)
        {
            ILCursor cursor = new ILCursor(il).Goto(0);

            if (cursor.TryGotoNext(
                i => i.MatchLdindI4(),
                i => i.MatchLdcI4(5),
                i => i.MatchAdd()
            ))
            {
                cursor.GotoNext();
                cursor.GotoNext();
                cursor.EmitDelegate<Func<int, int>>(furyDamage);
            }
        }

        private int furyDamage(int prevDam) => 5 * furyAmount;

        private System.Collections.IEnumerator HatchlingExplode(On.KnightHatchling.orig_Explode orig, KnightHatchling self)
        {
            self.dungExplosionPrefab.GetComponent<DamageEffectTicker>().damageInterval = 0.2f / (dungAmount == 0 ? 1 : dungAmount);

            return orig(self);
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
