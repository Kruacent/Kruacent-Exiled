using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;
using PlayerRoles;
using Respawning.NamingRules;
using System.Collections.Generic;
using System.Linq;

internal class FakeSCPDeath : ICoinEffect
{
    public string Name { get; set; } = "FakeSCPDeath";
    public string Message { get; set; } = "DID YOU JUST KILLED AN SCP !?!?";
    public int Weight { get; set; } = 40;
    public EffectType Type { get; set; } = EffectType.Negative;

    private static readonly Dictionary<string, string> _scpNames = new()
    {
        { "1 7 3", "SCP-173"},
        { "9 3 9", "SCP-939"},
        { "0 9 6", "SCP-096"},
        { "0 7 9", "SCP-079"},
        { "0 4 9", "SCP-049"},
        { "1 0 6", "SCP-106"}
    };

    private static readonly Dictionary<int, string> scpTermination = new()
    {
        { 0, "SUCCESSFULLY TERMINATED BY AUTOMATIC SECURITY SYSTEM" },
        { 1, "SUCCESSFULLY TERMINATED BY ALPHA WARHEAD" },
        { 2, "LOST IN DECONTAMINATION SEQUENCE" },
        { 3, "CONTAINEDSUCCESSFULLY" },
        { 4, "SUCCESSFULLY TERMINATED . TERMINATION CAUSE UNSPECIFIED" }
    };

    public void Execute(Player player)
    {
        var scpName = _scpNames.ToList().RandomItem();
        var scpDeath = scpTermination.GetRandomValue();
        string deathMessage = scpDeath.Value;
        Team deathTeam = Player.List.Where(p => !p.IsScp).GetRandomValue().Role.Team;
        string unitTeam = NineTailedFoxNamingRule.PossibleCodes.GetRandomValue() + "-" + UnityEngine.Random.Range(NineTailedFoxNamingRule.MinUnitNumber, NineTailedFoxNamingRule.MaxUnitNumber); ;

        if (scpDeath.Key == 3)
        {
            deathMessage += " " + Cassie.ConvertTeam(deathTeam, unitTeam);
        }

        Cassie.MessageTranslated($"scp {scpName.Key} successfully terminated by automatic security system",
            $"{scpName.Value} ${deathMessage}.");
    }
}