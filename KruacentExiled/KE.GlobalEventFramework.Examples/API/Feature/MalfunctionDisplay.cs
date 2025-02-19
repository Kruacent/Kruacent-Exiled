using Exiled.API.Features;
using InventorySystem.Items.Firearms.Modules;
using KE.GlobalEventFramework.Examples.API.Interfaces;
using KE.Utils;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.GlobalEventFramework.Examples.API.Feature
{
    public class MalfunctionDisplay
    {
        private Malfunctions _malfunction;
        public float RefreshRate { get; set; } = 5;
        private CoroutineHandle _coroutineHandle;
        public MalfunctionDisplay(Malfunctions malfunction)
        {
            _malfunction = malfunction;
            _coroutineHandle = Show();
        }

        ~MalfunctionDisplay()
        {
            Timing.KillCoroutines(_coroutineHandle);
        }

        public CoroutineHandle Show()
        {
            return Timing.RunCoroutine(Tick());
        }

        private IEnumerator<float> Tick()
        {
            while (Round.InProgress)
            {
                yield return Timing.WaitForSeconds(RefreshRate);
                ShowAllSpect(GetHint());
            }
        }



        private void ShowAllSpect(RueIHint hint)
        {

            foreach (Player p in Player.List.Where(p => p.Role == RoleTypeId.Spectator))
            {
                
                p.ShowHint(hint);
            }
        }

        private RueIHint GetHint()
        {
            string content = $"<align=right> {GetCurrentMalfunction()}\n{GetAllEffect()}</align>";
            return new RueIHint(content,RefreshRate+.1f,true,800);
        }


        private string GetCurrentMalfunction()
        {
            sbyte malfunction = _malfunction.Malfunction;
            sbyte previous = _malfunction.PreviousMalfunction;
            if (malfunction > previous)
                return $"<color=#ff0000> {malfunction}\u2191 (+{malfunction-previous})</color>";
            if(malfunction < previous)
                return $"<color=#00ff00> {malfunction}\u2193 ({malfunction - previous})</color>";
            else 
                return $"<color=#ffffff> {malfunction}\u2192 ({malfunction - previous})</color>";
            
        }

        private string GetAllEffect()
        {
            string result = string.Empty;
            foreach (MalfunctionEffect me in Malfunctions.MalfunctionEffects)
            {
                if (Malfunctions.EffectAlreadyActivated(me))
                {
                    if (IsREActivated(me)) result += "<color=#f00>";
                    result += $"{me.MalfunctionActivation} - {me.Name}";
                    if (IsREActivated(me)) result += "</color>";
                }

            }
            return result;
        }
        private bool IsREActivated(MalfunctionEffect me)
        {
            if(me is IReversibleEffect re)
            {
                if(_malfunction.Malfunction >= re.MalfunctionDeactivation)
                {
                    return true;
                }
            }

            return false;
        }
        
    }
}
