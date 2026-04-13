using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;
using MEC;
using UnityEngine;


namespace KE.GlobalEventFramework.Examples
{
    internal class MainPlugin : Plugin<Config>
    {
        public override string Author => "Patrique & OmerGS";

        public override Version Version => new Version(1, 0, 0);
        public override string Name => "KE.GEF.Examples";
        public override string Prefix => "KE.GEFE";
        public static MainPlugin Instance { get; private set; }

        private static Harmony Harmony;
        public override PluginPriority Priority => PluginPriority.Lower;

        

        
        public override void OnEnabled()
        {
            Instance = this;
            Utils.API.Sounds.SoundPlayer.Load();
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingPlayer;




            //Kannassie.LoadVoiceLine();
            Log.Debug("patching");
            Harmony = new(Name);
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
}
