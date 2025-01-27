using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using KE.GlobalEventFramework.GEFE.API.Features;
using KE.GlobalEventFramework.GEFE.API.Interfaces;
using MEC;
using System.Collections.Generic;
using System.Linq;

namespace KE.GlobalEventFramework.Examples.GE
{
    /// <summary>
    /// Spawn fused grenades in random rooms in the map 
    /// </summary>
    public class CassieGoCrazy : GlobalEvent, IStart
    {
        ///<inheritdoc/>
        public override uint Id { get; set; } = 1049;
        ///<inheritdoc/>
        public override string Name { get; set; } = "Cassie Go Crazy";
        ///<inheritdoc/>
        public override string Description { get; set; } = "Crazy Cassie !";
        ///<inheritdoc/>
        public override int Weight { get; set; } = 1;

        /// <summary>
        /// The cooldown between 2 cassie event
        /// </summary>
        public int Cooldown { get; set; } = 280;

        /// <summary>
        /// Percentage for rare event to occur
        /// </summary>
        public int RareEvent { get; set; } = 7;

        /// <summary>
        /// Starts a coroutine to perform random actions during the game.
        /// </summary>
        /// <returns>A coroutine that runs until the game round ends.</returns>
        /// <inheritdoc />
        public IEnumerator<float> Start()
        {
            while (!Round.IsEnded)
            {
                Log.Debug("waiting");
                yield return Timing.WaitForSeconds(Cooldown);

                int action = UnityEngine.Random.Range(1, 6);

                switch (action)
                {
                    // Turns off the lights in the Heavy Containment zone.
                    case 1:
                        Map.TurnOffAllLights(20, Exiled.API.Enums.ZoneType.HeavyContainment);
                        break;

                    // Starting the Warhead
                    case 2:
                        Warhead.Start();
                        break;
                    
                    // Change Map Color to Cyan
                    case 3:
                        Map.ChangeLightsColor(UnityEngine.Color.cyan);
                        break;

                    // Sad Cassie scenario.
                    case 4:
                        Cassie.Message("I wanna just be useful", true, true, true);
                        Map.TurnOffAllLights(2, Exiled.API.Enums.ZoneType.HeavyContainment);
                        Map.TurnOffAllLights(2, Exiled.API.Enums.ZoneType.Entrance);
                        yield return Timing.WaitForSeconds(3);
                        Map.TurnOffAllLights(2, Exiled.API.Enums.ZoneType.HeavyContainment);
                        Map.TurnOffAllLights(2, Exiled.API.Enums.ZoneType.Entrance);

                        Warhead.Start();

                        for (int i = 0; i < 10; i++)
                        {
                            ((ExplosiveGrenade)Item.Create(ItemType.GrenadeHE)).SpawnActive(Room.Random().Position);
                        }

                        yield return Timing.WaitForSeconds(20);
                        Warhead.Stop();


                        break;

                    // Antagonistic Cassie scenario.
                    case 5:
                        List<Player> nonScpPlayers = Player.List.Where(player => !player.IsScp).ToList();

                        if (nonScpPlayers.Count > 0)
                        {
                            Player target = nonScpPlayers[UnityEngine.Random.Range(0, nonScpPlayers.Count)];
                            
                            Cassie.Message("New target : " +  target);

                            void OnPlayerDeath(DyingEventArgs ev)
                            {
                                if (ev.Player == target && ev.Attacker != null && ev.Attacker != target)
                                {
                                    if (!ev.Attacker.IsScp)
                                    {
                                        GiveRandomRewardPlayer(ev.Attacker);
                                    }

                                    Exiled.Events.Handlers.Player.Dying -= OnPlayerDeath;
                                }
                            }

                            Exiled.Events.Handlers.Player.Dying += OnPlayerDeath;
                        }
                    break;
                }

                /// <summary>
                /// Gives a random reward to the player who killed the target.
                /// </summary>
                /// <param name="player">The player who will receive the reward.</param>
                /// <inheritdoc />
                void GiveRandomRewardPlayer(Player player)
                {
                    var items = new List<ItemType>
                    {
                        ItemType.Jailbird,
                        ItemType.ParticleDisruptor,
                        ItemType.MicroHID,
                        ItemType.Coin,
                        ItemType.KeycardO5,
                        ItemType.SCP268
                    };

                    ItemType randomItem = items[UnityEngine.Random.Range(0, items.Count)];

                    player.AddItem(randomItem);

                    if (UnityEngine.Random.Range(0, 100) < RareEvent)
                    {
                        player.MaxHealth = 125;
                        player.Broadcast(5, "Another gift for you !");
                    }
                }


            }
        }

    }
}
