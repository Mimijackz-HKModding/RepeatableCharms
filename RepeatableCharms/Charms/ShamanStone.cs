using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace RepeatableCharms.Charms
{
    internal class ShamanStone : CharmRepeat
    {
        public new int charmID = 19;

        private FsmInt fireballDamage = 20;
        private FsmFloat fireballScaleX = 1.3f;
        private FsmFloat fireballScaleY = 1.6f;
        private FsmInt shadeFireballDamage = 40;

        private FsmInt diveFallDamage = 23;
        private FsmInt diveShockDamage = 30;
        private FsmInt shadeShockDamage = 50;

        private FsmInt shriekDamage = 20;
        private FsmInt abyssShriekDamage = 30;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_19 = true;

            fireballDamage.Value = (charms[19] * 5) + 15;
            fireballScaleX.Value = (charms[19] * 0.3f) + 1;
            fireballScaleY.Value = (charms[19] * 0.6f) + 1;
            shadeFireballDamage.Value = (charms[19] * 10) + 30;

            diveFallDamage.Value = (charms[19] * 8) + 15;
            diveShockDamage.Value = (charms[19] * 10) + 20;
            shadeShockDamage.Value = (charms[19] * 20) + 30;

            shriekDamage.Value = (charms[19] * 7) + 13;
            abyssShriekDamage.Value = (charms[19] * 10) + 20;
        }
        public ShamanStone() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
        }

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "Fireball Control")
            {
                if (self.name.Contains("Fireball2 Spiral"))
                {
                    (self.FsmStates[5].Actions[5] as SetFsmInt).setValue = shadeFireballDamage;
                    (self.FsmStates[5].Actions[7] as FloatMultiply).multiplyBy = fireballScaleX;
                    (self.FsmStates[5].Actions[8] as FloatMultiply).multiplyBy = fireballScaleY;
                }
                else
                {
                    (self.FsmStates[10].Actions[4] as SetFsmInt).setValue = fireballDamage;
                    (self.FsmStates[10].Actions[6] as SetScale).x = fireballScaleX;
                    (self.FsmStates[10].Actions[6] as SetScale).y = fireballScaleY;
                }
            }else if (self.FsmName == "Set Damage")
            {
                if (self.name.Contains("Q Fall Damage"))
                {
                    (self.FsmStates[0].Actions[2] as SetFsmInt).setValue = diveFallDamage;
                }else if (self.name.Contains("Hit L"))
                {
                    if (self.transform.parent.name.Contains("Q Slam 2"))
                    {
                        (self.FsmStates[0].Actions[2] as SetFsmInt).setValue = shadeShockDamage;
                    }
                    else
                    {
                        (self.FsmStates[0].Actions[2] as SetFsmInt).setValue = diveShockDamage;
                    }
                }
                else if (self.name.Contains("Hit R"))
                {
                    if (self.transform.parent.name.Contains("Q Slam 2"))
                    {
                        (self.FsmStates[0].Actions[2] as SetFsmInt).setValue = shadeShockDamage;
                    }
                    else
                    {
                        (self.FsmStates[0].Actions[2] as SetFsmInt).setValue = diveShockDamage;
                    }
                }else if (self.transform.parent.name.Contains("Scr Heads"))
                {
                    if (self.transform.parent.name.Contains("Scr Heads 2"))
                    {
                        (self.FsmStates[0].Actions[2] as SetFsmInt).setValue = abyssShriekDamage;
                    }
                    else
                    {
                        (self.FsmStates[0].Actions[2] as SetFsmInt).setValue = shriekDamage;
                    }
                }
            }
        }
    }
}
