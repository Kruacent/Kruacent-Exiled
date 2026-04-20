using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KruacentExiled.CustomRoles.API.Features.Abilities
{
    public abstract class BaseCompAbility : MonoBehaviour
    {

        public abstract bool Active { get; }

        public abstract void ToggleActive();
    }
}
