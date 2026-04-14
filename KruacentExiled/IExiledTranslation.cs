using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KruacentExiled
{
    internal interface IExiledTranslation
    {
        public abstract ITranslation Translation { get; }
    }
}
