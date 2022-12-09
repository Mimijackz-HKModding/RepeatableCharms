using HutongGames.PlayMaker;
using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RepeatableCharms.Charms;
using System.Linq;

namespace RepeatableCharms
{
    public class RepeatableCharmsMod : Mod
    {
        private static RepeatableCharmsMod _instance;

        internal static RepeatableCharmsMod Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException($"An instance of {nameof(RepeatableCharmsMod)} was never constructed");
                }
                return _instance;
            }
        }

        public FsmBool[] CharmsEnabled 
        {
            get
            {
                return Charms.Select((CharmRepeat) => new FsmBool(CharmEnabled(CharmRepeat))).ToArray();
            }
        }

        public CharmRepeat[] Charms = new CharmRepeat[]
        {
            null, //this is the '0' which is a nonexistent charm, so...
            null, // gathering swarm
            null, // wayward compass
            new Grubsong(),
            new StalwartShell(),
            new BaldurShell(),
            null, // fury of the fallen
            new QuickFocus(),
            new LifebloodHeart(),
            new LifebloodCore(),
            null, // defenders crest
            null, // flukenest
            null, // thorns of agony
            new MarkOfPride(),
            null, // steady body
            null, // heavy blow
            null, // sharp shadow
            null, // spore shroom
            new Longnail(),
            null, // shaman stone
            new SoulCatcher(),
            new SoulEater(),
            null, // glowing womb
            new Heart(),
            new Greed(),
            new Strength(),
            new Nailmasters(),
            null, // jonis blessing
            null, // shape of unn
            new Hiveblood(), // MISSING JONIS BLESSING SYNERGY
            null, // dream wielder
            new Dashmaster(),
            new Quickslash(),
            new SpellTwister(),
            new DeepFocus(), // MISSING SPORE SHROOM SYNERGY
            null, // grubberflys elegy
            null, // kingsoul/void heart
            new Sprintmaster(),
            new Dreamshield(), // MISSING DREAMWIELDER SYNERGY
            new Weaversong(),
            null, // grimmchild/carefree melody
        };
        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public RepeatableCharmsMod() : base("Repeatable Charms")
        {
            _instance = this;
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");

            On.PlayMakerFSM.OnEnable += OnFSM;
            ModHooks.CharmUpdateHook += OnCharm;
            On.PlayerData.CountCharms += NotchCalculation;


            Log("Initialized");
        }


        private void OnCharm(PlayerData data, HeroController controller)
        {
            //Log("new charm list");
            int[] charmEquipped = new int[40 + 1]; //i dont want to do a '-1' for each time you need a charm, so an extra int in memory will do
            foreach (int charm in data.equippedCharms)
            {
                charmEquipped[charm]++;
            }
            /*foreach (int charm in charmEquipped) { Log(charm); }
            Log("data charm list");
            foreach (int charm in data.equippedCharms) { Log(charm); }
            */

            for (int i = 0; i < Charms.Length; i++)
            {
                if (Charms[i] == null || charmEquipped[i] == 0) continue;

                //Log($"found possible charm: {i}, which is equipped {charmEquipped[i]} times");
                Charms[i].OnCharm(data, controller, charmEquipped);
            }

            /*    Moved to inherited classes of CharmRepeat
            //Grubsong
            if (GetCharmAmount(3) > 0)
            {
                data.equippedCharm_3 = true;
                controller.GRUB_SOUL_MP = GetCharmAmount(3) * 15;
            }
            //Quickslash
            if (GetCharmAmount(32) > 0)
            {
                data.equippedCharm_32 = true;
                controller.ATTACK_COOLDOWN_TIME_CH = Mathf.Pow((float)25 / 41, GetCharmAmount(32)) * 0.41f;
                controller.ATTACK_DURATION_CH = Mathf.Pow((float)28 / 35, GetCharmAmount(32)) * 0.35f;
            }
            //Dashmaster
            if (GetCharmAmount(31) > 0)
            {
                data.equippedCharm_31 = true;
                controller.DASH_COOLDOWN_CH = Mathf.Pow((float)4 / 6, GetCharmAmount(31)) * 0.6f;
            }
            //Nailmaster's Glory
            if (GetCharmAmount(26) > 0)
            {
                data.equippedCharm_26 = true;
                controller.NAIL_CHARGE_TIME_CHARM = Mathf.Pow((float)75 / 135, GetCharmAmount(26)) * 1.35f;
            }
            //Sprintmaster
            if (GetCharmAmount(37) > 0)
            {
                data.equippedCharm_37 = true;
                controller.RUN_SPEED_CH = GetCharmAmount(37) * 1.7f + 8.3f;
                controller.RUN_SPEED_CH_COMBO = GetCharmAmount(37) * (1.7f + (GetCharmAmount(31) * 1.5f)) + 8.3f; //Dashmaster-Sprintmaster combo
            }
            */
        }

        //Apparently the vanilla notch calculation calculates repeating charms as 0 cost so i just changed that calculation
        private void NotchCalculation(On.PlayerData.orig_CountCharms orig, PlayerData self)
        {
            orig(self);
            int slots = 0;
            foreach (int charm in self.equippedCharms)
            {

                string charmData = "charmCost_" + charm;
                int cost = self.GetInt(charmData);
                if (cost > 0)
                {
                    slots += cost;
                }
            }
            self.charmSlotsFilled = slots;
        }


        //Making the inventory accept more charms
        private void OnFSM(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.FsmName == "charm_show_if_collected")
            {
                if (!CharmEnabled(Charms[self.FsmVariables.GetFsmInt("ID").Value])) return;

                HutongGames.PlayMaker.Actions.PlayerDataBoolTest playerDataBoolTest = self.FsmStates[1].Actions[3] as HutongGames.PlayMaker.Actions.PlayerDataBoolTest;
                playerDataBoolTest.isTrue = null;
                foreach (HutongGames.PlayMaker.FsmStateAction action in self.FsmStates[3].Actions)
                {
                    action.Enabled = false;
                }
            }
            else if (self.FsmName == "UI Charms")
            {
                HutongGames.PlayMaker.Actions.PlayerDataBoolTest playerDataBoolTest = self.FsmStates[23].Actions[2] as HutongGames.PlayMaker.Actions.PlayerDataBoolTest;
                
                RepeatCharmFsmAction charmFsm = new RepeatCharmFsmAction();
                charmFsm.isTrue = playerDataBoolTest.isTrue;
                charmFsm.isFalse = playerDataBoolTest.isFalse;
                charmFsm.Init(self.FsmStates[23]); //this just applies more variables, no need to look into it

                self.FsmStates[23].Actions[2] = charmFsm;
            }
        }

        [System.Obsolete("Use an inherited class of CharmRepeat instead")]
        public int GetCharmAmount(int id)
        {
            int slots = 0;
            foreach (int charm in PlayerData.instance.equippedCharms)
            {
                if (charm == id) slots++;
            }
            return slots;
        }

        public bool CharmEnabled(CharmRepeat charm)
        {
            return charm != null && charm.enabled;
        }
    }
}
