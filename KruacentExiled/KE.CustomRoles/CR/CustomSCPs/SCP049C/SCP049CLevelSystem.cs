using Exiled.API.Enums;
using Exiled.API.Features;
using KE.CustomRoles.CR.CustomSCPs.SCP049C.Positions;
using KE.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities;
using KE.CustomRoles.Settings;
using KE.Utils.API;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KE.Utils.API.Features;
using MEC;
using NorthwoodLib.Pools;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.Ragdolls;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public const int MaxLevel = 3;


        private int currentkill = 0;
        private int objective = 0;


        public int CurrentKill => currentkill;
        public int KillObjective => objective;

        public bool MaxLevelReached
        {
            get
            {
                return Level < MaxLevel;
            }
        }

        private ReferenceHub _hub;

        public ReferenceHub Hub => _hub;
        private SCP049CGUI gui;
        public SCP049CGUI GUI => gui;

        public List<Unlockable> CurrentAbilities { get; private set; }

        private static HashSet<Unlockable> abilities;

        private static IReadOnlyCollection<Unlockable> Abilities
        {
            get
            {
                if(abilities == null)
                {
                    abilities = ReflectionHelper.GetObjects<Unlockable>().ToHashSet();
                }
                return abilities;
            }
        }


        public void Awake()
        {
            _hub = ReferenceHub.GetHub(base.gameObject);
            CurrentAbilities = new();
            currentkill = 0;
            currentTime = 0;
            Level = 0;
            objective = GetNbKillPerTier(0);

            gui = new(this);

            SettingHandler.RightPressed += SettingHandler_RightPressed;
            SettingHandler.LeftPressed += SettingHandler_LeftPressed;


        }

        private int selected = 0;
        public int Selected => selected;
        private const int maxPerScreen = 2;
        private void SettingHandler_LeftPressed(Player obj)
        {
            if (obj.ReferenceHub != _hub) return;
            if (!CurrentlyChoosing) return;

            selected = Mathf.Abs((selected - 1) % maxPerScreen);
        }

        private void SettingHandler_RightPressed(Player obj)
        {
            if (obj.ReferenceHub != _hub) return;
            if (!CurrentlyChoosing) return;
            

            selected = (selected + 1) % maxPerScreen;
        }

        public void OnDestroy()
        {
            GUI.Dispose();
            gui = null;
            DisableAll();
        }



        public void AddKill()
        {
            KELog.Debug("add kill");
            currentkill++;

            if(!MaxLevelReached &&currentkill >= KillObjective)
            {
                currentkill = 0;

                AddLevel();
            }

        }

        public static int GetNbKillPerTier(int tier)
        {
            int x = tier + 1;

            double result = ((-1) * Math.Pow(x, 2)) + (6*x)-1;

            return (int)Math.Ceiling(result);

        }


        public void DisableAll()
        {
            foreach (Unlockable ability in CurrentAbilities)
            {
                ability.Remove(_hub);
            }
        }

        public void AddLevel()
        {
            List<Unlockable> ability = ListPool<Unlockable>.Shared.Rent();

            Level++;
            objective = GetNbKillPerTier(Level);
            foreach (Unlockable possibleAbility in Abilities)
            {
                if(possibleAbility.Tier == Level)
                {
                    ability.Add(possibleAbility);
                }
            }

            Log.Debug("nbaiblit"+ability.Count);
            Timing.RunCoroutine(GiveNewUnlockable(ability));


        }


        








        private int currentTime = 0;
        private const int maxTime = 17;
        public int TimeRemaining => maxTime-currentTime;

        private bool currentlyChoosing = false;
        public bool CurrentlyChoosing => currentlyChoosing;
        private IEnumerator<float> GiveNewUnlockable(List<Unlockable> ability)
        {

            Unlockable choosen = null;

            
            GUI.CreateHints(ability);
            currentTime = 0;
            selected = 0;
            currentlyChoosing = true;
            while (currentTime < maxTime)
            {
                yield return Timing.WaitForSeconds(1);
                currentTime++;
            }

            try
            {
                currentlyChoosing = false;

                choosen = GUI.UnlockableOnScreen[selected];
                GUI.DestroyHints();

                if (choosen != null)
                {
                    choosen.Grant(_hub);
                    CurrentAbilities.Add(choosen);
                    KELog.Debug(CurrentAbilities.Count);
                }
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
            finally
            {
                ListPool<Unlockable>.Shared.Return(ability);
            }
        }

 
        private Collider[] NonAlloc = new Collider[8];
        private Ragdoll ragdoll = null;

        private float cooldown = 0f;
        private float timeOnCorpse = 10f;

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

        public const float MaxDistance = 2;

        private void UpdateTime()
        {
            if (Vector3.Distance(ragdoll.Position,_hub.transform.position) > MaxDistance)
            {
                Reset();
            }


            cooldown += Time.deltaTime;
            RagdollArrowComp comp = ragdoll.GameObject.GetComponent<RagdollArrowComp>();

            comp.SetColor(Color.red);
            

            if (cooldown > timeOnCorpse)
            {
                ragdoll.Destroy();
                AddKill();
                Reset();

                
            }
            
        }


        private void Reset()
        {
            ragdoll.GameObject.GetComponent<RagdollArrowComp>().SetColor(Color.white);
            ragdoll = null;
            cooldown = 0;
        }


        private void GetNearRagdoll()
        {
            int num = Physics.OverlapSphereNonAlloc(_hub.GetPosition(), MaxDistance, NonAlloc, (int)LayerMasks.Ragdoll);

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
