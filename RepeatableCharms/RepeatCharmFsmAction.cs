
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using RepeatableCharms.Charms;
using RepeatableCharms;

namespace RepeatableCharms
{
    public class RepeatCharmFsmAction : PlayerDataBoolTest
    {
        private bool charmRepeating;
        public override void OnEnter()
        {
            CharmRepeat currentCharm = RepeatableCharmsMod.Instance.Charms[base.Fsm.GetFsmInt("Current Item Number").Value];
            bool equippedCharm = PlayerData.instance.GetBool("equippedCharm_" + base.Fsm.GetFsmInt("Current Item Number").Value);
            charmRepeating = currentCharm != null && currentCharm.enabled;

            /*
            RepeatableCharmsMod.Instance.Log(base.Fsm.GetFsmInt("Current Item Number").Value);
            RepeatableCharmsMod.Instance.Log(base.Fsm.Name);
            RepeatableCharmsMod.Instance.Log(charmRepeating);
            */

            if (!charmRepeating && equippedCharm)
            {
                base.Fsm.Event(isTrue);
            }
            else
            {
                base.Fsm.Event(isFalse);
            }

            Finish();
        }
    }
}