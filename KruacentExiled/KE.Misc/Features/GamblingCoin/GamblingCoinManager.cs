using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.Misc.Features.GamblingCoin.Interfaces;
using KE.Misc.Features.GamblingCoin.Types;
using KE.Utils.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KE.Misc.Features.GamblingCoin
{
    public static class GamblingCoinManager
    {
        private static readonly Dictionary<string, ICoinEffect> _nameLookup = new();
        private static readonly HashSet<ICoinEffect> _activeEffects = new();

        public static List<ICoinEffect> EffectList { get; private set; } = new();

        public static IEnumerable<ICoinEffect> RegisterAll()
        {
            List<Assembly> assemblies = new();
            foreach (var plugin in Exiled.Loader.Loader.Plugins)
                assemblies.Add(plugin.Assembly);

            IEnumerable<ICoinEffect> effects = ReflectionHelper.GetObjects<ICoinEffect>(assemblies)
                .Where(e => e != null);

            foreach (var effect in effects)
            {
                try
                {
                    Register(effect);
                }
                catch (Exception ex)
                {
                    Log.Error($"[GamblingCoin] Failed to register {effect.Name} : {ex}");
                }
            }

            BuildWeightedList();

            return effects;
        }

        public static void Register(ICoinEffect effect)
        {
            if (_nameLookup.ContainsKey(effect.Name))
                throw new Exception($"Effect with Name {effect.Name} is already registered.");

            _nameLookup.Add(effect.Name, effect);

            if (effect.Weight > 0)
                _activeEffects.Add(effect);
                
            Log.Debug($"[GamblingCoin] Registered: {effect.Name}");

        }

        private static void BuildWeightedList()
        {
            EffectList.Clear();
            foreach (var effect in _activeEffects)
            {
                for (int i = 0; i < effect.Weight; i++)
                    EffectList.Add(effect);
            }
        }

        public static void DestroyAll()
        {
            _nameLookup.Clear();
            _activeEffects.Clear();
            EffectList.Clear();
        }

        public static ICoinEffect GetRandomEffect()
        {
            if (EffectList.Count == 0) return null;

            return (EffectList.GetRandomValue());
        }

        public static ICoinEffect GetRandomEffect(EffectType type)
        {
            if(EffectList.Count == 0) return null;

            return(EffectList.Where(e => e.Type == type).GetRandomValue());
        }
    }
}