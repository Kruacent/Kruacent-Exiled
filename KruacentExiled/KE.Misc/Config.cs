using Exiled.API.Interfaces;
using System.ComponentModel;

namespace KE.Misc
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = true;
        [Description("Chance to d-boy doors goes boom")]
        public int ChanceClassDDoorGoesBoom { get; set; } = 2;
        [Description("Enable or disable the auto elevator")]
        public bool AutoElevator { get; set; } = true;
        [Description("Chance to get a pink candy (0-100)")]
        public int ChancePinkCandy { get; set; } = 10;
        public bool SurfaceLight { get; set; } = true;

        public bool ScpPreferences { get; set; } = true;
        public bool Scp035Enabled { get; set; } = true;
        public bool GamblingCoin { get; set; } = true;
        public int GamblingCoinMinUse { get; set; } = 1;
        public int GamblingCoinMaxUse { get; set; } = 2;
        public int GamblingCoinCooldown { get; set; } = 3;

        [Description("health mutiplicator scps)")]
        public float MultSCP049 { get; set; } = 0.8f;
        public float MultSCP939 { get; set; } = 1.2f;
        public float MultSCP106 { get; set; } = 1.1f;
        public int MinPlayerVote { get; set; } = 6;


        public string PatchNote { get; set; } = "-Ajout d'un truc à Surface\r\n-Patch note dans le lobby\r\n-Ajout d'un nouveau custom SCP :\r\nun 049 qui n'a pas de zombie mais des buff à la place\r\n-Ajout d'un bouton pour remontré la description d'un custom role\r\n-Ajout d'un nouveau custom role : Le pacifiste (ClassD et Scientifique)\r\n-Ajout d'un nouveau custom role pour SCP173\r\n-Ajout d'une lumière au dos du Terroriste\r\n-Ajout d'une indication pour SetPosition\r\nuniquement visible pour le joueur ayant utilisé l'ability\r\n-Mise à jour des pickups models de la TP Grenada et de la Mine\r\n-La nuke (ni deadman ni auto) ajoute un respawn à la faction qui l'a activé\r\net met le timer à ~45s du respawn\r\n-auto nuke : 30 min -> 20 min\r\n-SCP-173 gambling est de nouveau interactable\r\nmais les items ne drop plus (sauf par manque de place)\r\nle temps d'utilisation : 10s -> 5s (approx)\r\n-Ajout de traduction aux custom roles et aux abilités\r\n-Shield Belt: 110HP -> 210HP\r\nAjout d'une taille min à la sphere de protection\r\n-Molotov <=> Heal zone à 914\r\n-Enlevé SCP-202\r\n-Removed Sou Hiyori from the facility\r\n";




    }
}
