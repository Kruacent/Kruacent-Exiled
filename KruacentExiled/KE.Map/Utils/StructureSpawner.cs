
using Exiled.API.Features;
using Interactables;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Firearms.Attachments;
using LabApi.Features.Wrappers;
using MapGeneration;
using MapGeneration.Distributors;
using MEC;
using Mirror;
using ProjectMER.Commands.Modifying.Position;
using ProjectMER.Commands.Modifying.Rotation;
using ProjectMER.Commands.Modifying.Scale;
using ProjectMER.Features;
using ProjectMER.Features.Enums;
using ProjectMER.Features.Serializable.Lockers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static PlayerList;
using static PlayerRoles.FirstPersonControl.Thirdperson.Subcontrollers.FeetStabilizerSubcontroller;

namespace KE.Map.Utils
{
    public static class StructureSpawner
    {
        public static Dictionary<FacilityZone, HashSet<DoorVariant>> AdditionalDoors { get; } = new();



        public static DoorVariant GetDoorPrefab(DoorType doortype)
        {
            return doortype switch
                {
                    DoorType.Lcz => PrefabManager.DoorLcz,
                    DoorType.Hcz => PrefabManager.DoorHcz,
                    DoorType.Ez => PrefabManager.DoorEz,
                    DoorType.Bulkdoor => PrefabManager.DoorHeavyBulk,
                    DoorType.Gate => PrefabManager.DoorGate,
                    _ => throw new InvalidOperationException(),
                };
        }

        public static MapGeneration.Distributors.Locker LockerPrefab(LockerType lockertype)
        {
            return lockertype switch
            {
                LockerType.PedestalScp500 => PrefabManager.PedestalScp500,
                LockerType.LargeGun => PrefabManager.LockerLargeGun,
                LockerType.RifleRack => PrefabManager.LockerRifleRack,
                LockerType.Misc => PrefabManager.LockerMisc,
                LockerType.Medkit => PrefabManager.LockerRegularMedkit,
                LockerType.Adrenaline => PrefabManager.LockerAdrenalineMedkit,
                LockerType.PedestalScp018 => PrefabManager.PedestalScp018,
                LockerType.PedestalScp207 => PrefabManager.PedstalScp207,
                LockerType.PedestalScp244 => PrefabManager.PedestalScp244,
                LockerType.PedestalScp268 => PrefabManager.PedestalScp268,
                LockerType.PedestalScp1853 => PrefabManager.PedstalScp1853,
                LockerType.PedestalScp2176 => PrefabManager.PedestalScp2176,
                LockerType.PedestalScpScp1576 => PrefabManager.PedestalScp1576,
                LockerType.PedestalAntiScp207 => PrefabManager.PedestalAntiScp207,
                LockerType.PedestalScp1344 => PrefabManager.PedestalScp1344,
                LockerType.ExperimentalWeapon => PrefabManager.LockerExperimentalWeapon,
                _ => throw new InvalidOperationException(),
            };
        }


        public static DoorVariant SpawnDoor(DoorType doortype,Vector3 position,Quaternion rotation, Vector3 scale)
        {
            Vector3 absolutePosition = position;
            Quaternion absoluteRotation = rotation;
            DoorVariant doorVariant = UnityEngine.Object.Instantiate(GetDoorPrefab(doortype));
            if (doorVariant.TryGetComponent<DoorRandomInitialStateExtension>(out var component))
            {
                UnityEngine.Object.Destroy(component);
            }

            doorVariant.transform.SetPositionAndRotation(absolutePosition, absoluteRotation);
            doorVariant.transform.localScale = scale;
            doorVariant.NetworkTargetState = false;
            doorVariant.ServerChangeLock(DoorLockReason.SpecialDoorFeature, false);
            doorVariant.RequiredPermissions = new DoorPermissionsPolicy(DoorPermissionFlags.None, false);

            NetworkServer.UnSpawn(doorVariant.gameObject);
            NetworkServer.Spawn(doorVariant.gameObject);
            FacilityZone zone = doorVariant.transform.position.GetZone();

            if (!AdditionalDoors.ContainsKey(zone))
            {
                AdditionalDoors.Add(zone, new());
            }
            AdditionalDoors[zone].Add(doorVariant);


            
            return doorVariant;
        }
    
        public static MapGeneration.Distributors.Locker SpawnPedestal(ItemType item, Vector3 position, Quaternion rotation,Vector3? scale)
        {
            MapGeneration.Distributors.Locker locker = UnityEngine.Object.Instantiate(LockerPrefab(LockerType.PedestalScp500));
            Vector3 absolutePosition = position;
            Quaternion absoluteRotation = rotation;
            locker.transform.localScale = scale ?? Vector3.one;
            locker.transform.SetPositionAndRotation(absolutePosition, absoluteRotation);
            if (locker.TryGetComponent<StructurePositionSync>(out var component))
            {
                component.Network_position = locker.transform.position;
                component.Network_rotationY = (sbyte)Mathf.RoundToInt(locker.transform.rotation.eulerAngles.y / 5.625f);
            }

            LabApi.Features.Wrappers.Locker labApiLocker = LabApi.Features.Wrappers.Locker.Get(locker);

            labApiLocker.ClearLockerLoot();


            labApiLocker.ClearAllChambers();
            NetworkServer.UnSpawn(locker.gameObject);
            NetworkServer.Spawn(locker.gameObject);



            foreach (MapGeneration.Distributors.LockerChamber chamber in locker.Chambers)
            {
                chamber.SpawnItem(item, 1);
            }
            
            return locker;
        }

        public static WorkstationController SpawnWorkshop(Vector3 position, Quaternion rotation,Vector3 scale)
        {
            WorkstationController workstationController = UnityEngine.Object.Instantiate(PrefabManager.Workstation);
            Vector3 absolutePosition = position;
            Quaternion absoluteRotation = rotation;
            workstationController.transform.SetPositionAndRotation(absolutePosition, absoluteRotation);
            workstationController.transform.localScale = scale;
            workstationController.NetworkStatus = (byte)0u;
            if (workstationController.TryGetComponent<StructurePositionSync>(out var component))
            {
                component.Network_position = workstationController.transform.position;
                component.Network_rotationY = (sbyte)Mathf.RoundToInt(workstationController.transform.rotation.eulerAngles.y / 5.625f);
            }

            NetworkServer.UnSpawn(workstationController.gameObject);
            NetworkServer.Spawn(workstationController.gameObject);
            return workstationController;
        }
    }
}
