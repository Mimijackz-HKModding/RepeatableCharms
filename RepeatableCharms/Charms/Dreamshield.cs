using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace RepeatableCharms.Charms
{
    internal class Dreamshield : CharmRepeat
    {
        public new int charmID = 38;

        GameObject orbitShield;
        int charmAmount = 0;
        FsmFloat wielderScalePositive = 1.15f;
        FsmFloat wielderScaleNegative = -1.15f;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_38 = true;

            charmAmount = charms[38];

            wielderScaleNegative.Value = -1 + (-0.15f * charms[30]);
            wielderScalePositive.Value = 1 + (0.15f * charms[30]);

            /*for (int i = 0; i < shields.Count; i++)
            {
                if (shields[i] == null)
                {
                    shields.RemoveAt(i);
                    continue;
                }
                shields[i].LocateMyFSM("Shield Hit").SendEvent("CHECK COMBO");
            }*/

            if (orbitShield != null) SpawnOrbitShields();
        }

        public Dreamshield() : base()
        {
            On.HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool.OnEnter += SpawnObjectFromGlobalPoolOnEnter;
            On.HutongGames.PlayMaker.Actions.SendEventByName.OnEnter += SendEventByNameEnter;
            On.PlayMakerFSM.OnEnable += FsmEnable;
        }

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "Shield Hit")
            {
                (self.FsmStates[20].Actions[3] as SetScale).x = wielderScaleNegative;
                (self.FsmStates[20].Actions[3] as SetScale).y = wielderScalePositive;
                (self.FsmStates[20].Actions[4] as SetScale).x = wielderScalePositive;
                (self.FsmStates[20].Actions[4] as SetScale).y = wielderScalePositive;

                //shields.Add(self.gameObject);
            }
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
