using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;

namespace RepeatableCharms.Charms
{
    internal class DreamWielder : CharmRepeat
    {
        public new int charmID = 30;

        private const float skippedTime = 0.5833f;
        private const float anticTime = 0.4167f;
        //private const float timeDecrease = skippedTime / (skippedTime + anticTime);
        private const float timeDecrease = (1f / skippedTime) - (1f / (skippedTime + anticTime));

        private FsmFloat newAnticTime = anticTime;
        private bool dreamImpactReceiving = false;
        private int charmAmount = 0;

        private const float smallEssenceDecrease = (1 / 40f) - (1 / 60f);
        private const float essenceDecrease = (1 / 200f) - (1 / 400f);
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_30 = true;
            
            charmAmount = charms[30];
            //newAnticTime.Value = anticTime * Mathf.Pow(timeDecrease, charms[30] - 1);
            newAnticTime.Value = 1f / ((timeDecrease * (charms[30] - 1)) + anticTime);
        }
        public override void Unequip(PlayerData data, HeroController controller, int[] charms)
        {
            base.Unequip(data, controller, charms);
            charmAmount = 0;
        }

        public DreamWielder() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
            On.EnemyDreamnailReaction.RecieveDreamImpact += RecieveDreamImpact;
            On.HeroController.AddMPCharge += AddMPCharge;
            IL.EnemyDeathEffects.EmitEssence += EnemyDeathEffects_EmitEssence;
        }

        private void EnemyDeathEffects_EmitEssence(ILContext il)
        {
            ILCursor cursor = new ILCursor(il).Goto(0);

            if (cursor.TryGotoNext
            (
                i => i.MatchLdcI4(200),
                i => i.MatchStloc(3)
            ))
            {
                cursor.Remove();
                cursor.EmitDelegate<Func<int>>(EssenceChance);
            }
            if (cursor.TryGotoNext
            (
                i => i.MatchLdcI4(40),
                i => i.MatchStloc(3)
            ))
            {
                cursor.Remove();
                cursor.EmitDelegate<Func<int>>(ShortEssenceChance);
            }
        }
        private int ShortEssenceChance() => Mathf.RoundToInt(1f / ((smallEssenceDecrease * charmAmount) + (1f / 60f)));
        private int EssenceChance() => Mathf.RoundToInt(1f / ((essenceDecrease * charmAmount) + (1f / 400f)));
        private void AddMPCharge(On.HeroController.orig_AddMPCharge orig, HeroController self, int amount)
        {
            if (dreamImpactReceiving)
            {
                if (charmAmount > 0) amount -= 33;
                amount += 33 * charmAmount;
            }

            orig(self, amount);
        }

        private void RecieveDreamImpact(On.EnemyDreamnailReaction.orig_RecieveDreamImpact orig, EnemyDreamnailReaction self)
        {
            dreamImpactReceiving = true;
            orig(self);
            dreamImpactReceiving = false;
        }

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "Dream Nail" && self.name == "Knight")
            {
                FsmState slashAntic = self.FsmStates[9];

                SendEvent sendEvent = new SendEvent();
                sendEvent.Init(slashAntic);
                sendEvent.everyFrame = false;
                sendEvent.sendEvent = (slashAntic.Actions[0] as Tk2dPlayAnimationWithEvents).animationCompleteEvent;
                sendEvent.eventTarget = (self.FsmStates[20].Actions[3] as SendEventByName).eventTarget;
                sendEvent.delay = newAnticTime;

                slashAntic.Actions = slashAntic.Actions.Append(sendEvent).ToArray();
            }
        }
    }
}
