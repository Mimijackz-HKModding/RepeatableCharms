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
            new DefendersCrest(), // MISSING SYNERGYS
            new Flukenest(), // MISSING SYNERGYS
            null, // thorns of agony
            new MarkOfPride(), // MISSING SYNERGYS
            null, // steady body
            new HeavyBlow(),
            new SharpShadow(),
            new SporeShroom(),
            new Longnail(),
            null, // shaman stone
            new SoulCatcher(),
            new SoulEater(),
            new GlowingWomb(), // MISSING SYNERGYS
            new Heart(), // fragile and unbreakable
            new Greed(), // fragile and unbreakable
            new Strength(), // fragile and unbreakable
            new Nailmasters(),
            new JonisBlessing(), // MISSING HIVEBLOOD SYNERGY
            null, // shape of unn
            new Hiveblood(), // MISSING JONIS BLESSING SYNERGY
            new DreamWielder(), // MISSING SYNERGYS
            new Dashmaster(),
            new Quickslash(),
            new SpellTwister(),
            new DeepFocus(), // MISSING SPORE SHROOM SYNERGY
            null, // grubberflys elegy
            new Kingsoul(), // MISSING VOIDHEART
            new Sprintmaster(),
            new Dreamshield(), // MISSING DREAMWIELDER SYNERGY
            new Weaversong(),
            new Grimmchild(), // and carefree melody
        };
        private int[] prevCharms = new int[41];
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
                if (Charms[i] == null) continue;
                if (charmEquipped[i] == 0)
                {
                    if (prevCharms[i] != 0) Charms[i].Unequip(data, controller, charmEquipped);

                    continue;
                }
                //Log($"found possible charm: {i}, which is equipped {charmEquipped[i]} times");
                Charms[i].OnCharm(data, controller, charmEquipped);
            }

            prevCharms = charmEquipped;
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

        public static int GetCharmAmount(int id)
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
