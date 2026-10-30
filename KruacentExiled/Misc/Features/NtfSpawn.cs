using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs.Server;
using KruacentExiled.Audio;
using System.Linq;
using System.Collections.Generic;

namespace KruacentExiled.Misc.Features
{
    public class NtfSpawn
    {
        public void RegisterEvents()
        {

            Exiled.Events.Handlers.Server.RespawnedTeam += MtfRespawn;
        }

        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.RespawnedTeam -= MtfRespawn;
        }

        public const string SpawnMusic = "ntf_spawn";


        public void MtfRespawn(RespawnedTeamEventArgs ev)
        {
            SpawnableFaction faction = ev.Wave.GetSpawnableFaction();
            if(faction == SpawnableFaction.NtfWave || faction == SpawnableFaction.NtfMiniWave)
            {
                List<Player> playerInSameFaction = Player.List.Where(p => p.Role.Side == Side.Mtf).ToList();

                foreach (Player p in playerInSameFaction)
                {
                    AudioHandler.Instance.Play(p, SoundType.Music, SpawnMusic);
                }
            }
        }
    }
}