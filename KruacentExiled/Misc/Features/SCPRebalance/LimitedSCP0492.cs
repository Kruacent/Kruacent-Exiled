using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp049;
using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KE.Utils.API.Features;
using KE.Utils.API.Interfaces;
using KE.Utils.API.Translations;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.CR.CustomSCPs.SCP049C;
using PlayerRoles;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KruacentExiled.Misc.Features.SCPRebalance
{
    public class LimitedSCP0492 : MiscFeature
    {

        public const string TooMuchZombies = "TranslationTooMuchZombie";
        public const string TranslationId = "Misc";
        public LimitedSCP0492()
        {

            Dictionary<string, Dictionary<string, string>> langToKeyToText = new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TooMuchZombies] = "Too much zombies! %CurrZombies%/%MaxZombies%",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TooMuchZombies] = "Trop de zombie! %CurrZombies%/%MaxZombies%",
                },
            };


            TranslationHub.Add(TranslationId, langToKeyToText);

        }

        public override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Scp049.StartingRecall += OnStartingRecall;
        }

        public override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Scp049.StartingRecall -= OnStartingRecall;
        }

        private int CurrentAliveZombies => Player.Enumerable.Count(p => p.Role == RoleTypeId.Scp0492);
        private int CurrentAlive049 => Player.Enumerable.Count(p => p.Role == RoleTypeId.Scp049);


        private static TooMuchZombiePosition Position = new TooMuchZombiePosition();
        private void OnStartingRecall(StartingRecallEventArgs ev)
        {
            KELog.Debug("recal");
            Player player = ev.Player;
            if (KECustomRole.Get<SCP049CRole>().Check(player))
            {
                return;
            }




            if (!ev.IsAllowed) return;

            int number049 = CurrentAlive049;
            int numberZombie = CurrentAliveZombies;

            int maxZombie = GetMaxZombie(number049);

            if(HasMaxZombie(numberZombie, number049))
            {
                ev.IsAllowed = false;
                string msg = TranslationHub.Get(player, TranslationId, TooMuchZombies)
                    .Replace("%CurrZombies%", numberZombie.ToString()).Replace("%MaxZombies%", maxZombie.ToString());
                DisplayHandler.Instance.AddHint(Position.HintPlacement, player, msg, 3);
            }


        }


        public static bool HasMaxZombie(int numberZombie, int number049)
        {
            return GetMaxZombie(number049) <= numberZombie;
        }


        public static int GetMaxZombie(int number049)
        {
            return number049 * GetZombiePer049();
        }

        public static int GetZombiePer049()
        {
            int result = Player.ConnectedCount / 3;
            if (result < 2)
            {
                result = 2;
            }

            return result;
        }

    }

    public class TooMuchZombiePosition : HintPosition
    {
        public override float Xposition => 0;

        public override float Yposition => 500;

        public override HintAlignment HintAlignment => HintAlignment.Center;
        public override string Name => "TooMuchZombies";
    }
}
