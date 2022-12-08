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
    internal class Heart : CharmRepeat
    {
        public new int charmID = 23;

        private GameObject[] masks = null;
        private GameObject maskGo = null;

        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_23 = true;

            if (data.brokenCharm_23) return;

            //TODO: add more masks to the HUD
            

            controller.playerData.SetInt("maxHealth", controller.playerData.GetInt("maxHealthBase") + (charms[23] * 2));


            if (masks == null)
            {
                masks = GetMaskObjects(GameCameras.instance);
                maskGo = masks[1];
            }

            if (controller.playerData.GetInt("maxHealth") > masks.Length)
            {
                AddMasks(GameCameras.instance, masks.Length + 1, controller.playerData.GetInt("maxHealth") - masks.Length);
            }
            //DoAddMasks(GameCameras.instance, controller.playerData.GetInt("maxHealthBase"));
            foreach(GameObject obj in masks)
            {
                obj.LocateMyFSM("health_display").SendEvent("HERO HEALED FULL");
            }

            controller.MaxHealth();
        }


        // credits to SFGrenade for most of the code below
        GameObject[] GetMaskObjects(GameCameras self)
        {
            Transform maskParent = self.transform.GetChild(0).GetChild(0).GetChild(1);
            GameObject[] maskObjects = new GameObject[0];
            for (int i = 0; i < maskParent.childCount; i++)
            {
                Transform child = maskParent.GetChild(i);
                if (Regex.IsMatch(child.name, @"Health \d+"))
                {
                    Array.Resize(ref maskObjects, maskObjects.Length + 1);
                    maskObjects[maskObjects.Length - 1] = child.gameObject;
                }
            }

            return maskObjects;
        }
        private void AddMasks(GameCameras self, int startIndex, int maskAmount)
        {
            //int totalMaskAmount = 27 * 13;
            for (int i = startIndex; i < maskAmount + startIndex; i++)
            {
                var healthGo = GameObject.Instantiate(maskGo, maskGo.transform.parent);
                SetPositionAndFsm(healthGo, i);

                Array.Resize(ref masks, masks.Length + 1);
                masks[masks.Length - 1] = healthGo;
            }
            //SetHealthPosition(self.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(4).gameObject, totalMaskAmount + 1);
        }
        private void SetPositionAndFsm(GameObject ob, int num)
        {
            ob.name = $"Health {num}";
            var healthFsm = ob.LocateMyFSM("health_display");
            var healthFsmVars = healthFsm.FsmVariables;
            healthFsmVars.GetFsmInt("Health Number").Value = num;
            SetPosition(ob, num);
        }

        private void SetPosition(GameObject ob, int num)
        {
            float xPos = -10.32f + (0.94f * (num - 1))/* + ((row % 2 == 0) ? 0 : (0.94f / 2f))*/;

            ob.transform.localPosition = new Vector3(xPos, 7.7f, -2);
        }
    }
}
