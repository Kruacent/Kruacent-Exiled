using KruacentExiled.CustomRoles.API.Features;
namespace KruacentExiled.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities.Tier1
{
    internal class JumpUnlockable : UnlockableAbility
    {
        public override byte Tier => 1; //1

        public override KEAbilities Ability => KEAbilities.Get("ToggleHighJump");

        public override string GetName(ReferenceHub hub)
        {
            return "Jump";
        }
        public override string GetDescription(ReferenceHub hub)
        {
            return "You can now jump higher\n(high enough to go up Gate B)";
        }
        public override void Grant(ReferenceHub hub)
        {

            if (!hub.gameObject.TryGetComponent<HigherJumpComp>(out _))
            {
                hub.gameObject.AddComponent<HigherJumpComp>();
            }
            base.Grant(hub);
        }

        public override void Remove(ReferenceHub hub)
        {
            if (hub.gameObject.TryGetComponent<HigherJumpComp>(out var comp))
            {
                comp.Destroy();
            }
            base.Remove(hub);
        }
    }
}
