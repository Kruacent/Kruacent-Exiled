using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp049;
using KE.GlobalEventFramework.Examples.API.Interfaces;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.NonAllocLINQ;

namespace KE.GlobalEventFramework.Examples.API.Feature
{
    public class Malfunctions
    {

        private sbyte _malfunction = 15;
        public const sbyte Lower = -50;
        public const sbyte Higher = 125;
        public sbyte Malfunction
        {
            get { return _malfunction; }
            set
            {
                if (value > Higher) _malfunction = Higher;
                else if (value < Lower) _malfunction = Lower;
                else _malfunction = value;
            }
        }

        public MalfunctionLevel MalfunctionLevels
        {
            get{return (MalfunctionLevel) Malfunction;}
        }

        private static HashSet<MalfunctionEffect> _malfunctionEffects = new HashSet<MalfunctionEffect>();
        public static HashSet<MalfunctionEffect> MalfunctionEffects => _malfunctionEffects.ToList().ToHashSet();
        private static Dictionary<MalfunctionEffect, bool> _voiced;
        private static Dictionary<MalfunctionEffect, bool> _voicedDeactivate;

        public static bool EffectAlreadyActivated(MalfunctionEffect effect)
        {
            return _voiced[effect];
        }

        public sbyte PreviousMalfunction { get; private set; }
        public sbyte MalfunctionAdd { get; set; } = 1;
        public MalfunctionDisplay MalfunctionDisplay { get; private set; }
        
        
        internal Malfunctions(bool display = true)
        {
            try
            {
                if (display)
                    MalfunctionDisplay = new MalfunctionDisplay(this);
                else
                    MalfunctionDisplay = null;
            }
            catch (Exception ex)
            {
                Log.Error(ex + "\nRueI is probably missing : put it in dependency or disable the display");
                MalfunctionDisplay = null;
            }
           
            LoadMalfunctionsEffect();
        }

        public bool IsDisplayEnabled()
        {
            return MalfunctionDisplay != null;
        }

        private void LoadMalfunctionsEffect()
        {
            foreach (IPlugin<IConfig> plugin in Exiled.Loader.Loader.Plugins.Where(pl => pl.Name != MainPlugin.Instance.Name))
            {
                foreach (Type type in plugin.Assembly.GetTypes())
                {
                    try
                    {
                        if (type.IsSubclassOf(typeof(MalfunctionEffect)))
                        {
                            MalfunctionEffect me = Activator.CreateInstance(type) as MalfunctionEffect;
                            _malfunctionEffects.Add(me);
                        }
                    }
                    catch (System.Exception e)
                    {
                        Log.Error($"Error registering in plugin {plugin.Name} : {e.Message}");
                    }
                }
            }
            _voiced = _malfunctionEffects.ToDictionary(m => m, m => false);
            _voicedDeactivate = _malfunctionEffects.Where(m => m is IReversibleEffect).ToDictionary(m => m, m => false);
        }


        internal IEnumerator<float> Tick()
        {
            while (Round.InProgress)
            {
                PreviousMalfunction = Malfunction;
                Malfunction += MalfunctionAdd;
                Malfunction += AdditionnalMalfunction();
                CheckMalfunctionEffect(Malfunction);
                Log.Debug($"Malfunction={Malfunction}");
                yield return Timing.WaitForSeconds(60);
            }
        }



        private void CheckMalfunctionEffect(sbyte malfunction)
        {
            foreach (MalfunctionEffect me in _malfunctionEffects)
            {
                if (malfunction >= me.MalfunctionActivation)
                {
                    if (!_voiced[me])
                    {
                        _voiced[me] = true;
                        Cassie.MessageTranslated(me.VoiceLine,me.VoiceLineTranslated,false,false);
                    }
                    me.ActivateEffect();
                }

                if(me is IReversibleEffect re && malfunction < re.MalfunctionDeactivation)
                {
                    if (!_voicedDeactivate[me])
                    {
                        _voicedDeactivate[me] = true;
                        Cassie.MessageTranslated(re.VoiceLineDeactivate, re.VoiceLineDeactivateTranslated, false, false);
                    }
                    re.DeactivateEffect();
                }
            }

        }

        


        private sbyte AdditionnalMalfunction()
        {
            sbyte result = (sbyte)UnityEngine.Random.Range(-2, 3);
            //generator reduce the malfunction
            result -= (sbyte)(Generator.List.Count(x => x.IsEngaged) * 3);
            //number of scp increase 3 (except zombies)
            result += (sbyte)(Player.List.Count(p => p.Role.Side == Side.Scp && p.Role != RoleTypeId.Scp0492) * 3);
            //number of zombies increase 1
            result += (sbyte)Player.List.Count(p => p.Role == RoleTypeId.Scp0492);
            return result;

        }


        internal void OnDying(DyingEventArgs ev)
        {
            switch (ev.Player.Role.Side)
            {
                case Side.Mtf:
                    Malfunction += 1;
                    break;
                case Side.ChaosInsurgency:
                    Malfunction -= 1;
                    break;
                case Side.Scp:
                    if (ev.Player.Role != RoleTypeId.Scp0492) Malfunction -= 10;
                    else Malfunction -= 1;
                    break;
            }
        }

        internal void OnFinishingRevive(FinishingRecallEventArgs ev)
        {
            Malfunction += 3;
        }



        public enum MalfunctionLevel
        {
            VeryLowMalfunction = 0,
            LowMalfunction = 25,
            MediumMalfunction = 50,
            HighMalfunction = 75,
            VeryHighMalfunction = 100,
        }
    }
}
