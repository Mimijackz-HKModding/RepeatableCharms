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
    internal class StrengthFloatMultiply : FsmStateAction
    {
        public FsmFloat floatVariable;

        public FsmFloat multiplyBy;

        public override void Reset()
        {
            floatVariable = null;
            multiplyBy = null;
        }

        public override void OnEnter()
        {
            floatVariable.Value *= Mathf.Pow(multiplyBy.Value, (RepeatableCharmsMod.Instance.Charms[25] as Strength).charmAmount);

            Finish();
        }

        public override void OnUpdate()
        {
            floatVariable.Value *= multiplyBy.Value;
        }
    }
    internal class Strength : CharmRepeat
    {
        public new int charmID = 25;

        public int charmAmount { get; private set; }
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_25 = true;

            charmAmount = charms[25];
        }

        public Strength() : base()
        {
            //Log("Initiating");
            On.PlayMakerFSM.OnEnable += FSMEnable;
        }

        private void FSMEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);
            if (self.FsmName == "Set Slash Damage")
            {
                FloatMultiply prevAction = self.FsmStates[4].Actions[2] as FloatMultiply; //404 needs to be changed to whatever the state of "glass attack modifier" is

                StrengthFloatMultiply newAction = new StrengthFloatMultiply();
                newAction.floatVariable = prevAction.floatVariable;
                newAction.multiplyBy = prevAction.multiplyBy;
                newAction.Init(self.FsmStates[4]);

                self.FsmStates[4].Actions[2] = newAction;
            }
        }
    }
}
