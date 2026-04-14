using Exiled.API.Features;
using Exiled.API.Features.Toys;
using LabApi.Features.Wrappers;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Player = Exiled.API.Features.Player;

namespace KE.Map.Others.EasterEggs
{
    internal class SpinnyBaras
    {
        private CapybaraToy _capybara;
        private CoroutineHandle _handle;

        private float speed = 100;
        public SpinnyBaras(Vector3 position)
        {
            _capybara = CapybaraToy.Create(position);
            _handle = Timing.RunCoroutine(Spin());
        }

        ~SpinnyBaras()
        {
            Timing.KillCoroutines(_handle);
        }
        private IEnumerator<float> Spin()
        {
            while (true)
            {
                _capybara.Transform.Rotate(Vector3.up, 1);
                yield return Timing.WaitForSeconds(1 / speed);
            }

        }


        public void Kill()
        {
            Timing.KillCoroutines(_handle);
        }

    }
}
