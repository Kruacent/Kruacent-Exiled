using Exiled.API.Features;
using KE.Utils.API.Interfaces;
using KruacentExiled.CustomItems.Events.CustomItem.EventArgs;
using KruacentExiled.CustomItems.Items;
using ServerAchievementFramework.API.Feature;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace KruacentExiled.Achievements.CustomItems
{
    public class Scanning : Achievement, IUsingEvents
    {
        public override string Name => "scanning";

        public override string Description => "Scan every player with one MScan";

        private Dictionary<Player, HashSet<Player>> scanned = null;
        public void SubscribeEvents()
        {
            scanned = new Dictionary<Player, HashSet<Player>>();
            MScan.ScannedByMScan += OnScannedByMScan;
            MScan.DeployedMScan += OnDeployedMScan;
            MScan.RemovedMScan += OnRemovedMScan;
        }

        public void UnsubscribeEvents()
        {
            MScan.ScannedByMScan -= OnScannedByMScan;
            MScan.DeployedMScan -= OnDeployedMScan;
            MScan.RemovedMScan -= OnRemovedMScan;
            scanned = null;
        }

        private void OnDeployedMScan(DeployedMScanEventArgs ev)
        {
            scanned[ev.Player] = new HashSet<Player>();
        }
        private void OnScannedByMScan(ScannedByMScanEventArgs ev)
        {
            HashSet<Player> players = scanned[ev.Owner];           

            if(players.Count == Player.ConnectedCount - 1)
            {
                GiveAchievement(ev.Owner);
            }

        }
        private void OnRemovedMScan(RemovedMScanEventArgs ev)
        {
            scanned.Remove(ev.Player);
        }
    }
}
