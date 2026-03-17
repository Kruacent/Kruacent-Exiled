using Exiled.API.Features;
using Exiled.API.Features.Pools;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Extension;
using HintServiceMeow.Core.Models.Arguments;
using HintServiceMeow.Core.Models.Hints;
using KE.CustomRoles.Abilities.FireAbilities;
using KE.CustomRoles.API.HintPositions;
using KE.CustomRoles.API.Interfaces;
using KE.CustomRoles.API.Interfaces.Ability;
using KE.CustomRoles.Settings;
using KE.Utils.API;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using KE.Utils.API.Features;
using KE.Utils.API.Translations;
using MEC;
using PlayerRoles.FirstPersonControl.Thirdperson;
using PlayerRoles.PlayableScps.HUDs;
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

        public string TranslationKeyName => Name+"_"+ AbilityNameKey;
        public string TranslationKeyDesc => Name+"_"+ AbilityDescriptionKey;

        /// <summary>
        /// in seconds
        /// </summary>
        public abstract float Cooldown { get; }
        #endregion

        private Dictionary<Player, DateTime> LastUsed = new();
        private static Dictionary<System.Type, KEAbilities> TypeToAbility { get; } = new();
        private static Dictionary<string, KEAbilities> NameToAbility { get; } = new();
        public HashSet<Player> Players { get; } = new HashSet<Player>();
        public IReadOnlyCollection<Player> Selected => selected;
        private HashSet<Player> selected = new();
        private HashSet<Player> blockedPlayer = new();
        private HashSet<Player> playerWithActiveAbility = new();



        public static Dictionary<Player, List<KEAbilities>> PlayersAbility { get;} = new();




        protected const string AbilityNameKey = "Name";
        protected const string AbilityDescriptionKey = "Desc";

        public const string AbilityTranslationId = "Ability";
        protected KEAbilities()
        {
            
        }


        private bool activated = false;
        private void OneTimeInit()
        {
            if (activated) return;
            TranslationHub.Add(AbilityTranslationId, "en", "AbilityReady", "READY");
            TranslationHub.Add(AbilityTranslationId, "fr", "AbilityReady", "PRÊT");


            activated = true;
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

            OneTimeInit();
            InternalSubscribeEvent();

            var translate = SetTranslation();


            TranslationHub.Add(AbilityTranslationId, translate);
        }


        protected abstract Dictionary<string, Dictionary<string, string>> SetTranslation();

        public static string GetTranslation(Player player, string key)
        {
            return TranslationHub.Get(player, AbilityTranslationId, key);
        }


        public static void ShowEffectHint(Player player,string translationKey)
        {

            string text = GetTranslation(player, translationKey);

            float delay = MainPlugin.SettingHandler.GetTime(player);
            DisplayHandler.Instance.AddHint(MainPlugin.CREffect, player, text, delay);
        }

        public static void ShowEffectHint(Player player, string text, float delay)
        {
            delay = MainPlugin.SettingHandler.GetTime(player);
            DisplayHandler.Instance.AddHint(MainPlugin.CREffect, player, text, delay);
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
            sb.Append(GetTranslation(player, TranslationKeyName));
            sb.Append("</b>");
            sb.AppendLine();
            sb.Append(GetTranslation(player, TranslationKeyDesc));

            


            DisplayHandler.Instance.AddHint(MainPlugin.AbilitiesDesc, player, sb.ToString(), time);
            StringBuilderPool.Pool.Return(sb);
        }

        public void SelectAbility(Player player, bool hide = false)
        {
            if (selected.Add(player))
            {
                Log.Debug($"player {player.Nickname} selected ability {this}");
                foreach (KEAbilities abilities in Registered.Where(a => a != this && a.Players.Contains(player)))
                {
                    abilities.UnselectAbility(player);
                }
                if (!hide)
                {
                    ShowAbility(player);
                }
                
            }
        }

        public void UnselectAbility(Player player)
        {
            if (selected.Remove(player))
            {
                KELog.Debug($"player {player.Nickname} unselected ability {this}");
            }
            
        }
        
        public void RemoveAbility(Player player)
        {

            if (Players.Contains(player))
            {
                KELog.Debug($"player {player.Nickname} lost {this}");
                PlayersAbility[player].Remove(this);
                Players.Remove(player);
                UnselectAbility(player);
                AbilityRemoved(player);
            }
        }
        public virtual void AddAbility(Player player)
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
                InitHints(player);


                AbilityAdded(player);
            }
            
        }
        public void UseAbility(Player player)
        {
            if (AbilityUsed(player))
            {
                if (this is IDuration durationAbility)
                {
                    this.playerWithActiveAbility.Add(player);
                    Timing.CallDelayed(durationAbility.Duration, () =>
                    {
                        durationAbility.ActionAfterAbility(player);
                        this.playerWithActiveAbility.Remove(player);
                    });
                }

                LastUsed[player] = DateTime.Now;
            }
        }

        public bool IsAbilityActive(Player player)
        {
            return this.playerWithActiveAbility.Contains(player);
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
            if (this.blockedPlayer.Contains(player))
            {
                result = "blocked";
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
                Log.Debug("got selected " + ability.Name);
                if (ability.CanUse(player, out string _))
                {
                    Log.Debug("can use");
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
                ability.selected.Remove(player);
            }

        }

        public static void SelectFirstAbility(Player player,bool hide = false)
        {
            if(PlayersAbility.TryGetValue(player,out var list))
            {
                list[0].SelectAbility(player);

            }
            
        }

        public static void TryRemoveFromPlayer(Player player)
        {
            RemoveAllSelect(player);
            foreach(KEAbilities abilities in Registered)
            {
                if (abilities.Players.Contains(player))
                {
                    abilities.RemoveAbility(player);
                }
            }
        }

        public static void TemporaryRemoveAbilities(Player player)
        {
            foreach(KEAbilities ability in PlayersAbility[player])
            {
                ability.blockedPlayer.Add(player);
            }
        }

        public static void ReaffectRemovedAbilities(Player player)
        {
            foreach (KEAbilities ability in PlayersAbility[player])
            {
                ability.blockedPlayer.Remove(player);
            }
        }


        public bool IsSelected(Player player)
        {
            return Selected.Contains(player);
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
                if (ability.IsSelected(player))
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


        
        protected void GuiReady(StringBuilder sb, Player player)
        {

            if (CanUse(player, out var output))
            {
                sb.Append("[");
                sb.Append(GetTranslation(player, "AbilityReady"));
                sb.Append("]");
            }
            else
            {
                DateTime dateTime = LastUsed[player] + TimeSpan.FromSeconds(Cooldown);
                sb.Append("[");
                sb.Append(Math.Round((dateTime - DateTime.Now).TotalSeconds, 0));
                sb.Append("s]");
            }
        }

        protected void GuiArrow(StringBuilder sb, Player player)
        {
            if (IsSelected(player))
            {
                string arrow = SettingHandler.Instance.GetArrow(player);
                if (string.IsNullOrEmpty(arrow))
                {
                    arrow = SettingHandler.baseArrow;
                }
                sb.Append(arrow);
            }
        }

        protected void GuiAbilityName(StringBuilder sb, Player player)
        {
            sb.Append(GetTranslation(player, TranslationKeyName));
            sb.Append(" ");
        }

        /// <summary>
        /// The ability gui without the arrow or the ready
        /// </summary>
        protected virtual void AbilityGui(StringBuilder sb, Player player)
        {
            GuiAbilityName(sb, player);
            
        }

        /// <summary>
        /// The full ability gui
        /// </summary>
        protected virtual void Gui(StringBuilder sb,Player player)
        {
            IConditional conditional = this as IConditional;

            if(conditional != null)
            {
                if (conditional.CheckCondition(player))
                {
                    sb.Append("<color=#FFFFFF>");
                }
                else
                {
                    sb.Append("<color=#454545>");
                }
            }
            else
            {
                sb.Append("<color=#FFFFFF>");
            }

            AbilityGui(sb, player);
            GuiReady(sb, player);
            GuiArrow(sb, player);

            sb.Append("</color>");
        }


        public static Dictionary<Player, List<AbstractHint>> PlayersHints { get; } = new();
        public static Dictionary<AbstractHint, AbstractHint> AddonHints { get; } = new();
        public const int InitialAbilitySlot = 5;
        private void InitHints(Player player)
        {


            if (!PlayersHints.TryGetValue(player, out var _))
            {
                PlayersHints.Add(player, new());
                for (int i = 0; i < InitialAbilitySlot; i++)
                {
                    AbilitiesPosition position = AbilitiesPosition.GetIndex(i);
                    AbstractHint hint = DisplayHandler.Instance.CreateAuto(player, arg => UpdateHint(arg), position.HintPlacement);
                    PlayersHints[player].Add(hint);
                    AddonHints.Add(hint, DisplayHandler.Instance.CreateAuto(player, arg => UpdateAddon(arg,hint), AddonAbilitiesPosition.Get(position).HintPlacement,HintSyncSpeed.Slow));
                }



            }

        }

        private static HashSet<KEAbilities> addonAbilitiesexception = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="hint">the other hint</param>
        /// <returns></returns>
        private static string UpdateAddon(AutoContentUpdateArg arg,AbstractHint hint)
        {
            Player player = Player.Get(arg.PlayerDisplay.ReferenceHub);
            int index = PlayersHints[player].FindIndex(h => h == hint);

            if (PlayersAbility[player].TryGet(index, out KEAbilities ability))
            {
                if (ability is ICustomIcon icon && !addonAbilitiesexception.Contains(ability))
                {
                    StringBuilder sb = StringBuilderPool.Pool.Get();
                    try
                    {
                        sb.Append(icon.IconName.RawString);
                    }
                    catch(KeyNotFoundException)
                    {
                        addonAbilitiesexception.Add(ability);
                        Log.Error("Icon for ability "+ ability.Name+ " is missing");
                        return " ";
                    }
                    return StringBuilderPool.Pool.ToStringReturn(sb);
                }
            }
            return " ";
        }





        private static string UpdateHint(AutoContentUpdateArg arg)
        {
            Player player = Player.Get(arg.PlayerDisplay.ReferenceHub);
            int index = PlayersHints[player].FindIndex(h => h == arg.Hint);



            if (PlayersAbility[player].TryGet(index,out KEAbilities ability))
            {
                
                StringBuilder sb = StringBuilderPool.Pool.Get();
                ability.Gui(sb,player);
                return StringBuilderPool.Pool.ToStringReturn(sb);
            }



            return " ";
        }


        public static string UpdateGUI(Player player)
        {
            StringBuilder builder = StringBuilderPool.Pool.Get();
            

            List<KEAbilities> allAbilities = PlayersAbility[player];

            for (int i = 0; i < allAbilities.Count; i++)
            {


                KEAbilities ability = allAbilities[i];
                ability.Gui(builder,player);


            }

            string msg = builder.ToString();
            StringBuilderPool.Pool.Return(builder);
            return msg;
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
