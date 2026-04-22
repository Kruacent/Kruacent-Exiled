using Exiled.API.Enums;
using Exiled.API.Features;
using KruacentExiled.Misc;

namespace KruacentExiled.Misc.Handlers
{
    internal class ServerHandler
    {
        public void OnRoundStarted()
        {
            if (MainPlugin.Configs.AutoElevator)
                MainPlugin.Instance.AutoElevator.StartLoop();

            MainPlugin.Instance.AutoTesla.StartLoop();
            Respawn.SetTokens(SpawnableFaction.NtfWave, 1);
            Respawn.SetTokens(SpawnableFaction.ChaosWave, 1);
        }
    }
}
