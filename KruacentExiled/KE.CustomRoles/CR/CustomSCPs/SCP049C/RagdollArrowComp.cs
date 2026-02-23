using Exiled.API.Features;
using KE.Utils.API.Features;
using KE.Utils.API.KETextToy;
using NorthwoodLib.Pools;
using PlayerRoles.Ragdolls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.CR.CustomSCPs.SCP049C
{
    public class RagdollArrowComp : MonoBehaviour
    {

        private FollowingTextToy arrow = null;

        private Ragdoll ragdoll;

        public const string arrowText = "↓";

        public void Init(Ragdoll ragdoll)
        {
            this.ragdoll = ragdoll;
            
        }


        private void Update()
        {
            if (ragdoll.IsExpired && arrow == null)
            {
                CreateArrow();
            }
        }

        private void OnDestroy()
        {
            arrow.Destroy();
        }

        public void SetColor(Color color)
        {
            StringBuilder sb = StringBuilderPool.Shared.Rent();

            sb.Append("<color=#");
            sb.Append(ColorUtility.ToHtmlStringRGB(color));
            sb.Append(">");
            sb.Append(arrowText);
            sb.Append("</color>");


            arrow.Toy.TextFormat = sb.ToString();

            StringBuilderPool.Shared.Return(sb);

        }

        private void CreateArrow()
        {
            KELog.Debug("arrow");
            arrow = new FollowingTextToy(SCP049CRole.instance.TrackedPlayers, ragdoll.Position+Vector3.up, Quaternion.identity, Vector3.one*2);

            arrow.OnlyMoveY = true;
            SetColor(Color.white);
            arrow.Toy.Spawn();
        }
    }
}
