using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using HarmonyLib;
using KruacentExiled;
using MEC;
using UnityEngine;


namespace KruacentExiled.GlobalEventFramework.Examples
{
    internal class MainPlugin : KEPlugin
    {

        public override string Name => "KE.GEF.Examples";
        public override string Prefix => "KE.GEFE";
        public static MainPlugin Instance { get; private set; }

        private static Harmony Harmony;



        public override IConfig Config => config;
        private Config config;
        public static Config Configs => Instance?.config;

        public override void OnEnabled()
        {
            config = KruacentExiled.MainPlugin.Instance.Config.GEFEConfig;
            Instance = this;
            KE.Utils.API.Sounds.SoundPlayer.Load();
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingPlayer;




            //Kannassie.LoadVoiceLine();
            Log.Debug("patching");
            Harmony = new Harmony(Name);
            //Harmony.PatchAll();
        }

        private void OnWaitingPlayer()
        {

        }


        private IEnumerator<float> Showpos()
        {
            while (true)
            {
                Log.Debug(Player.List.First()?.Position);
                yield return Timing.WaitForSeconds(2);
            }
            
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingPlayer;

            Instance = null;
            Harmony?.UnpatchAll();
            Harmony = null;
        }
    }


    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
    }
}
