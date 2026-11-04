using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using KE.Utils.API.Features;
using KruacentExiled.CustomItems.API.Enum;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.CustomItems.API.Interface;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KruacentExiled.CustomItems.Items
{
    public class CompressedCoin : KECustomItem, IViolentItem, ILumosItem
    {
        private static readonly Dictionary<ushort, int> ItemFlips = new Dictionary<ushort, int>();
        public override ItemType ItemType => ItemType.Coin;
        public Color Color { get; set; } = Color.yellow;
        public override string Name { get; set; } = "CompressedCoin";
        public override float Weight { get; set; } = 0.2f;
        public override SpawnProperties SpawnProperties { get; set; } = null;
        public bool IsViolent => false;

        public override ItemRank Rank { get; } = ItemRank.Good;

        public static bool TrySetCoinUses(ushort serial, int count)
        {
            if (ItemFlips.ContainsKey(serial))
            {
                return false;
            }

            ItemFlips[serial] = count;
            return true;
        }

        public static int GetCoinUses(ushort serial)
        {
            if (ItemFlips.TryGetValue(serial, out int count))
            {
                return count;
            }

            int coinUses = Mathf.FloorToInt(Mathf.Lerp(10, 31, Mathf.Pow(UnityEngine.Random.value, 2f)));

            ItemFlips[serial] = coinUses;

            return coinUses;
        }

        public static bool RemoveCoinUses(ushort serial)
        {
            return ItemFlips.Remove(serial);
        }

        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Compressed Coin",
                    [TranslationKeyDesc] = "A very strange coin",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Pièce compressée",
                    [TranslationKeyDesc] = "Une pièce très étrange",
                },
            };
        }
    }
}