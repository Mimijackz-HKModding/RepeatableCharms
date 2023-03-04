using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Modding;
using System.Reflection;

namespace RepeatableCharms.Charms
{
    internal class MarkOfPride : CharmRepeat
    {
        public new int charmID = 13;

        private int charmAmount = 0;
        public static int longNailAmount = 0;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_13 = true;

            charmAmount = charms[13];

            typeof(HeroController).GetField("MANTIS_CHARM_SCALE", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(controller, (0.35f * charms[13]) + 1);
        }

        public MarkOfPride() : base()
        {
            On.NailSlash.StartSlash += StartSlash;
        }

        private void StartSlash(On.NailSlash.orig_StartSlash orig, NailSlash self)
        {
            Vector3 origScale = self.scale;

            float scaleDecrease = 1;
            if (charmAmount > 0) scaleDecrease += 0.25f;
            if (longNailAmount > 0) scaleDecrease += 0.15f;
            self.scale.Scale(new Vector3(1 / scaleDecrease, 1 / scaleDecrease, 1));

            float multiplier = ((charmAmount * 0.25f) + (longNailAmount * 0.15f)) + 1f;
            self.scale.Scale(new Vector3(multiplier, multiplier, 1));

            orig(self);
            self.scale = origScale;
        }
    }
}
