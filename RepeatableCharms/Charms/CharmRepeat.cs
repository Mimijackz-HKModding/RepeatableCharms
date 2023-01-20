using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepeatableCharms.Charms
{
    public abstract class CharmRepeat
    {
        public virtual int charmID { get; set; }
        public virtual bool enabled { get; set; } = true;
        public abstract void OnCharm(PlayerData data, HeroController controller, int[] charms);
        public virtual void Unequip(PlayerData data, HeroController controller, int[] charms) { }
        public void Log(object message)
        {
            RepeatableCharmsMod.Instance?.Log(message);
        }

    }
}
