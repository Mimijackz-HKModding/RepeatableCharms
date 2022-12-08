using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Modding;
using System.Text.RegularExpressions;

namespace RepeatableCharms.Charms
{
    internal class Greed : CharmRepeat
    {
        public new int charmID = 23;

        private bool dieFunctionActive = false;
        int charmAmount = 0;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_24 = true;

            if (data.brokenCharm_24) return;

            //Log($"changed charm amount to {charms[24]}");
            charmAmount = charms[24];
        }

        public Greed() : base()
        {
            On.HealthManager.Die += HealthManager_Die;
            On.FlingUtils.SpawnAndFling += SpawnAndFling;
        }

        private GameObject[] SpawnAndFling(On.FlingUtils.orig_SpawnAndFling orig, FlingUtils.Config config, Transform spawnPoint, Vector3 positionOffset)
        {
            if (dieFunctionActive && PlayerData.instance.equippedCharm_24 && !PlayerData.instance.brokenCharm_24)
            {
                //Log($"Got a spawn and fling call with name {config.Prefab.name} and num {config.AmountMin}/{config.AmountMax}");

                config.AmountMin = Mathf.CeilToInt((float)RemoveGreedEffect(config.AmountMin) * (1f + (0.2f * charmAmount)));
                config.AmountMax = Mathf.CeilToInt((float)RemoveGreedEffect(config.AmountMax) * (1f + (0.2f * charmAmount)));

                //Log($"Changed value to {config.AmountMin}/{config.AmountMax}");
            }

            return orig(config, spawnPoint, positionOffset);
        }

        private int RemoveGreedEffect(int geoNum)
        {
            return Mathf.FloorToInt((float)geoNum / 1.2f); //i spent 30 minutes just for this...
        }

        private void HealthManager_Die(On.HealthManager.orig_Die orig, HealthManager self, float? attackDirection, AttackTypes attackType, bool ignoreEvasion)
        {
            dieFunctionActive = true;
            //RepeatableCharmsMod.Instance.Log($"Starting die function");
            orig(self, attackDirection, attackType, ignoreEvasion);
            //RepeatableCharmsMod.Instance.Log($"Ending die function");
            dieFunctionActive = false;
        }
    }
}
