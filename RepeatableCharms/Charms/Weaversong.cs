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
        public override void OnCharm(PlayerData data, HeroController controller, int[] charms)
        {
            data.equippedCharm_39 = true;

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
    }
}
