using Exiled.API.Features;
using Exiled.API.Features.Pools;
using KE.CustomRoles.Abilities.FireAbilities;
using KE.CustomRoles.Settings;
using KE.Utils.API;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using MEC;
using PlayerRoles.FirstPersonControl.Thirdperson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace KE.CustomRoles.API.Features
{
    /// <summary>
    /// Our version of the <see cref="Exiled.CustomRoles.API.Features.ActiveAbility"/>
    /// </summary>
    public abstract class KEAbilities
    {
        #region static stuff
        private static HashSet<KEAbilities> registered = new();
        public static HashSet<KEAbilities> Registered => registered;
        #endregion

        #region abstract stuff
        public abstract string Name { get; }
        public abstract string PublicName { get; }
        public abstract string Description { get; }

        /// <summary>
        /// in seconds
        /// </summary>
        public abstract float Cooldown { get; }
        #endregion

        private Dictionary<Player, DateTime> LastUsed = new();
        private static Dictionary<System.Type, KEAbilities> TypeToAbility { get; } = new();
        private static Dictionary<string, KEAbilities> NameToAbility { get; } = new();
        public HashSet<Player> Players { get; } = new HashSet<Player>();
        public HashSet<Player> Selected { get; } = new();



        public static Dictionary<Player, List<KEAbilities>> PlayersAbility { get;} = new();

        protected KEAbilities()
        {
            
        }
        public void Init()
        {
            if (NameToAbility.ContainsKey(Name))
            {
                KEAbilities other = NameToAbility[Name];
                Log.Warn($"{GetType().FullName} have the same name as {other.GetType().FullName}. Skipping...");
                return;
            }

            TypeToAbility.Add(GetType(), this);
            NameToAbility.Add(Name, this);
            InternalSubscribeEvent();
        }

        public void Destroy()
        {
            InternalUnsubcribeEvent();
        }

        private void InternalSubscribeEvent()
        {

            SubscribeEvents();
        }

        private void InternalUnsubcribeEvent()
        {
            UnsubscribeEvents();
        }





        protected virtual void SubscribeEvents()
        {

        }

        protected virtual void UnsubscribeEvents()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <returns>return true if the ability was used; returns false if the ability was not used and need to be refunded</returns>
        protected virtual bool AbilityUsed(Player player)
        {
            return true;
        }

        protected virtual void AbilityAdded(Player player)
        {

        }
        protected virtual void AbilityRemoved(Player player)
        {

        }


        public void ShowAbility(Player player)
        {
            float time = MainPlugin.SettingHandler.GetAbilityTime(player);

            StringBuilder sb = StringBuilderPool.Pool.Get();

            sb.Append("<b>");
            sb.Append(PublicName);
            sb.Append("</b>");
            sb.AppendLine();
            sb.Append(Description);


            DisplayHandler.Instance.AddHint(MainPlugin.AbilitiesDesc, player, sb.ToString(), time);
            StringBuilderPool.Pool.Return(sb);
        }

        public void SelectAbility(Player player)
        {
            if (Selected.Add(player))
            {
                Log.Debug($"player {player.Nickname} selected ability {this}");
                foreach (KEAbilities abilities in Registered.Where(a => a != this && a.Players.Contains(player)))
                {
                    abilities.UnselectAbility(player);
                }

                ShowAbility(player);
            }
        }

        public void UnselectAbility(Player player)
        {
            Log.Debug($"player {player.Nickname} unselected ability {this}");
            Selected.Remove(player);
        }
        
        public void RemoveAbility(Player player)
        {

            if (Players.Contains(player))
            {
                Log.Debug($"player {player.Nickname} lost {this}");
                PlayersAbility[player].Remove(this);
                Players.Remove(player);
                AbilityRemoved(player);
            }
        }
        public void AddAbility(Player player)
        {
            
            bool result = Players.Add(player);
            Log.Debug($"player {player.Nickname} got {this} ({result})");
            if (result)
            {
                if(!PlayersAbility.TryGetValue(player,out var _))
                {
                    PlayersAbility.Add(player, new());
                }
                PlayersAbility[player].Add(this);

                DisplayHandler.Instance.CreateAuto(player, arg => UpdateGUI(player), AbilityPosition.HintPlacement);


                AbilityAdded(player);
            }
            
        }
        public void UseAbility(Player player)
        {
            

            if (AbilityUsed(player))
            {
                LastUsed[player] = DateTime.Now;
            }
            
        }

        public bool Check(Player player)
        {
            if(player is not null)
            {
                return Players.Contains(player);
            }
            return false;
        }

        public bool CanUse(Player player, out string result)
        {
            if (player == null)
            {
                result = "player null";
                return false;
            }
            if (!Players.Contains(player))
            {
                result = "cannot use this";
                return false;
            }

            if (!LastUsed.ContainsKey(player))
            {
                result = "never used";
                return true;
            }

            DateTime dateTime = LastUsed[player] + TimeSpan.FromSeconds(Cooldown);
            if (DateTime.Now > dateTime)
            {
                result = "ok";
                return true;
            }
            result = "in cooldown";
            return false;
        }



        public static void UseSelected(Player player)
        {
            if(TryGetSelected(player,out var ability))
            {
                if (ability.CanUse(player, out string _))
                {
                    ability.UseAbility(player);
                }
            }
        }

        public static bool TryAddToPlayer(Type typeAbility,Player player)
        {
            if (!TypeToAbility.TryGetValue(typeAbility, out var ability)) return false;


            ability.AddAbility(player);
            return true;
        }
        public static bool TryAddToPlayer(string name, Player player)
        {
            if (!TryGet(name,out var ability)) return false;


            ability.AddAbility(player);
            return true;
        }

        public static void RemoveAllSelect(Player player)
        {
            if (!PlayersAbility.ContainsKey(player)) return;

            foreach(KEAbilities ability in PlayersAbility[player])
            {
                ability.Selected.Remove(player);
            }

        }

        public static void SelectFirstAbility(Player player)
        {
            if(PlayersAbility.TryGetValue(player,out var list))
            {
                list[0].SelectAbility(player);

            }
            
        }

        public static void TryRemoveFromPlayer(Player player)
        {
            foreach(KEAbilities abilities in Registered)
            {
                if (abilities.Players.Contains(player))
                {
                    abilities.RemoveAbility(player);
                }
            }
        }




        #region register
        private bool TryRegister()
        {


            if (!Registered.Contains(this))
            {
                Registered.Add(this);
                Init();
                Log.Debug("registered " + Name);
                return true;
            }



            return false;
        }


        public static IEnumerable<KEAbilities> Register(Assembly assembly =null)
        {
            IEnumerable<KEAbilities> abilities = ReflectionHelper.GetObjects<KEAbilities>(assembly);
            List<KEAbilities> result = abilities.ToList();

            foreach(KEAbilities ability in abilities)
            {

                if (!ability.TryRegister())
                {
                    Log.Warn("couldn't register KEability " + ability);
                    result.Remove(ability);
                }
                
            }
            registered = result.ToHashSet();
            return result;

        }


        private bool TryUnregister()
        {
            Destroy();
            if (Registered.Remove(this))
            {
                Log.Debug("unregistered " + this);
                return true;
            }
            Log.Warn(this + " was not registered");
            return false;
        }

        public static IEnumerable<KEAbilities> Unregister()
        {
            List<KEAbilities> list = new();
            foreach (KEAbilities item in Registered)
            {
                item.TryUnregister();
                list.Add(item);
            }

            return list;
        }
        #endregion

        #region getters

        public static KEAbilities Get(string name)
        {
            foreach(KEAbilities kEAbilities in Registered)
            {
                if (name == kEAbilities.Name)
                {
                    return kEAbilities;
                }
            }

            return null;
        }

        public static bool TryGet(string name, out KEAbilities ability)
        {
            ability = Get(name);
            return ability != null;
        }

        public static KEAbilities GetSelected(Player player)
        {
            foreach(KEAbilities ability in Registered)
            {
                if (ability.Selected.Contains(player))
                {
                    return ability;
                }
            }
            return null;
        }

        public static bool TryGetSelected(Player player, out KEAbilities ability)
        {
            ability = GetSelected(player);
            return ability != null;
        }




        #endregion

        #region gui


        public const string ReadyText = "[READY]";
        private static AbilitiesPosition AbilityPosition = new();
        public static string UpdateGUI(Player player)
        {
            StringBuilder builder = StringBuilderPool.Pool.Get();
            

            List<KEAbilities> allAbilities = PlayersAbility[player];

            for (int i = 0; i < allAbilities.Count; i++)
            {


                KEAbilities ability = allAbilities[i];

                builder.Append(ability.PublicName);
                builder.Append(" ");
                

                if(ability is FireAbilityBase fire)
                {
                    builder.Append("(");
                    builder.Append(fire.Cost);
                    builder.Append(")");
                    builder.Append(" ");
                }



                if (ability.CanUse(player,out var output))
                {
                    builder.Append(ReadyText);
                }
                else
                {
                    DateTime dateTime = ability.LastUsed[player] + TimeSpan.FromSeconds(ability.Cooldown);
                    builder.Append("[");
                    builder.Append(Math.Round((dateTime - DateTime.Now).TotalSeconds, 0));
                    builder.Append("s]");
                }

                
                if (ability.Selected.Contains(player))
                {
                    string arrow = SettingHandler.Instance.GetArrow(player);
                    if (string.IsNullOrEmpty(arrow))
                    {
                        arrow = SettingHandler.baseArrow;
                    }
                    builder.Append(arrow);
                }
                builder.AppendLine();
            }

            string msg = builder.ToString();
            StringBuilderPool.Pool.Return(builder);
            return msg;
            //DisplayHandler.Instance.AddHint(MainPlugin.Abilities, player, msg, UpdateTime);
        }


        public static void ShowEffectHint(Player player, string text)
        {
            float delay = MainPlugin.SettingHandler.GetTime(player);
            DisplayHandler.Instance.AddHint(MainPlugin.CREffect, player, text, delay);
        }


        public static void ShowAbilityHint(Player player, string text)
        {
            float delay = MainPlugin.SettingHandler.GetTime(player);
            DisplayHandler.Instance.AddHint(MainPlugin.AbilitiesDesc, player, text, delay);
        }


        #endregion

        public override string ToString()
        {
            return Name;
        }

    }
}
