using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp049;
using HintServiceMeow.Core.Enum;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KE.Utils.API.Interfaces;
using KE.Utils.API.Translations;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.CR.CustomSCPs.SCP049C;
using PlayerRoles;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

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

        public const int ZombiePer049 = 2;

        private static TooMuchZombiePosition Position = new TooMuchZombiePosition();
        private void OnStartingRecall(StartingRecallEventArgs ev)
        {
            Log.Info("recal");
            Player player = ev.Player;
            if (KECustomRole.Get<SCP049CRole>().Check(player))
            {
                return;
            }




            if (!ev.IsAllowed) return;
            int maxZombie = ZombiePer049 * CurrentAlive049;

            if(maxZombie < CurrentAliveZombies + 1)
            {
                ev.IsAllowed = false;
                string msg = TranslationHub.Get(player, TranslationId, TooMuchZombies)
                    .Replace("%CurrZombies%", CurrentAliveZombies.ToString()).Replace("%MaxZombies%", maxZombie.ToString());


                DisplayHandler.Instance.AddHint(Position.HintPlacement, player, msg, 3);
            }


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
