using AdminToys;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.CustomRoles.Commands;
using LabApi.Features.Wrappers;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Player = Exiled.API.Features.Player;
using PlayerLab = LabApi.Features.Wrappers.Player;

namespace KruacentExiled.ClientPrimitives
{
    public class ClientSidePrimitive
    {

        private Vector3 _position;
        public Vector3 Position
        {
            get
            {
                return _position;
            }
            set
            {
                if(value != _position)
                {
                    _position = value;
                    if (AutoResync)
                    {
                        Resync();
                    }
                }
            }
        }

        private Quaternion _rotation;
        public Quaternion Rotation
        {
            get => _rotation;
            set
            {
                if (value != _rotation)
                {
                    _rotation = value;
                    if (AutoResync)
                    {
                        Resync();
                    }
                    
                }
            }
        }
        public Vector3 Scale { get; private set; }
        public PrimitiveType PrimitiveType { get; private set; }
        public Color Color { get; private set; }
        public PrimitiveFlags PrimitiveFlags { get; private set; }

        public SpawnMessage SpawnMessage { get; private set; }

        public ObjectDestroyMessage DestroyMessage { get; private set; }

        public uint NetId { get; private set; }

        public Player Player { get; }


        public bool AutoResync { get; set; }
        

        public ClientSidePrimitive(Vector3 position, Quaternion rotation, Vector3 scale, PrimitiveType primitiveType, Color color, PrimitiveFlags primitiveFlags,Player player,bool autoResync = true)
        {

            if(player == null || player.IsHost)
            {
                throw new ArgumentException("player null or is host");
            }

            _position = position;
            _rotation = rotation;
            Scale = scale;
            PrimitiveType = primitiveType;
            Color = color;
            PrimitiveFlags = primitiveFlags;
            Player = player;

            AutoResync = autoResync;

            GenerateNetworkMessages();

        }

        private void GenerateNetworkMessages()
        {
            NetId = NetworkIdentity.GetNextNetworkId();
            NetworkWriterPooled writer = NetworkWriterPool.Get();
            writer.Write<byte>(1); //??
            writer.Write<byte>(67); //??
            writer.Write(_position); //position
            writer.Write(_rotation); //rotation
            writer.Write(Scale); //scale
            writer.Write<byte>(0); //movement smoothing
            writer.Write(false); //static
            writer.Write((int)PrimitiveType); //primitivetype
            writer.Write(Color); // color
            writer.Write((byte)PrimitiveFlags); //primitive flag
            writer.Write<uint>(0); //parent netid



            //Log.Info("primitive position " + _position);
            //Log.Info("Player position " + Player.Position);

            SpawnMessage = new SpawnMessage()
            {
                netId = NetId,
                isLocalPlayer = false,
                isOwner = false,
                sceneId = 0,
                assetId = Loader.PrimitiveAssetId,
                position = _position,
                rotation = _rotation,
                scale = Scale,
                payload = writer.ToArraySegment()
            };

            DestroyMessage = new ObjectDestroyMessage()
            {
                netId = NetId,
            };
            //NetworkWriterPool.Return(writer);

        }



        public void Resync()
        {
            DestroyClientPrimitive();
            GenerateNetworkMessages();

            SpawnClientPrimitive();

        }

        public void DestroyClientPrimitive()
        {
            Player.Connection?.Send(DestroyMessage);
        }

        public void SpawnClientPrimitive()
        {
            Player.Connection?.Send(SpawnMessage);
        }
    }
}
