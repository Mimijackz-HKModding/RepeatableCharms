using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;


namespace RepeatableCharms.Charms
{
    internal class Fury : CharmRepeat
    {
        public new int charmID = 6;

        FsmFloat furyMultiplier = 1.75f;
        FsmFloat elegyMultiplier = 1.5f;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_6 = true;

            furyMultiplier.Value = (0.75f * charms[6]) + 1;
            elegyMultiplier.Value = (0.5f * charms[6]) + 1;
        }

        public Fury() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
        }

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "Fury" && self.gameObject.name == "Charm Effects")
            {
                FsmState ActivateState = self.FsmStates[3];

                (ActivateState.Actions[8] as SetFsmFloat).setValue = furyMultiplier;
                (ActivateState.Actions[9] as SetFsmFloat).setValue = furyMultiplier;
                (ActivateState.Actions[10] as SetFsmFloat).setValue = furyMultiplier;
                (ActivateState.Actions[11] as SetFsmFloat).setValue = furyMultiplier;
                (ActivateState.Actions[12] as SetFsmFloat).setValue = furyMultiplier;
            }
            else if (self.FsmName == "nailart_damage")
            {
                (self.FsmStates[1].Actions[1] as FloatMultiply).multiplyBy = furyMultiplier;
            }
            else if (self.FsmName == "Control" && self.name.StartsWith("Grubberfly Beam") && self.name.Contains(" R"))
            {
                (self.FsmStates[6].Actions[2] as FloatMultiply).multiplyBy = elegyMultiplier;
            }
        }
    }
}
