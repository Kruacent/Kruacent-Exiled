using Exiled.API.Features;
using KE.CustomRoles.API.Features;
using System.Collections.Generic;
using UnityEngine;
using PlayerRoles.FirstPersonControl.Thirdperson;
using PlayerRoles;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Toys;
using Exiled.API.Enums;
using KE.CustomRoles.CR.MTF.RedMist;
namespace KE.CustomRoles.Abilities.RedMist
{
    public class ForwardSlash : KEAbilities
    {
        public override string Name { get; } = "ForwardSlash";
        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new()
            {
                ["en"] = new()
                {
                    [TranslationKeyName] = "Forward Slash",
                    [TranslationKeyDesc] = "Manifest your E.G.O, gain powerful buff but rapid health drain.\nCan be deactivated anytime",
                    ["ForwardSlashFailEGO"] = "You need to manifest your E.G.O. first",
                    ["ForwardSlashFailWeapon"] = "You need to manifest your E.G.O. first",
                },
                ["fr"] = new()
                {
                    [TranslationKeyName] = "todo",
                    [TranslationKeyDesc] = "todo",
                }
            };
        }
        public override float Cooldown { get; } = 0f;


        public const float MaxDistance = 15;

        protected override bool AbilityUsed(Player player)
        {

            
            if (!player.GameObject.TryGetComponent<EGO>(out var ego))
            {
                ego = player.GameObject.AddComponent<EGO>();
            }



            if (!ego.Active)
            {
                //show ForwardSlashFailEGO
                return false;
            }

            
            if(player.CurrentItem.Type != ItemType.SCP1509)
            {
                //show ForwardSlashFailWeapon
                return false;
            }

            Vector3 forward = player.Transform.forward;

            if(Physics.Raycast(player.Position, forward,out RaycastHit hit, MaxDistance, (int)~LayerMasks.Hitbox))
            {
                if(Physics.Linecast(player.Position, hit.point,out RaycastHit hitPlayer,(int)LayerMasks.Hitbox))
                {
                    
                }
            }



            return base.AbilityUsed(player);
        }


    }
}
