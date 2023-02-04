using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace RepeatableCharms.Charms
{
    internal class SporeShroom : CharmRepeat
    {
        public new int charmID = 17;

        private int charmAmount = 1;
        private int dungAmount = 1;

        private FsmFloat deepScale = 1.35f;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_17 = true;

            deepScale.Value = (0.35f * charms[34]) + 1f;

            charmAmount = charms[17];
            dungAmount = charms[10];
        }
        public SporeShroom() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
        }

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "Control" && self.name.Contains("Knight Spore Cloud"))
            {
                self.gameObject.GetComponent<DamageEffectTicker>().damageInterval = 0.15f / (charmAmount);
                (self.FsmStates[4].Actions[1] as SetScale).x = deepScale;
                (self.FsmStates[4].Actions[1] as SetScale).y = deepScale;
            }
            else if (self.FsmName == "Control" && self.name.Contains("Knight Dung Cloud"))
            {
                self.gameObject.GetComponent<DamageEffectTicker>().damageInterval = 0.2f / (charmAmount) / (dungAmount == 0 ? 1 : dungAmount);
                (self.FsmStates[4].Actions[1] as SetScale).x = deepScale;
                (self.FsmStates[4].Actions[1] as SetScale).y = deepScale;
            }
        }
    }
}
