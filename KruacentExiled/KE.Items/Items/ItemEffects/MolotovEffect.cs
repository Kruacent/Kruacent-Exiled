using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Features;
using KE.Items.API.Interface;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KE.Items.Items.ItemEffects
{
    public class MolotovEffect : CustomItemEffect
    {
        public float Duration { get; set; } = 20f;
        public float Radius { get; set; } = 5f;
        public float TickRate { get; set; } = 0.5f;

        private static HashSet<ushort> ActiveMolotovSerialNumbers = new HashSet<ushort>();
        private static List<Vector3> ActiveFireZones = new List<Vector3>();

        public override void Effect(UsedItemEventArgs ev) => SpawnMolotov(ev.Player, ev.Player.Position);
        public override void Effect(DroppingItemEventArgs ev) => SpawnMolotov(ev.Player, ev.Player.Position);
        public override void Effect(ExplodingGrenadeEventArgs ev) => SpawnMolotov(ev.Player, ev.Position);

        public void OnReceivingEffect(ReceivingEffectEventArgs ev)
        {
            if (ev.Effect.GetEffectType() == EffectType.Hypothermia)
            {
                foreach (Vector3 zoneCenter in ActiveFireZones)
                {
                    if (Vector3.Distance(ev.Player.Position, zoneCenter) <= Radius + 1f)
                    {
                        ev.IsAllowed = false;
                        return;
                    }
                }
            }
        }

        public void OnPickingUp(PickingUpItemEventArgs ev)
        {
            if (ev.Pickup != null && ActiveMolotovSerialNumbers.Contains(ev.Pickup.Serial))
            {
                ev.IsAllowed = false;
                KECustomItem.ItemEffectHint(ev.Player, "<color=red>C'est brûlant ! Touche pas à ça !</color>");
                ev.Player.Hurt(10f, DamageType.Tesla);
            }
        }

        private void SpawnMolotov(Player owner, Vector3 centerPos)
        {
            Scp244 jarItem = (Scp244)Item.Create(ItemType.SCP244a);
            jarItem.Scale = new Vector3(0.2f, 0.2f, 0.2f);
            jarItem.Primed = true;
            jarItem.MaxDiameter = 0.2f;

            Pickup jarPickup = jarItem.CreatePickup(centerPos);
            if (jarPickup.GameObject.TryGetComponent(out Rigidbody rb)) rb.isKinematic = true;

            ActiveMolotovSerialNumbers.Add(jarPickup.Serial);
            ActiveFireZones.Add(centerPos);

            Vector3 groundPos = centerPos - Vector3.up * 0.15f;
            Primitive dangerZone = Primitive.Create(PrimitiveType.Cylinder, groundPos, Vector3.zero, new Vector3(Radius, 0.1f, Radius), true);
            dangerZone.Collidable = false;
            dangerZone.Color = new Color(1f, 0f, 0f, 0.5f);

            List<Light> fireLights = new List<Light>();
            Color fireColor = new Color(255, 128, 0);

            fireLights.Add(CreateLight(centerPos + Vector3.up * 0.5f, fireColor, Radius /2f, 0.2f));
            Timing.RunCoroutine(Fire(jarPickup, dangerZone, fireLights, owner, centerPos));
        }

        private Light CreateLight(Vector3 pos, Color col, float range, float intensity)
        {
            Light l = Light.Create(pos,spawn:false);
            l.LightType = LightType.Point;
            l.Color = col;
            l.Range = range;
            l.Intensity = intensity;
            l.Spawn();
            return l;
        }

        private IEnumerator<float> Fire(Pickup jar, Primitive zone, List<Light> lights, Player owner, Vector3 center)
        {
            float elapsed = 0f;

            while (elapsed < Duration)
            {
                foreach (var l in lights)
                {
                    if (l != null) l.Intensity = Random.Range(0.1f, 0.3f);
                }

                if (zone != null)
                {
                    float pulse = 0.1f + Mathf.PingPong(Time.time, 0.05f);
                    zone.Scale = new Vector3(Radius, pulse, Radius);
                }

                foreach (Player target in Player.List)
                {
                    if (!target.IsAlive) continue;

                    if (Vector3.Distance(new Vector3(center.x, 0, center.z), new Vector3(target.Position.x, 0, target.Position.z)) <= Radius / 2
                        && Mathf.Abs(center.y - target.Position.y) < 2.5f)
                    {
                        if (owner != null && !Server.FriendlyFire && target.Role.Team == owner.Role.Team && target != owner)
                            continue;

                        target.EnableEffect(EffectType.Burned, 1f);

                        if (target.IsHuman)
                        {
                            if (target.ArtificialHealth > 0) target.ArtificialHealth -= 10f;
                            else target.Hurt(6f, DamageType.Firearm);
                        }
                        else if (target.IsScp)
                        {
                            float dmg = 20f;

                            if (target.Role == RoleTypeId.Scp0492)
                            {
                                target.Hurt(dmg * 2.5f, DamageType.Firearm);
                            }
                            else
                            {
                                target.Health -= dmg;

                                if (target.Health <= 0)
                                {
                                    target.Kill(DamageType.Firearm);
                                }
                            }
                        }
                    }
                }

                yield return Timing.WaitForSeconds(TickRate);
                elapsed += TickRate;
            }

            if (zone != null) zone.Destroy();

            foreach (var l in lights)
            {
                if (l != null) l.Destroy();
            }

            if (jar != null)
            {
                ActiveMolotovSerialNumbers.Remove(jar.Serial);
                ActiveFireZones.Remove(center);
                jar.Destroy();
            }
        }
    }
}