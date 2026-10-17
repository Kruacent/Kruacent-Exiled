using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Items.Keycards;
using Exiled.API.Features.Pickups;
using Exiled.API.Interfaces.Keycards;
using InventorySystem.Items.Keycards;
using System;
using System.Linq;
using UnityEngine;

namespace KruacentExiled.CustomItems.API.Features
{
    /// <summary>
    /// <see cref="Exiled.CustomItems.API.Features.CustomKeycard"/>
    /// </summary>
    public abstract class KECustomKeycard : KECustomItem
    {

        public KeycardPermissions Permissions { get; set; } = KeycardPermissions.None;



        public virtual Color32? KeycardPermissionsColor { get; set; } = null;
        public virtual Color32? KeycardLabelColor { get; set; } = null;
        public virtual byte Wear { get; set; }
        public virtual byte Rank { get; set; }

        public virtual Color32? TintColor { get; set; } = null;
        public virtual string KeycardName { get; set; } = null;
        public virtual string KeycardLabel { get; set; } = null;
        public virtual string SerialNumber { get; set; } = null;

        public override ItemType ItemType
        {
            get
            {
                return base.Type;
            }
        }


        protected virtual void SetupKeycard(Keycard keycard)
        {
            NametagDetail detail = keycard.Base.Details.OfType<NametagDetail>().FirstOrDefault();
            if (keycard is CustomKeycardItem customKeycardItem)
            {
                customKeycardItem.Permissions = Permissions;
                if (KeycardPermissionsColor.HasValue)
                {
                    customKeycardItem.PermissionsColor = KeycardPermissionsColor.Value;
                }

                if (TintColor.HasValue)
                {
                    customKeycardItem.Color = TintColor.Value;
                }

                if (!string.IsNullOrEmpty(Name))
                {
                    customKeycardItem.ItemName = Name;
                }

                if (!string.IsNullOrEmpty(KeycardName) && customKeycardItem is INameTagKeycard nameTagKeycard)
                {
                    nameTagKeycard.NameTag = KeycardName;
                }

                if (customKeycardItem is ILabelKeycard labelKeycard)
                {
                    if (!string.IsNullOrEmpty(KeycardLabel))
                    {
                        labelKeycard.Label = KeycardLabel;
                    }

                    if (KeycardLabelColor.HasValue)
                    {
                        labelKeycard.LabelColor = KeycardLabelColor.Value;
                    }
                }

                if (customKeycardItem is IWearKeycard wearKeycard)
                {
                    wearKeycard.Wear = Wear;
                }

                if (customKeycardItem is ISerialNumberKeycard serialNumberKeycard)
                {
                    serialNumberKeycard.SerialNumber = SerialNumber;
                }

                if (customKeycardItem is IRankKeycard rankKeycard)
                {
                    rankKeycard.Rank = Rank;
                }
            }
            //else if (keycard.Base.Customizable && detail != null)
            //{
            //    detail.SetArguments(KeycardName);
            //    if (KeycardDetailSynchronizer.Database.Remove(keycard.Serial))
            //    {
            //        KeycardDetailSynchronizer.ServerProcessItem(keycard.Base);
            //    }
            //}
        }

        public override void Init()
        {
            if (!ItemType.IsKeycard())
            {
                throw new ArgumentOutOfRangeException("ItemType", ItemType, "Invalid keycard type.");
            }


            base.Init();
        }

        public override void Give(Player player, Item item, bool displayMessage = true)
        {
            if (item.Is<Keycard>(out var param))
            {
                SetupKeycard(param);
            }

            base.Give(player, item, displayMessage);
        }

        public override Pickup Spawn(Vector3 position, Item item, Player previousOwner = null)
        {
            if (item.Is<Keycard>(out var param))
            {
                SetupKeycard(param);
            }

            return base.Spawn(position, item, previousOwner);
        }


        protected override void ShowPickedUpMessage(Player player)
        {
            Message(this, player, true);
        }

        protected override void ShowSelectedMessage(Player player)
        {
            Message(this, player);
        }

    }
}
