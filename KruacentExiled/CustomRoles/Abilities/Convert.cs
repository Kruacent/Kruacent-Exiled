using DrawableLine;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Toys;
using Exiled.CustomRoles.API.Features;
using HintServiceMeow.UI.Utilities;
using KE.Utils.API.GifAnimator;
using KruacentExiled.ClientPrimitives;
using KruacentExiled.CustomItems.Items.ItemEffects;
using KruacentExiled.CustomRoles.API.Features;
using KruacentExiled.CustomRoles.API.Interfaces;
using KruacentExiled.CustomRoles.CustomSCPTeam;
using MEC;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using ProjectMER.Commands.Modifying.Scale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace KruacentExiled.CustomRoles.Abilities
{
    public class Convert : KEAbilities, ICustomIcon
    {
        public override string Name { get;  } = "Convert";


        public const string ConvertNobodyTranslation = "ConvertNobody";
        public const string ConvertSameTeamTranslation = "ConvertSameTeam";
        public const string ConvertNonZombieTranslation = "ConvertNonZombie";
        public const string ConvertSuccessTranslation = "ConvertSuccess";


        protected override Dictionary<string, Dictionary<string, string>> SetTranslation()
        {
            return new Dictionary<string, Dictionary<string, string>>()
            {
                ["en"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Convert",
                    [TranslationKeyDesc] = "Convert a zombie to your team",
                    [ConvertNobodyTranslation] = "But nobody's here",
                    [ConvertSameTeamTranslation] = "I know you don't like them, but they're in your team",
                    [ConvertNonZombieTranslation] = "That ain't a zombie",
                    [ConvertSuccessTranslation] = "New friend acquired!",
                },
                ["fr"] = new Dictionary<string, string>()
                {
                    [TranslationKeyName] = "Convert",
                    [TranslationKeyDesc] = "Converti un zombie à la bonne foi",
                    [ConvertNobodyTranslation] = "Mais personne n'est venu",
                    [ConvertSameTeamTranslation] = "Je sais que tu l'aimes pas mais il est bien avec toi",
                    [ConvertNonZombieTranslation] = "C'est pas un zombie ça",
                    [ConvertSuccessTranslation] = "Ami obtenu !",
                }
            };
        }

        public override float Cooldown { get;  } = 10*60f;

        public static float MaxDistance { get; set; } = 15f;

        public TextImage IconName => MainPlugin.Instance.icons["Convert"];





        protected override void AbilityAdded(Player player)
        {
            if (!player.GameObject.TryGetComponent<ConvertComponent>(out var comp))
            {
                comp = player.GameObject.AddComponent<ConvertComponent>();
                comp.Init(player, this,false);
            }
            base.AbilityAdded(player);
        }

        protected override void AbilityRemoved(Player player)
        {
            player.GameObject.GetComponent<ConvertComponent>().Destroy();
            base.AbilityRemoved(player);
        }


        protected override bool AbilityUsed(Player player)
        {
            Vector3 start = player.CameraTransform.position+ player.CameraTransform.forward*.2f;
            Vector3 end = start + player.CameraTransform.forward * MaxDistance;
          

            if (!Raycast(player,out var hit)) return false;


            Player playerHit = Player.Get(hit.collider);

            if (playerHit == null || playerHit == player)
            {

                
                ShowEffectHint(player, ConvertNobodyTranslation);
                return false;
            }


            if (playerHit.Role.Side == player.Role.Side)
            {
                ShowEffectHint(player, ConvertSameTeamTranslation);
                return false;
            }

            if (playerHit.IsScp && playerHit.Role != RoleTypeId.Scp0492)
            {
                ShowEffectHint(player, ConvertNonZombieTranslation);
                return false;
            }


            if (playerHit.IsScp)
            {
                playerHit.Role.Set(player.Role, RoleSpawnFlags.AssignInventory);
            }
            else
            {
                playerHit.Role.Set(player.Role, RoleSpawnFlags.None);
            }

            ShowEffectHint(player, ConvertSuccessTranslation);
            return base.AbilityUsed(player);
        }


        public static bool Raycast(Player player,out RaycastHit hit)
        {
            Vector3 start = player.CameraTransform.position + player.CameraTransform.forward * .5f;
            Vector3 end = player.CameraTransform.position + player.CameraTransform.forward * MaxDistance;

            bool result = Physics.Raycast(player.CameraTransform.position + player.CameraTransform.forward * .5f, player.CameraTransform.forward, out hit, MaxDistance, (int)(LayerMasks.All));


            //if (MainPlugin.Instance.Config.Debug)
            //{
            //    DrawableLines.IsDebugModeEnabled = true;
            //    DrawableLines.GenerateLine(start, end);
            //}



            return result;
        }



    }


    public class ConvertComponent : MonoBehaviour
    {
        public Player Player { get; private set; }

        private Dictionary<Collider,ClientSidePrimitive> primitives = null;
        private ClientSidePrimitive primitive = null;
        private bool debug;

        private bool _paused = false;
        public bool Paused
        {
            get => _paused;
            set
            {
                Log.Info("old=" + _paused);
                Log.Info("new=" + value);

                if (value && !_paused)
                {
                    DestroyPrimitive();
                }
                _paused = value;
            }
        }

        private Convert _ability;

        public void Init(Player player,Convert ability, bool debug = false)
        {
            Player = player;
            this.debug = debug;
            _ability = ability;
            primitives = new Dictionary<Collider, ClientSidePrimitive>();
            lastpositions = new Dictionary<ClientSidePrimitive, Vector3>();
            _paused = false;
        }



        private Color32 ColorValid = new Color32(0, 0, 200, 150);
        private Color32 ColorInvalid = new Color32(200, 0, 0, 150);

        private Dictionary<ClientSidePrimitive, Vector3> lastpositions;
        private ClientSidePrimitive MoveOrCreatePrimitive(Collider collider,bool valid)
        {
            Color color;

            if (valid)
            {
                color = ColorValid;
            }
            else
            {
                color = ColorInvalid;

            }



            ClientSidePrimitive primitive = null;
            if (!primitives.ContainsKey(collider))
            {
                Bounds bounds = collider.bounds;
                Vector3 scale = bounds.size;
                Vector3 center = bounds.center;

                primitive = new ClientSidePrimitive(center, Quaternion.identity, scale, PrimitiveType.Cube, color, AdminToys.PrimitiveFlags.Visible, Player, false);
                primitive.SpawnClientPrimitive();
                lastpositions.Add(primitive, primitive.Position);
                primitives.Add(collider, primitive);
            }
            else
            {
                primitive = primitives[collider];

                if(!lastpositions.TryGetValue(primitive,out Vector3 oldPosition))
                {
                    if (!oldPosition.Equals(collider.transform.position))
                    {
                        primitive.Position = oldPosition;
                        primitive.Resync();
                    }
                }
            }


            return primitive;
        }

        public void DestroyPrimitive()
        {

            foreach(ClientSidePrimitive primitive in primitives.Values)
            {
                primitive.DestroyClientPrimitive();
            }

            primitives?.Clear();
            lastpositions?.Clear();

            primitive?.DestroyClientPrimitive();
            primitive = null;
        }

        public void Destroy()
        {
            DestroyPrimitive();
            Destroy(this);
        }

        private float counter = 1f;
        private const float TickTime = .1f;


        private void CreatePrimitive(Exiled.API.Features.Player player,bool valid)
        {
            if (player.Role is FpcRole fpc)
            {

                foreach (HitboxIdentity hitboxIdentity in fpc.Model.Hitboxes)
                {
                    foreach (Collider collider in hitboxIdentity.TargetColliders)
                    {
                        MoveOrCreatePrimitive(collider, valid);
                    }
                }

            }
        }

        private Player firstPlayerHit = null;

        internal void Update()
        {
            if (!_ability.CanUse(Player, out _))
            {
                DestroyPrimitive();
                return;
            }
            counter -= Time.deltaTime;
            if (counter > 0)
            {
                return;
            }

            if (Paused)
            {
                return;
            }

            

            counter = TickTime;
            try
            {
                bool result = Convert.Raycast(Player, out var hit);

                if (result && Exiled.API.Features.Player.TryGet(hit.collider, out Exiled.API.Features.Player playerHit))
                {

                    if(firstPlayerHit != null && playerHit != firstPlayerHit)
                    {
                        return;
                    }

                    firstPlayerHit = playerHit;

                    if (SCPTeam.IsSCP(playerHit.ReferenceHub) && playerHit.Role.Type != RoleTypeId.Scp0492)
                    {
                        Log.Info("scp non zombie");
                        CreatePrimitive(playerHit, false);
                        return;
                    }


                    if(playerHit.LeadingTeam == Player.LeadingTeam)
                    {
                        Log.Info("same team");
                        CreatePrimitive(playerHit, false);
                        return;
                    }

                    CreatePrimitive(playerHit,true);

                }
                else
                {
                    DestroyPrimitive();
                    firstPlayerHit = null;
                }

            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

    }

}

