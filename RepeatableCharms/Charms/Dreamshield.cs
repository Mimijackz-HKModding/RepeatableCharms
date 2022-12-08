using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RepeatableCharms.Charms
{
    internal class Dreamshield : CharmRepeat
    {
        public new int charmID = 38;

        GameObject orbitShield;
        int charmAmount = 0;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_38 = true;

            charmAmount = charms[38];

            if (orbitShield != null) SpawnOrbitShields();
        }

        public Dreamshield() : base()
        {
            On.HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool.OnEnter += SpawnObjectFromGlobalPoolOnEnter;
            On.HutongGames.PlayMaker.Actions.SendEventByName.OnEnter += SendEventByNameEnter;
        }

        private void SendEventByNameEnter(On.HutongGames.PlayMaker.Actions.SendEventByName.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SendEventByName self)
        {
            orig(self);

            if (self.Fsm.Name == "Spawn Orbit Shield")
            {
                Transform shield = self.Fsm.Variables.GetFsmGameObject("Shield").Value.transform;
                for (int i = 2; i < shield.childCount; i++)
                {
                    shield.GetChild(i).gameObject.LocateMyFSM("Shield Hit").SendEvent("SLASH");
                }
            }
        }

        private void SpawnObjectFromGlobalPoolOnEnter(On.HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool self)
        {
            orig(self);

            if (self.Fsm.Name == "Spawn Orbit Shield")
            {
                orbitShield = self.Fsm.Variables.GetFsmGameObject("Shield").Value;
                SpawnOrbitShields();
            }
        }
        void SpawnOrbitShields()
        {
            Transform origShield = orbitShield.transform.GetChild(1);

            for (int i = 2; i < orbitShield.transform.childCount; i++)
            {
                GameObject.Destroy(orbitShield.transform.GetChild(i).gameObject);
            }

            float rotationOffset = 360f / charmAmount;
            for (int i = 1; i < charmAmount; i++)
            {
                GameObject newShield = GameObject.Instantiate(origShield.gameObject, origShield.position, origShield.rotation, orbitShield.transform);
                newShield.transform.localPosition = Quaternion.Euler(0, 0, rotationOffset * i) * newShield.transform.localPosition;
                newShield.transform.Rotate(new Vector3(0, 0, rotationOffset * i));
            }
        }
    }
}
