using Exiled.API.Features;
using KE.Utils.API.Interfaces;
using KruacentExiled.Map.Others.CustomZones.CustomRooms.MCZ;
using LabApi.Events.Arguments.ServerEvents;
using MapGeneration;
using System;
using System.Linq;
using UnityEngine;

namespace KruacentExiled.Map.Others.CustomZones
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
            new Curve();
            new XCorridor();
            new MCZDoorSeparator();

            System.Random random = new System.Random(seed);

            zone.Generate(random,Layout.Layouts.First(l => l.Name == "Circle"));


            teleport = CustomRoom.RegisteredRoom.First().SpawnedRoom.First(s => s.Shape == RoomShape.Straight).Position + Vector3.up * 5;
            Log.Debug("teleport " + teleport);
        }


    }
}
