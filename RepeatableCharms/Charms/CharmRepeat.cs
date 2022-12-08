using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepeatableCharms.Charms
{
    public abstract class CharmRepeat
    {
        public int charmID;
        public bool enabled = true;
        public abstract void OnCharm(PlayerData data, HeroController controller, int[] charms);
        public void Log(object message)
        {
            RepeatableCharmsMod.Instance?.Log(message);
        } 

    }
}
