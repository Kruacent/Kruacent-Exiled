namespace KruacentExiled.CustomRoles.API.Interfaces
{
    public enum LuckProfile
    {
        UltimateLucky,
        UltimateUnlucky,
    }

    public interface ILucky
    {
        LuckProfile LuckProfile { get; }
    }
}