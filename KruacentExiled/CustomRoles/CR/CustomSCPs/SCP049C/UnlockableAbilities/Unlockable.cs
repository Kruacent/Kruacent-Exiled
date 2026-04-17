namespace KruacentExiled.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities
{
    public abstract class Unlockable
    {

        public abstract byte Tier { get; }

        public abstract string GetName(ReferenceHub hub);
        public abstract string GetDescription(ReferenceHub hub);

        public abstract void Grant(ReferenceHub hub);


        public abstract void Remove(ReferenceHub hub);

    }
}
