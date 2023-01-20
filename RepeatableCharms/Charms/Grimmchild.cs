using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using System.Reflection;


namespace RepeatableCharms.Charms
{
    internal class GrimmchildSpawnCustomAction : FsmStateAction
    {
        public FsmGameObject gameObject;

        public FsmGameObject spawnPoint;

        public FsmVector3 position;

        public FsmVector3 rotation;

        public FsmGameObject storeObject;

        public FsmInt charmAmount;

        public List<GameObject> grimms = new List<GameObject>();


        public override void OnEnter()
        {
            grimms.Clear();
            storeObject.Value = SpawnChild(0);

            for (int i = 1; i < charmAmount.Value; i++)
            {
                SpawnChild(i);
            }

            Finish();
        }
        public GameObject SpawnChild(int index)
        {
            GameObject newClone = gameObject.Value.Spawn(spawnPoint.Value.transform.position);
            newClone.transform.Translate(Vector3.forward * (index * 0.01f));
            (newClone.LocateMyFSM("Control").FsmStates[3].Actions[1] as SetFloatValue).floatValue = 2f + (index * 0.5f);
            grimms.Add(newClone);

            return newClone;
        }
    }
    internal class Grimmchild : CharmRepeat
    {
        public new int charmID = 40;

        private bool i_enabled = true;
        public override bool enabled 
        {
            get
            {
                return i_enabled/* && PlayerData.instance?.grimmChildLevel == 5*/;
            }
            set
            {
                i_enabled = value;
            }
        }

        private FsmInt charmAmount = 0;
        private GrimmchildSpawnCustomAction spawnAction = null;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_40 = true;

            //Log($"new checking grimms with count {((spawnAction != null) ? spawnAction.grimms.Count : -1)}, charm amount {charmAmount.Value}, charms {charms[40]}");
            if (data.grimmChildLevel != 5 && spawnAction != null && charms[40] != spawnAction.grimms.Count && spawnAction.grimms.Count != 0 && charms[40] != 0)
            {
                //Log($"checking grimms with count {spawnAction.grimms.Count}, charm amount {charmAmount.Value}, charms {charms[40]}");
                if (charms[40] > spawnAction.grimms.Count)
                {
                    for (int i = spawnAction.grimms.Count; i < charms[40]; i++)
                    {
                        spawnAction.SpawnChild(i);
                    }
                    //Log($"added some grimms with new count {spawnAction.grimms.Count}, charm amount {charmAmount.Value}, charms {charms[40]}");
                }
                else
                {
                    for (int i = charms[40]; i < spawnAction.grimms.Count; i++)
                    {
                        spawnAction.grimms[i].LocateMyFSM("Control").SendEvent("ALL CHARMS END");
                    }
                    spawnAction.grimms.RemoveRange(charms[40], spawnAction.grimms.Count - charms[40]);
                    //Log($"removed some grimms with new count {spawnAction.grimms.Count}, charm amount {charmAmount.Value}, charms {charms[40]}");
                }
            }
            charmAmount.Value = charms[40];
        }

        public Grimmchild() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
            On.HeroController.TakeDamage += TakeDamage;
        }

        private void TakeDamage(On.HeroController.orig_TakeDamage orig, HeroController self, GameObject go, GlobalEnums.CollisionSide damageSide, int damageAmount, int hazardType)
        {
            FieldInfo hitsSinceShielded = self.GetType().GetField("hitsSinceShielded", BindingFlags.Instance | BindingFlags.NonPublic);
            int prevHits = (int)hitsSinceShielded.GetValue(self);
            orig(self, go, damageSide, damageAmount, hazardType);
            int newHits = (int)hitsSinceShielded.GetValue(self);

            if (newHits == 0) return;
            if (newHits > prevHits)
            {
                hitsSinceShielded.SetValue(self, newHits - 1 + (charmAmount.Value));
            }

        }

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "Spawn Grimmchild")
            {
                SpawnObjectFromGlobalPool prevAction = self.FsmStates[3].Actions[2] as SpawnObjectFromGlobalPool;
                GrimmchildSpawnCustomAction action = new GrimmchildSpawnCustomAction()
                {
                    gameObject = prevAction.gameObject,
                    spawnPoint = prevAction.spawnPoint,
                    position = prevAction.position,
                    rotation = prevAction.rotation,
                    storeObject = prevAction.storeObject,
                    charmAmount = this.charmAmount,
                };

                self.FsmStates[3].Actions[2] = action;
                spawnAction = action;
            }
        }
        public override void Unequip(PlayerData data, HeroController controller, int[] charms)
        {
            base.Unequip(data, controller, charms);
            charmAmount.Value = 0;

            if (data.grimmChildLevel != 5)
            {
                if (spawnAction == null) return;
                foreach (GameObject grimm in spawnAction.grimms)
                {
                    grimm.LocateMyFSM("Control").SendEvent("ALL CHARMS END");
                }
                spawnAction.grimms.Clear();
            }else
            {

            }
            
        }
    }
}
