using System;
using CommandSystem;
using KruacentExiled.GlobalEventFramework.GEFE.API.Features;
using System.Text;
using Exiled.API.Features.Pools;
using System.Linq;
using System.Collections.Generic;

namespace KruacentExiled.GlobalEventFramework.GEFE.Commands
{
    public class List : ICommand
    {
        public string Command { get; } = "list";
        public string[] Aliases { get; } = new string[] { "l", "ls" };
        public string Description { get; } = "get the list of all Global Events";


        public static string Activated => "[o]";
        public static string NotActivated => "[ ]";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            StringBuilder builder = StringBuilderPool.Pool.Get();
            builder.AppendLine();




            List<GlobalEvent> glist = GlobalEvent.GlobalEventsList.ToList();

            builder.Append("Global Events (");
            builder.Append(glist.Count);
            builder.Append(")");


            foreach(GlobalEvent ge in glist)
            {
                builder.AppendLine();
                if (ge.IsActive)
                {
                    builder.Append(Activated);
                }
                else
                {
                    builder.Append(NotActivated);
                }
                Show(builder, ge);
            }
            builder.AppendLine();
            List<MiddleEvent> mlist = MiddleEvent.MiddleEventsList.ToList();
            builder.Append("Middle Event (");
            builder.Append(mlist.Count);
            builder.Append(")");
            
            foreach (MiddleEvent me in mlist)
            {
                builder.AppendLine();
                if (me.IsActive)
                {
                    builder.Append(Activated);
                }
                else
                {
                    builder.Append(NotActivated);
                }
                Show(builder, me);
            }

            response = builder.ToString();
            StringBuilderPool.Pool.Return(builder);
            return true;
        }



        private void Show(StringBuilder builder, KEEvents events)
        {

            builder.Append(" (");
            builder.Append(events.Id);
            builder.Append(") ");
            builder.Append(events.Name);
            builder.Append(" - ");
            builder.Append(events.WeightedChance);
        }
    }
}
