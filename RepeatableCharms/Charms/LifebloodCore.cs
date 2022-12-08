using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Modding;

namespace RepeatableCharms.Charms
{
    internal class LifebloodCore : CharmRepeat
    {
        public new int charmID = 9;

        int charmAmount = 0;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_9 = true;

            charmAmount = charms[9];
        }

        public LifebloodCore() : base()
        {
            On.PlayerData.UpdateBlueHealth += UpdateBlueHealth;
        }
        private void UpdateBlueHealth(On.PlayerData.orig_UpdateBlueHealth orig, PlayerData self)
        {
            orig(self);

            self.SetInt("healthBlue", self.GetInt("healthBlue") + BlueHealthHookCore());
        }

        private int BlueHealthHookCore()
        {
            if (!PlayerData.instance.equippedCharm_9) return 0;

            return (charmAmount * 4) - 4;
        }
    }
}
