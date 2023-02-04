using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Modding;

namespace RepeatableCharms.Charms
{
    internal class JonisBlessing : CharmRepeat
    {
        public new int charmID = 27;

        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_27 = true;

            float healthMulti = 1 + (charms[27] * 0.4f);

            if (charms[23] > 0)
            {
                data.SetInt("joniHealthBlue", (int)((float)data.GetInt("maxHealth") * healthMulti));
                data.SetInt("maxHealth", 1);
            }else
            {
                data.SetInt("joniHealthBlue", (int)((float)data.GetInt("maxHealthBase") * healthMulti));
            }
            controller.MaxHealth();
        }
    }
}
