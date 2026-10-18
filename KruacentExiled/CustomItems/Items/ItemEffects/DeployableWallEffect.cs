using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Sounds;
using KruacentExiled.CustomItems.API.Interface;
using MEC;
using ProjectMER.Commands.Modifying.Position;
using UnityEngine;
using static PlayerList;


namespace KruacentExiled.CustomItems.Items.ItemEffects
{
    public class DeployableWallEffect : CustomItemEffect
    {
        public override void Effect(UsedItemEventArgs ev)
        {
            SpawnWall(ev.Player.Position, ev.Player.Rotation);
        }
        public override void Effect(DroppingItemEventArgs ev)
        {
            SpawnWall(ev.Player.Position, ev.Player.Rotation);
        }

        public override void Effect(ExplodingGrenadeEventArgs ev)
        {
            SpawnWall(ev.Position, ev.Projectile.Rotation);
        }

        private void SpawnWall(Vector3 pos, Quaternion rotation)
        {
            Vector3 forward = rotation * Vector3.forward;
            Vector3 spawnPos = GetSpawnPosition(pos, forward);
            Vector3 rotat = new Vector3(0, rotation.eulerAngles.y, 0);




            Primitive wall = Primitive.Create(PrimitiveType.Cube, spawnPos, rotat, new Vector3(4, 4, 0.2f), true);

            KruacentExiled.MainPlugin.AudioHandler.Play(Player.List, Audio.SoundType.Noise, "lego", wall.GameObject, 10f);
            wall.Collidable = true;
            wall.Visible = true;
            Timing.CallDelayed(10, () =>
            {
                wall?.Destroy();
                wall = null;
            });
            Timing.CallDelayed(5, () =>
            {
                if(wall != null)
                {
                    wall.Color = Color.yellow;
                }
                
            });
            Timing.CallDelayed(8, () =>
            {
                if (wall != null)
                {
                    wall.Color = Color.red;
                }
            });


        }

        public static Vector3 GetSpawnPosition(Vector3 position, Vector3 forward)
        {
            Vector3 result;
            if (Raycast(position, forward, out RaycastHit hit))
            {
                result = hit.point;
            }
            else
            {
                result = position + forward * Distance;
            }

            return result;
        }


        public const float Distance = 3f;



        public static bool Raycast(Vector3 position, Vector3 forward, out RaycastHit hit)
        {
            bool result = Physics.Raycast(position, forward, out hit, Distance, (int)LayerMasks.Default);

            //DrawableLines.IsDebugModeEnabled = true;
            //    DrawableLines.GenerateLine(1, UnityEngine.Color.gray, Player.Position, Player.Position + Player.CameraTransform.forward * Distance);

            return result;
        }
    }
}
