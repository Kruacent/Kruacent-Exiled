using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.Interface;
using KE.Utils.API.Sounds;
using MEC;
using UnityEngine;


namespace KE.Items.ItemEffects
{
    public class DeployableWallEffect : CustomItemEffect
    {
        public override void Effect(UsedItemEventArgs ev)
        {
            SpawnWall(ev.Player.Position,ev.Player.Rotation);
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
            float distance = 2;
            Vector3 forward = rotation * Vector3.forward;
            Vector3 spawnPos = pos + forward * distance;
            Vector3 rotat = new Vector3(0, rotation.eulerAngles.y, 0);


            
            
            Primitive wall = Primitive.Create(PrimitiveType.Cube, spawnPos, rotat, new Vector3(4, 4, 0.2f), true);
            Utils.API.Sounds.SoundPlayer.Instance.Play("lego", wall.GameObject, 10f, 40);
            wall.Collidable = true;
            wall.Visible = true;
            Timing.CallDelayed(10, () => {
                wall.Destroy();
            });
            Timing.CallDelayed(5, () =>
            {
                wall.Color = Color.yellow;
            });
            Timing.CallDelayed(8, () =>
            {
                wall.Color = Color.red;
            });


        }
    }
}
