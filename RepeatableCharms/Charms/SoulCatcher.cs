using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modding;

namespace RepeatableCharms.Charms
{
    internal class SoulCatcher : CharmRepeat
    {
        public new int charmID = 20;

        private int charmAmount = 0;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_20 = true;

            charmAmount = charms[20];
        }

        public SoulCatcher() : base()
        {
            ModHooks.SoulGainHook += SoulGainHook;
        }

        private int SoulGainHook(int amount)
        {
            PlayerData playerData = PlayerData.instance;

            if (!playerData.equippedCharm_20) return amount;
            if (playerData.GetInt("MPCharge") < playerData.GetInt("maxMP"))
            {
                amount -= 3;
                amount += 3 * charmAmount;
            }else
            {
                amount -= 2;
                amount += 2 * charmAmount;
            }
            return amount;
        }
    }
}
