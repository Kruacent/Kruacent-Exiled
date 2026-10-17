using AdminToys;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.Loader;
using global::KE.Utils.API.KETextToy;
using KE.Utils.API.Features;
using MEC;
using Mirror;
using ProjectMER.Commands.Modifying.Scale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.DedicatedServer;

namespace KruacentExiled.ClientPrimitives
{


    internal class ClientFollowingTextToy
    {

        private static HashSet<ClientFollowingTextToy> list = new HashSet<ClientFollowingTextToy>();
        public HashSet<Player> Following { get; }
        public SpawnMessage SpawnMessage { get; private set; }

        public ObjectDestroyMessage DestroyMessage { get; private set; }

        public uint NetId { get; private set; }
        public string Text { get; private set; }
        public const string EmptyText = " ";

        public bool OnlyMoveY { get; set; } = false;

        private Vector3 _position;
        public Vector3 Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (value != _position)
                {
                    _position = value;
                    if (AutoResync)
                    {
                        Resync();
                    }
                }
            }
        }

        public bool AutoResync { get; set; }

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

        public ClientFollowingTextToy(IEnumerable<Player> players, Vector3 position, Quaternion rotation, Vector3 scale, string text)
        {

            throw new NotImplementedException("GenerateNetworkMessage doesn't work");
            Following = players.ToHashSet();

            _position = position;
            _rotation = rotation;
            Scale = scale;
            Text = text;


            list.Add(this);
            GenerateNetworkMessages();
            SendSpawnMessage();

            //Start();
        }

        public void Resync()
        {
            SendDestroyMessage();
            GenerateNetworkMessages();

            SendSpawnMessage();

        }
        public void SendDestroyMessage()
        {
            foreach (Player player in Following)
            {
                player.Connection?.Send(DestroyMessage);
            }


        }

        public void SendSpawnMessage()
        {
            foreach (Player player in Following)
            {
                player.Connection?.Send(SpawnMessage);
            }
        }

        private void GenerateNetworkMessages()
        {
            NetId = NetworkIdentity.GetNextNetworkId();
            NetworkWriterPooled writer = NetworkWriterPool.Get();



            writer.WriteUInt(1);       // SyncList count
            writer.WriteString(Text); // element 0
            writer.WriteUInt(0);       // changesAhead


            writer.Write(_position); //position
            writer.Write(_rotation); //rotation
            writer.Write(Scale); //scale
            writer.Write<byte>(0); //movement smoothing
            writer.Write(false); //static

            writer.WriteVector2(new Vector2(50f, 50f)); //display size
            writer.WriteString(Text); //text format

            writer.WriteUInt(0); // _clientParentId


            SpawnMessage = new SpawnMessage()
            {
                netId = NetId,
                isLocalPlayer = false,
                isOwner = false,
                sceneId = 0,
                assetId = Loader.TextToyAssetId,
                position = _position,
                rotation = _rotation,
                scale = Scale,
                payload = writer.ToArraySegment()
            };

            DestroyMessage = new ObjectDestroyMessage()
            {
                netId = NetId,
            };



        }



        public void SyncToPlayers()
        {
            foreach (Player player in Following)
            {
                Vector3 dir = player.CameraTransform.position - Position;
                if (OnlyMoveY)
                {
                    dir.y = 0f;
                    dir.Normalize();
                }

                if (!dir.Equals(Vector3.zero))
                {
                    Quaternion rot = Quaternion.LookRotation(dir) * Quaternion.Euler(0f, 180f, 0f);

                    Rotation = rot;
                    Resync();


                }
            }
        }






        private static CoroutineHandle handle;
        public static void Start()
        {
            if (!handle.IsRunning)
            {
                Log.Info("starting followingTextToy");
                handle = Timing.RunCoroutine(Loop());
            }

        }

        public static void Stop()
        {
            Timing.KillCoroutines(handle);
        }

        public static bool IsRunning
        {
            get
            {
                return handle.IsRunning;
            }
            set
            {
                if (value)
                {
                    Start();
                }
                else
                {
                    Stop();
                }
            }
        }


        public void Destroy()
        {
            list.Remove(this);
            SendDestroyMessage();
            if (list.Count == 0)
            {
                Stop();
            }
        }


        private static IEnumerator<float> Loop()
        {
            while (true)
            {

                foreach (ClientFollowingTextToy textToy in list.ToList())
                {
                    try
                    {
                        textToy.SyncToPlayers();
                    }catch(Exception e)
                    {
                        Log.Error(e);
                    }
                }

                yield return Timing.WaitForSeconds(1);

            }
        }

        
    }

}
