using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modding;

namespace RepeatableCharms.Charms
{
    internal class SoulEater : CharmRepeat
    {
        public new int charmID = 21;

        private int charmAmount = 0;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_21 = true;

            charmAmount = charms[21];
        }

        public SoulEater() : base()
        {
            ModHooks.SoulGainHook += SoulGainHook;
        }

        private int SoulGainHook(int amount)
        {
            PlayerData playerData = PlayerData.instance;

            if (!playerData.equippedCharm_21) return amount;

            if (playerData.GetInt("MPCharge") < playerData.GetInt("maxMP"))
            {
                amount -= 8;
                amount += 8 * charmAmount;
            }else
            {
                amount -= 6;
                amount += 6 * charmAmount;
            }

            return amount;
        }
    }
}
