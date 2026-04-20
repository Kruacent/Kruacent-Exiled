using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp1509;
using KE.Utils.API.Features;
using KruacentExiled.CustomRoles.CR.MTF.RedMist;
using System.Collections.Generic;
namespace KruacentExiled.CustomRoles.Abilities.RedMist.GreaterSplitHorizontal
{
    public class GreaterSplitHorizontal : EgoAbility
    {
        public const string FailEgo = "GreaterSplitFailEGO";
        public const string FailWeapon = "GreaterSplitFailWeapon";


        public override string Name { get; } = "GreaterSplitHorizontal";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Greater Split : Horizontal",
                    [TranslationKeyDesc] = "Manifest your E.G.O, gain powerful buff but rapid health drain.\nCan be deactivated anytime",
                    [FailEgo] = "You need to manifest your E.G.O. first",
                    [FailWeapon] = "You need your weapon",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "todo",
                    [TranslationKeyDesc] = "todo",
                    [FailEgo] = "todo",
                    [FailWeapon] = "todo",
                }
            };
        }
        public override float Cooldown { get; } = 0f;

        protected override NeedActive NeedEGOActive => NeedActive.NeedActive;

        public const float Damage = 200;


        public static readonly LayerMasks Mask = LayerMasks.Scp173Teleport | LayerMasks.Glass;
        protected override void AbilityAdded(Player player)
        {

            if (!player.GameObject.TryGetComponent<AttackGreaterSplitComp>(out var comp))
            {
                comp = player.GameObject.AddComponent<AttackGreaterSplitComp>();
            }
            comp.Init(player, this);
            

            base.AbilityAdded(player);
        }

        protected override void AbilityRemoved(Player player)
        {
            if (player.GameObject.TryGetComponent<AttackGreaterSplitComp>(out var comp))
            {
                UnityEngine.Object.Destroy(comp);
            }
            base.AbilityRemoved(player);
        }


        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Scp1509.TriggeringAttack += OnTriggeringAttack;
            
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Scp1509.TriggeringAttack -= OnTriggeringAttack;
            base.UnsubscribeEvents();
        }





        private void OnTriggeringAttack(TriggeringAttackEventArgs ev)
        {
            Player player = ev.Player;
            if (!Check(player)) return;
            if (!ev.IsAllowed) return;
            if (player.GameObject.TryGetComponent<AttackGreaterSplitComp>(out var comp))
            {
                ev.IsAllowed = !comp.OnTriggerAttack();

            }
            ev.Scp1509.NextResurrectTime = float.MaxValue;
        }


        
        

        protected override bool LaunchedAbility(Player player,EGO ego)
        {
            KELog.Debug("check weapopgn");
            if(player.CurrentItem is null || player.CurrentItem.Type != ItemType.SCP1509)
            {
                ShowEffectHint(player, FailWeapon);
                return false;
            }


            if (player.GameObject.TryGetComponent<AttackGreaterSplitComp>(out var comp))
            {
                comp.StartAttack();
            }


            return true;
        }
        
        


    }
}
