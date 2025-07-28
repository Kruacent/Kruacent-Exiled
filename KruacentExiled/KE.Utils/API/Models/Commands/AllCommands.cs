using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KE.Utils.API.Models.Commands
{
    public static class AllCommands
    {

        public static IEnumerable<ICommand> Get(Assembly assembly = null)
        {
            return new List<ICommand>()
            {
                new ChangeColor(),
                new ChangePrimType(),
                new CreateModel(),
                new CreatePrim(),
                new ListModel(),
                new LoadModel(),
                new ModeMovePrim(),
                new SelectModel(),
                new ShowCenter(),
                new SaveModel()

            };

        }
    }
}
