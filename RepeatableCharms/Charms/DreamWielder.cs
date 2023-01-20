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
    internal class DreamWielder : CharmRepeat
    {
        public new int charmID = 30;

        private const float skippedTime = 0.5833f;
        private const float anticTime = 0.4167f;
        private const float timeDecrease = skippedTime / (skippedTime + anticTime);

        private FsmFloat newAnticTime = anticTime;
        private bool dreamImpactReceiving = false;
        private int charmAmount = 0;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_30 = true;

            charmAmount = charms[30];
            newAnticTime.Value = anticTime * Mathf.Pow(timeDecrease, charms[30] - 1);
        }
        
        public DreamWielder() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
            On.EnemyDreamnailReaction.RecieveDreamImpact += RecieveDreamImpact;
            On.HeroController.AddMPCharge += AddMPCharge;
        }

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
