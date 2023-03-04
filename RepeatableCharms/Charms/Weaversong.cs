using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace RepeatableCharms.Charms
{
    internal class Weaversong : CharmRepeat
    {
        public new int charmID = 39;

        FsmFloat speedMulti = 1.5f;
        FsmInt soulGain = 3;
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_39 = true;

            speedMulti.Value = 1 + (0.5f * charms[37]);
            soulGain.Value = 3 * charms[3];

            PlayMakerFSM controlFSM = controller.transform.GetChild(13).gameObject.LocateMyFSM("Weaverling Control");

            FsmState spawnState = controlFSM.FsmStates[3];
            SpawnObjectFromGlobalPool spawnAction = spawnState.Actions[4] as SpawnObjectFromGlobalPool;

            List<FsmStateAction> newActions = new List<FsmStateAction>();

            for (int i = 0; i < 6; i++)
            {
                newActions.Add(spawnState.Actions[i]);
            }

            for (int i = 1; i < charms[39]; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    newActions.Add(new SpawnObjectFromGlobalPool { 
                        Fsm = spawnAction.Fsm,
                        gameObject = spawnAction.gameObject,
                        spawnPoint = spawnAction.spawnPoint,
                        position = spawnAction.position,
                        rotation = spawnAction.rotation,
                        storeObject = spawnAction.storeObject,
                        State = spawnAction.State,
                        Owner = spawnAction.Owner,
                    });
                }
            }

            spawnState.Actions = newActions.ToArray();

        }
        
        public Weaversong() : base()
        {
            On.PlayMakerFSM.OnEnable += FsmEnable;
        }

        private void FsmEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "Control" && self.name.StartsWith("Weaverling"))
            {
                (self.FsmStates[30].Actions[2] as SetFloatValue).floatValue = speedMulti;
            } else if (self.FsmName == "Attack" && self.transform.parent.name.StartsWith("Weaverling")) {
                (self.FsmStates[10].Actions[1] as CallMethodProper).parameters[0] = new FsmVar() { Type = VariableType.Int, intValue = 3, NamedVar = soulGain };
            }
        }
    }
}
