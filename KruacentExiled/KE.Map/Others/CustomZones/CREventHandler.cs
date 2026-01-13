using Exiled.API.Features;
using KE.Map.CustomZones;
using KE.Map.CustomZones.CustomRooms.MCZ;
using KE.Utils.API.Interfaces;
using LabApi.Events.Arguments.ServerEvents;
using MapGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Others.CustomZones
{
    internal class CREventHandler : IUsingEvents
    {
        public void SubscribeEvents()
        {
            Exiled.Events.Handlers.Map.Generated += OnGenerated;
            LabApi.Events.Handlers.ServerEvents.MapGenerating += OnGenerating;
        }

        public void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Map.Generated -= OnGenerated;
            LabApi.Events.Handlers.ServerEvents.MapGenerating -= OnGenerating;
        }

        private int seed;
        private Vector3 teleport;
        private void OnGenerating(MapGeneratingEventArgs ev)
        {
            seed = ev.Seed;

        }
        private void OnGenerated()
        {
            Log.Debug("read");
            try
            {

                AltasReader.Read();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }


            CustomZone zone = new MediumContainmentZone();
            //teleport = zone.Spawnzone;
            new SCorridor();
            new EndRoom();
            new TCorridor();

            zone.Generate(new System.Random(seed));

            teleport = CustomRoom.RegisteredRoom.First().SpawnedRoom.First(s => s.Shape == RoomShape.Straight).Position + Vector3.up * 5;
            Log.Debug("teleport " + teleport);
        }


    }
}
