using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;

namespace RepeatableCharms.Charms
{
    internal class Flukenest : CharmRepeat
    {
        private class DungFlukeSpawn : FsmStateAction
        {
            public FsmGameObject dungFluke;
            public int direction = 1;
            public FsmGameObject spawnPoint;
            public FsmInt charmAmount = 0;
            public override void OnEnter()
            {
                for (int i = 1; i < charmAmount.Value; i++)
                {
                    SpawnFluke();
                }
            }
            public void SpawnFluke()
            {
                float angle = UnityEngine.Random.Range(direction > 0 ? 30f : 140f, direction > 0 ? 40f : 150f);

                GameObject dungClone = dungFluke.Value.Spawn(spawnPoint.Value.transform.position + new Vector3(0, 0.5f, 0), Quaternion.Euler(Vector3.zero));
                dungClone.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle) * 15f, Mathf.Sin(angle) * 15f);
                dungClone.GetComponent<Rigidbody2D>().angularVelocity = direction * -100f;
                dungClone.transform.rotation = Quaternion.Euler(0, 0, direction * 26f);
                dungClone.transform.SetScaleX(direction);

                Finish();
            }
        }

        public new int charmID = 11;

        private FsmInt flukeBlackAmount = 16, flukeAmount = 9;
        private FsmInt charmAmount = 0;
        public FsmInt shamanAmount = 0;
        private FsmFloat dungScale = 1.1f;
        private FsmFloat dungInterval = 0.075f;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_11 = true;

            flukeAmount.Value = charms[11] * 9;
            flukeBlackAmount.Value = charms[11] * 16;

            charmAmount.Value = charms[11];
            shamanAmount.Value = charms[19];

            dungScale.Value = (charms[11] * 0.1f) + 1f;
            dungInterval.Value = 1 / (((1 / 0.075f - 1) * charms[11]) + 1);
        }
        
        public Flukenest() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;

            IL.SpellFluke.OnEnable += SpellFlukeOnEnable;
        }

        private void SpellFlukeOnEnable(MonoMod.Cil.ILContext il)
        {
            ILCursor cursor = new ILCursor(il).Goto(0);

            if (cursor.TryGotoNext(
                i => i.MatchLdcR4(0.9f),
                i => i.MatchLdcR4(1.2f)
            ))
            {
                cursor.GotoNext();
                cursor.EmitDelegate(shamanMinSize);
                cursor.GotoNext();
                cursor.GotoNext();
                cursor.EmitDelegate(shamanMaxSize);
            }

            if (cursor.TryGotoNext(
                i => i.MatchLdarg(0),
                i => i.MatchLdcI4(5)
            ))
            {
                cursor.GotoNext();
                cursor.GotoNext();
                cursor.EmitDelegate(shamanDamage);
            }
        }

        private float shamanMinSize(float actualSize) => 0.7f + (shamanAmount.Value * 0.2f);
        private float shamanMaxSize(float actualSize) => 0.9f + (shamanAmount.Value * 0.3f);
        private int shamanDamage(int actualDam) => 4 + (shamanAmount.Value * 1);

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "Fireball Cast")
            {
                FsmInt amount = self.name.StartsWith("Fireball2") ? flukeBlackAmount : flukeAmount;

                (self.FsmStates[5].Actions[0] as FlingObjectsFromGlobalPool).spawnMin = amount;
                (self.FsmStates[5].Actions[0] as FlingObjectsFromGlobalPool).spawnMax = amount;



                FsmGameObject dungFluke = (self.FsmStates[7].Actions[0] as SpawnObjectFromGlobalPool).gameObject;
                FsmGameObject dungSelf = (self.FsmStates[7].Actions[0] as SpawnObjectFromGlobalPool).spawnPoint;
                DungFlukeSpawn DungSpawnR = new DungFlukeSpawn()
                {
                    dungFluke = dungFluke,
                    spawnPoint = dungSelf,
                    direction = 1,
                    charmAmount = charmAmount
                };
                DungFlukeSpawn DungSpawnL = new DungFlukeSpawn()
                {
                    dungFluke = dungFluke,
                    spawnPoint = dungSelf,
                    direction = -1,
                    charmAmount = charmAmount
                };
                DungSpawnR.Init(self.FsmStates[7]);
                DungSpawnL.Init(self.FsmStates[8]);

                self.FsmStates[7].Actions = Append(self.FsmStates[7].Actions, DungSpawnR, 6);
                self.FsmStates[8].Actions = Append(self.FsmStates[8].Actions, DungSpawnL, 6);
            }else if (self.FsmName == "Control" && self.name.StartsWith("Spell Fluke Dung"))
            {
                (self.FsmStates[4].Actions[0] as RandomFloat).min = dungScale;
                (self.FsmStates[4].Actions[0] as RandomFloat).max = dungScale;
                (self.FsmStates[4].Actions[1] as CallMethodProper).parameters[0].NamedVar = dungInterval;

            }
        }
        private T[] Append<T>(T[] array, T newElement, int maxSize)
        {
            if (array.Length >= maxSize) return array;
            T[] newArr = new T[array.Length + 1];
            for (int i = 0; i < newArr.Length - 1; i++)
            {
                newArr[i] = array[i];
            }
            newArr[array.Length] = newElement;

            return newArr;
        }
    }
}
