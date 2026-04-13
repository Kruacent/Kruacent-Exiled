using Exiled.API.Features;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Models.HintContent;
using KE.CustomRoles.CR.CustomSCPs.SCP049C.Positions;
using KE.CustomRoles.CR.CustomSCPs.SCP049C.UnlockableAbilities;
using KE.Utils.API.Displays.DisplayMeow;
using KE.Utils.API.Displays.DisplayMeow.Placements;
using NorthwoodLib.Pools;
using PlayerRoles.FirstPersonControl.Thirdperson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE.CustomRoles.CR.CustomSCPs.SCP049C
{
    public class SCP049CGUI : IDisposable
    {

        private SCP049CLevelSystem _lvl;

        private Player player => Player.Get(_lvl.Hub);
        public static readonly HintPosition HintTitlePosition = new SCP049CUnlockableTitlePosition();
        public static readonly HintPosition HintLeftPosition = new SCP049CUnlockableLeftPosition();
        public static readonly HintPosition HintRightPosition = new SCP049CUnlockableRightPosition();


        private int timeRemaining => _lvl.TimeRemaining;
        public SCP049CGUI(SCP049CLevelSystem lvl)
        {
            _lvl = lvl;
        }

        public Unlockable[] UnlockableOnScreen { get; private set; }
        public void CreateHints(List<Unlockable> ability)
        {
            UnlockableOnScreen = ability.ToArray();
            CreateAutoIfNotExist(player, HintTitlePosition, (arg) => Title(player),HintSyncSpeed.Normal);
            CreateAutoIfNotExist(player, HintLeftPosition, (arg) => GetContent(player, HintLeftPosition.HintPlacement), HintSyncSpeed.Fastest);
            CreateAutoIfNotExist(player, HintRightPosition, (arg) => GetContent(player, HintRightPosition.HintPlacement), HintSyncSpeed.Fastest);
        }

        private void CreateAutoIfNotExist(Player player,HintPosition hintPosition, AutoContent.TextUpdateHandler update, HintSyncSpeed hintSyncSpeed)
        {
            if (!DisplayHandler.Instance.HasHint(player, hintPosition.HintPlacement))
            {
                DisplayHandler.Instance.CreateAuto(player, update, hintPosition.HintPlacement, hintSyncSpeed);
            }
        }

        public void DestroyHints()
        {
            DisplayHandler.Instance.RemoveHint(player, HintTitlePosition.HintPlacement);
            DisplayHandler.Instance.RemoveHint(player, HintLeftPosition.HintPlacement);
            DisplayHandler.Instance.RemoveHint(player, HintRightPosition.HintPlacement);
            UnlockableOnScreen = null;
        }


        private const string TitleMessage = "Choose an ability! (%seconds%s)\n use Right and Left (Server settings)";


        private string Title(Player player)
        {
            return TitleMessage.Replace("%seconds%", timeRemaining.ToString());
        }


        public const int FontSize = 15;

        private string GetContent(Player player,HintPlacement hintPlacement)
        {
            StringBuilder sb = StringBuilderPool.Shared.Rent();
            string msg;
            try
            {

                int abilityShowingIndex;

                if (hintPlacement.XCoordinate < 0)
                {
                    abilityShowingIndex = 0;
                }
                else
                {
                    abilityShowingIndex = 1;
                }




                Unlockable unlockable= UnlockableOnScreen[abilityShowingIndex];

                bool flag = _lvl.Selected == abilityShowingIndex;

                if (flag)
                {
                    sb.AppendLine("<b>");
                }
                else
                {
                    sb.AppendLine("<color=#8c8c8c>");
                }


                    sb.AppendLine(unlockable.GetName(player.ReferenceHub));
                if (flag)
                {
                    sb.AppendLine("</b>");
                }
                else
                {
                    sb.AppendLine("</color>");
                }

                    sb.Append("<size=");
                sb.Append(FontSize);
                sb.Append(">");
                sb.Append(unlockable.GetDescription(player.ReferenceHub));
                sb.Append("</size>");
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
            finally
            {
                msg = sb.ToString();

                StringBuilderPool.Shared.Return(sb);
                if (string.IsNullOrEmpty(msg))
                {
                    msg = " ";
                }
            }
            return msg;

        }




        public void Dispose()
        {
            if (player != null)
            {
                DestroyHints();
            }
        }
    }
}
