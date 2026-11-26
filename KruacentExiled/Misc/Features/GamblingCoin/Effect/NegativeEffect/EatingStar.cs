using Exiled.API.Features;
using KE.Utils.API.Features;
using KruacentExiled.Audio;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;

namespace KruacentExiled.Misc.Features.GamblingCoin.Effect.NegativeEffect
{
    public class EatingStar : IDurationEffect
    {
        public string Name { get; set; } = "EatingStar";
        public string Message { get; set; } = "Eating the disco ball wasn't good idea";
        public int Weight { get; set; } = 7;
        public EffectType Type { get; set; } = EffectType.Negative;



        public const string FileName = "starman";
        public const float CooldownLight = .5f;

        public float Duration { get; set; } = 20;
        private static CoroutineHandle _coroutines;

        public void Execute(Player player)
        {
            if (PlayerEatingStarData.IsRunning(player))
            {
                return;
            }

            PlayerEatingStarData.Start(player);

            if (!_coroutines.IsValid || !_coroutines.IsRunning)
            {
                _coroutines = Timing.RunCoroutine(ColorTransformer());
            }
        }

        public IEnumerator<float> ColorTransformer()
        {
            while (PlayerEatingStarData.Count > 0)
            {
                foreach(PlayerEatingStarData data in PlayerEatingStarData.Data)
                {
                    data.ChangeLightColor();
                }
                yield return Timing.WaitForSeconds(CooldownLight);
            }
        }



        public void ExecuteAfterDuration(Player player)
        {
            PlayerEatingStarData.Get(player)?.Stop();
        }


        private class PlayerEatingStarData
        {
            private static readonly Queue<PlayerEatingStarData> emptyDatas = new Queue<PlayerEatingStarData>();
            private Light light = null;
            private readonly List<AudioClipPlayback> clips = new List<AudioClipPlayback>();

        private static Dictionary<Player, PlayerEatingStarData> _data = new Dictionary<Player, PlayerEatingStarData>();
        public static IReadOnlyCollection<PlayerEatingStarData> Data => _data.Values;
        public static int Count => _data.Count;
        private Player _player;
        public static bool IsRunning(Player player)
        {
            return _data.ContainsKey(player);
        }

            public static PlayerEatingStarData Get(Player player)
            {
                PlayerEatingStarData result = null;
                _data.TryGetValue(player, out result);

                return result;
            }

            public static void Start(Player player)
            {
                PlayerEatingStarData result;

                if (emptyDatas.Count > 0)
                {
                    result = emptyDatas.Dequeue();
                }
                else
                {
                    result = new PlayerEatingStarData();
                }
                try
                {
                    result.InternalStart(player);
                }
                catch(Exception e)
                {
                    Log.Error(e);
                }
            }


            private void InternalStart(Player player)
            {
                var c = AudioHandler.Instance.PlayToAll(SoundType.Noise, FileName, player.GameObject, 50);

                foreach (AudioClipPlayback clip in c.Values)
                {
                    clips.Add(clip);
                }

                Light light = Light.Create(player.Position, null, null, true, UnityEngine.Color.blue);
                light.Transform.parent = player.GameObject.transform;
                light.MovementSmoothing = 0;
                this.light = light;

                _data.Add(player, this);
            }

            public void ChangeLightColor()
            {
                if(light == null)
                {
                    throw new NullReferenceException("light null");
                }
                light.Color = ColorPicker();
            }

            private static Color32 ColorPicker()
            {
                byte r = (byte)UnityEngine.Random.Range(0, 256);
                byte g = (byte)UnityEngine.Random.Range(0, 256);
                byte b = (byte)UnityEngine.Random.Range(0, 256);

                return new Color32(r, g, b, 0);
            }

        public void Stop()
        {
            foreach(AudioClipPlayback audioClipPlayback in clips)
            {
                audioClipPlayback.Loop = false;
            }

            light?.Destroy();
            light = null;
            clips.Clear();
            _data.Remove(_player);
            _player = null;
            emptyDatas.Enqueue(this);
        }
    }
}