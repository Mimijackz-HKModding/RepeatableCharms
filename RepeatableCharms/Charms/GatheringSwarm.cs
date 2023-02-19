using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using UnityEngine;

namespace RepeatableCharms.Charms
{
    internal class GatheringSwarm : CharmRepeat
    {
        public new int charmID = 1;

        private int charmAmount = 0;
        ILHook hook;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_1 = true;

            charmAmount = charms[1];
        }
        public override void Unequip(PlayerData data, HeroController controller, int[] charms)
        {
            base.Unequip(data, controller, charms);

            charmAmount = 0;
        }

        public GatheringSwarm() : base() // https://gist.github.com/flibber-hk/47bd1e713405fa5936255dd2265c3bb3 this really helped out with getting these ilhooks to work and understanding it
        {
            hook = new ILHook
            (
                typeof(GeoControl).GetMethod("Getter", BindingFlags.NonPublic | BindingFlags.Instance).GetStateMachineTarget(),
                SetSpeed
            );

            IL.GeoControl.FixedUpdate += GeoControl_FixedUpdate;
        }

        private void GeoControl_FixedUpdate(ILContext il)
        {
            ILCursor cursor = new ILCursor(il).Goto(0);

            if (cursor.TryGotoNext
            (
                i => i.MatchLdcR4(150f),
                i => i.MatchMul()
            ))
            {
                cursor.GotoNext();
                cursor.EmitDelegate<Func<float, float>>(speedIncrease);
            }


            if (cursor.TryGotoNext
            (
                i => i.MatchLdcR4(150f),
                i => i.MatchMul()
            ))
            {
                cursor.GotoNext();
                cursor.EmitDelegate<Func<float, float>>(speedIncrease);
            }


            if (cursor.TryGotoNext
            (
                i => i.MatchLdloc(1),
                i => i.MatchLdcR4(20f)
            ))
            {
                cursor.GotoNext();
                cursor.GotoNext();
                cursor.EmitDelegate<Func<float, float>>(speedIncrease);
            }
        }

        private void SetSpeed(ILContext il) 
        {
            ILCursor cursor = new ILCursor(il).Goto(0);

            if (cursor.TryGotoNext
            (
                i => i.MatchLdarg(0),
                i => i.MatchLdcR4(1f),
                i => i.MatchLdcR4(1.7f)
            ))
            {
                cursor.GotoNext();
                cursor.GotoNext();
                cursor.EmitDelegate<Func<float, float>>(speedDecrease);

                cursor.GotoNext();
                cursor.GotoNext();
                cursor.EmitDelegate<Func<float, float>>(speedDecrease);
            }


            if (cursor.TryGotoNext
            (
                i => i.MatchLdarg(0),
                i => i.MatchLdcR4(0.3f),
                i => i.MatchLdcR4(0.5f)
            ))
            {
                cursor.GotoNext();
                cursor.GotoNext();
                cursor.EmitDelegate<Func<float, float>>(speedDecrease);

                cursor.GotoNext();
                cursor.GotoNext();
                cursor.EmitDelegate<Func<float, float>>(speedDecrease);
            }
        }
        private float speedDecrease(float current) => current / charmAmount;
        private float speedIncrease(float current) => current * charmAmount;
    }
}
