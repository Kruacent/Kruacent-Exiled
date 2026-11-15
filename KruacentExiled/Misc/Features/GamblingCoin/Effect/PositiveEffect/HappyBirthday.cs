using Exiled.API.Extensions;
using Exiled.API.Features;
using KE.Utils.API.Features;
using KruacentExiled.Audio;
using KruacentExiled.CustomItems.API.Features;
using KruacentExiled.Misc;
using KruacentExiled.Misc.Features.GamblingCoin.Interfaces;
using KruacentExiled.Misc.Features.GamblingCoin.Types;
using MEC;
using System.Collections.Generic;
using UnityEngine;

internal class HappyBirthday : ICoinEffect
{
    public string Name { get; set; } = "HappyBirthday";
    public string Message { get; set; } = string.Empty;
    public int Weight { get; set; } = 8;
    public EffectType Type { get; set; } = EffectType.Positive;
    private static readonly Queue<Player> _birthdayQueue = new Queue<Player>();
    private static CoroutineHandle _currentBirthday;
    private static bool _isBirthdayActive = false;
    private float Duration { get; set; } = 19f;
    private float Interval { get; set; } = 1;

    private List<Color> _colorList = new List<Color>() {
        Color.red,
        Color.blue,
        Color.green,
        Color.gray,
        Color.cyan,
        Color.magenta,
    };

    public void Execute(Player player)
    {
        _birthdayQueue.Enqueue(player);
        if (!_isBirthdayActive)
        {
            _currentBirthday = Timing.RunCoroutine(ProcessQueue());
        }

    }

    private IEnumerator<float> ProcessQueue()
    {
        _isBirthdayActive = true;

        while (_birthdayQueue.Count > 0)
        {
            Player player = _birthdayQueue.Dequeue();

            if (player == null || !player.IsConnected)
            {
                continue;
            }

            Dictionary<string, string> customBirthday = MainPlugin.Configs.CustomBirthdayMusicPerPlayer;
            customBirthday.TryGetValue(player.UserId, out string happyBirthdayMusicFile);

            if (string.IsNullOrEmpty(happyBirthdayMusicFile))
            {
                happyBirthdayMusicFile = MainPlugin.Configs.DefaultBirthdayMusic;
            }

            if (KruacentExiled.Misc.Utils.RandomNumberGenerator.RollChance(5, player, true))
            {
                happyBirthdayMusicFile = "birthday_surprise";
            }

            KELog.Debug(happyBirthdayMusicFile);

            foreach (Player p in Player.List)
            {
                AudioHandler.Instance.Play(p, SoundType.Music, happyBirthdayMusicFile, 50f);
                KECustomItem.ItemEffectHint(p, $"It's {player.Nickname}'s birthday !!");
            }


            float elapsed = 0f;

            while (elapsed < this.Duration)
            {
                Map.ChangeLightsColor(_colorList.GetRandomValue());

                elapsed += this.Interval;
                yield return Timing.WaitForSeconds(this.Interval);
            }

            Map.ResetLightsColor();
        }

        _isBirthdayActive = false;
    }



}