using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HutongGames.PlayMaker;

namespace RepeatableCharms.Charms
{
    internal class Kingsoul : CharmRepeat
    {
        public new int charmID = 36;

        //private FsmInt kingSoulIncrease = 4;
        private FsmFloat kingSoulWaitTime = 2f;

        private bool i_enabled = true;
        public override bool enabled 
        {
            get
            {
                return i_enabled && PlayerData.instance?.royalCharmState == 3;
            }
            set
            {
                i_enabled = value;
            }
        }
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_36 = true;

            kingSoulWaitTime.Value = 2f / (float)charms[36];
        }

        public Kingsoul() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
        }

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "White Charm")
            {
                //(self.FsmStates[5].Actions[0] as HutongGames.PlayMaker.Actions.SendMessageV2).functionCall.IntParameter = kingSoulIncrease;
                (self.FsmStates[3].Actions[0] as HutongGames.PlayMaker.Actions.Wait).time = kingSoulWaitTime;
            }
        }
    }
}
