using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using KE.Utils.API.Sounds;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KE.Map.Surface.BlinkingBlocks
{
    public class BlinkingBlocksGroup : IPosition
    {
        private AudioPlayer player;
        public Vector3 Position { get; }
        public BlockColor Color { get; private set; }

        private HashSet<BlinkingBlock> _list;
        public BlinkingBlocksGroup(IEnumerable<BlinkingBlock> blocks)
        {
            _list = new(blocks);

            Vector3 center = new(_list.Average(x => x.Position.x), _list.Average(x => x.Position.y),_list.Average(x => x.Position.z));


            
            var a = new GameObject();
            a.transform.position = center;
            Timing.RunCoroutine(Switch());
            KE.Utils.API.Sounds.SoundPlayer.Instance.Play("beep", a, 50, 50);
        }

        private BlockColor GetNewColor()
        {
            if(Color == BlockColor.Red)
            {
                return BlockColor.Blue;
            }
            else
            {
                return BlockColor.Red;
            }
        }

        private IEnumerator<float> Switch()
        {
            while (true)
            {
                
                yield return Timing.WaitForSeconds(5);
                foreach (BlinkingBlock b in _list)
                {
                    b.Switch(Color);
                }
                //Log.Debug("switch to "+Color.ToString());
                Color = GetNewColor();
            }
        }
        
    }
}
