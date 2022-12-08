using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Modding;

namespace RepeatableCharms.Charms
{
    internal class LifebloodHeart : CharmRepeat
    {
        public new int charmID = 8;

        int charmAmount = 0;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_8 = true;

            charmAmount = charms[8];
        }

        public LifebloodHeart() : base()
        {
            On.PlayerData.UpdateBlueHealth += UpdateBlueHealth;
        }

        private void UpdateBlueHealth(On.PlayerData.orig_UpdateBlueHealth orig, PlayerData self)
        {
            orig(self);

            self.SetInt("healthBlue", self.GetInt("healthBlue") + BlueHealthHookHeart());
        }

        private int BlueHealthHookHeart()
        {
            if (!PlayerData.instance.equippedCharm_8) return 0;

            return (charmAmount * 2) - 2;
        }
    }
}
