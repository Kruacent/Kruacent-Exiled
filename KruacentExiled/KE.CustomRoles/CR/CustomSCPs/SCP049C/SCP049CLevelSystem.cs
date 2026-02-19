using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp049;
using Exiled.Events.Patches.Events.Scp0492;
using JetBrains.Annotations;
using KE.CustomRoles.CR.CustomSCPs.SCP049C.UnlockAbleAbilities;
using KE.Utils.API;
using NorthwoodLib.Pools;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.Ragdolls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.CustomRoles.CR.CustomSCPs.SCP049C
{
    public class SCP049CLevelSystem : MonoBehaviour
    {
        private int level;
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }

        private const int killperlevel = 2;

        private int currentkill = 0;

        private ReferenceHub _hub;

        private List<UnlockableAbility> CurrentAbilities;

        private static HashSet<UnlockableAbility> abilities;

        private static IReadOnlyCollection<UnlockableAbility> Abilities
        {
            get
            {
                if(abilities == null)
                {
                    abilities = ReflectionHelper.GetObjects<UnlockableAbility>().ToHashSet();
                }
                return abilities;
            }
        }

        public void Awake()
        {
            _hub = ReferenceHub.GetHub(base.gameObject);
            CurrentAbilities = new();
        }
        public void OnDestroy()
        {
            foreach(UnlockableAbility ability in CurrentAbilities)
            {
                ability.Remove(_hub);
            }
        }



        public void AddKill()
        {
            Log.Debug("add kill");
            currentkill++;

            if(currentkill >= killperlevel)
            {
                currentkill = 0;
                AddLevel();
            }

        }

        public void AddLevel()
        {
            List<UnlockableAbility> ability = ListPool<UnlockableAbility>.Shared.Rent();

            Level++;

            foreach(UnlockableAbility possibleAbility in Abilities)
            {
                if(possibleAbility.Tier == Level)
                {
                    ability.Add(possibleAbility);
                }
            }




            //todo choose ability
            UnlockableAbility choosen = ability.RandomItem();

            ListPool<UnlockableAbility>.Shared.Return(ability);

            choosen.Grant(_hub);

        }

 
        private Collider[] NonAlloc = new Collider[8];
        private Ragdoll ragdoll = null;

        private float cooldown = 0f;
        private float objective = 10f;

        internal void Update()
        {
            if (ragdoll != null)
            {
                UpdateTime();
            }
            else
            {
                GetNearRagdoll();
            }

        }


        private void UpdateTime()
        {
            cooldown += Time.deltaTime;
            if (cooldown > objective)
            {
                ragdoll.Destroy();
                AddKill();
                ragdoll = null;
                cooldown = 0;
            }
            
        }


        private void GetNearRagdoll()
        {
            int num = Physics.OverlapSphereNonAlloc(_hub.GetPosition(), 5, NonAlloc, (int)LayerMasks.Ragdoll);

            for (int i = 0; i < num; i++)
            {
                if (NonAlloc[i].TryGetComponent<BasicRagdoll>(out var r))
                {
                    Ragdoll ragdoll = Ragdoll.Get(r);

                    if (ragdoll.IsExpired)
                    {
                        this.ragdoll = ragdoll;
                    }


                }
            }
        }

    }
}
