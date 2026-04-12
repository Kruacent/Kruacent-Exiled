using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using KE.Items.API.Interface;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static KE.Items.Items.Scp1650;

namespace KE.Items.Items.ItemEffects
{
    public class Scp1650Effect : CustomItemEffect
    {
        public enum CardinalPoints
        {
            South,
            West,
            North,
            East,
        }

        public override void Effect(UsedItemEventArgs ev)
        {
            OnUsedItem(ev.Player);
        }
        public override void Effect(DroppingItemEventArgs ev)
        {
            OnUsedItem(ev.Player);
        }

        public override void Effect(ExplodingGrenadeEventArgs ev)
        {
            foreach (Player player in ev.TargetsToAffect)
            {
                OnUsedItem(player);
            }
        }


        private void OnUsedItem(Player player)
        {

            CardinalPoints rotation = Rotation(player.Rotation);

            Log.Debug(rotation);
            switch (rotation)
            {

                case CardinalPoints.South:
                    Timing.RunCoroutine(Regeneration(player, 10));
                    break;
                case CardinalPoints.West:
                    player.EnableEffect(Exiled.API.Enums.EffectType.BodyshotReduction, 25, 60);
                    break;
                case CardinalPoints.North:
                    player.EnableEffect(Exiled.API.Enums.EffectType.Invigorated, 1, 30);
                    break;
                case CardinalPoints.East:
                    player.EnableEffect(Exiled.API.Enums.EffectType.CardiacArrest, 1, 10);
                    break;

            }


        }

        private IEnumerator<float> Regeneration(Player p, float duration)
        {
            float timeWaited = duration;

            while (timeWaited > 0)
            {
                timeWaited -= .1f;
                p.Heal(1);
                yield return Timing.WaitForSeconds(.1f);
            }


        }

        private CardinalPoints Rotation(Quaternion rotation)
        {

            Vector3 forward1 = rotation * Vector3.forward;
            float x = forward1.x;
            float z = forward1.z;

            if (z > .75f)
                return CardinalPoints.South;
            if (x > .75f)
                return CardinalPoints.West;
            if (z <= -.75f)
                return CardinalPoints.North;
            if (x <= -.75f)
                return CardinalPoints.East;
            return CardinalPoints.East;
        }
    }
}
