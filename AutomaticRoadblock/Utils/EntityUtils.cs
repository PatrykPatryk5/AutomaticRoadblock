using System.Collections.Generic;
using System.Linq;
using Rage;
using Rage.Native;

namespace AutomaticRoadblocks.Utils
{
    public static class EntityUtils
    {
        /// <summary>
        /// Attach the given attachment to the given entity.
        /// </summary>
        /// <param name="attachment">Set the attachment entity.</param>
        /// <param name="target">Set the target entity.</param>
        /// <param name="placement">Set the place to attach the attachment to at the target.</param>
        public static void AttachEntity(Entity attachment, Entity target, PedBoneId placement)
        {
            Assert.NotNull(attachment, "attachment cannot be null");
            Assert.NotNull(target, "target cannot be null");
            var boneId = NativeFunction.Natives.GET_PED_BONE_INDEX<int>(target, (int)placement);

            NativeFunction.Natives.ATTACH_ENTITY_TO_ENTITY(attachment, target, boneId, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, false, false, false, 2, 1);
        }

        /// <summary>
        /// Detach the given attachment from it's entity.
        /// </summary>
        /// <param name="attachment">Set the attachment.</param>
        public static void DetachEntity(Entity attachment)
        {
            Assert.NotNull(attachment, "attachment cannot be null");
            NativeFunction.Natives.DETACH_ENTITY(attachment, false, false);
        }

        /// <summary>
        /// Remove the given entity from the game world.
        /// </summary>
        /// <param name="entity">Set the entity to remove.</param>
        public static void Remove(Entity entity)
        {
            Assert.NotNull(entity, "entity cannot be null");
            if (!entity.IsValid())
                return;

            entity.IsPersistent = false;
            entity.Dismiss();
            entity.Delete();
        }

        /// <summary>
        /// Clean the given area from entities (e.g. vehicles, objects, etc.)
        /// </summary>
        /// <param name="position">Set the position to clean.</param>
        /// <param name="radius">Set the area around the position to clean.</param>
        /// <param name="excludeEmergencyVehicles">Set if emergency vehicles should be excluded from the cleanup.</param>
        public static void CleanArea(Vector3 position, float radius, bool excludeEmergencyVehicles = false)
        {
            Assert.NotNull(position, "position cannot be null");
            var queryFlags = GetEntitiesFlags.ConsiderAllVehicles | GetEntitiesFlags.ExcludePlayerPed | GetEntitiesFlags.ExcludePlayerVehicle;

            if (excludeEmergencyVehicles)
                queryFlags |= GetEntitiesFlags.ExcludeAmbulances | GetEntitiesFlags.ExcludeFiretrucks | GetEntitiesFlags.ExcludePoliceCars;

            var entitiesToClean = World
                .GetEntities(position, radius, queryFlags)
                .Where(x => x.IsValid())
                .Where(x => x != Game.LocalPlayer.LastVehicle);

            foreach (var entity in entitiesToClean)
            {
                Remove(entity);
            }
        }

        public static Ped CreateFbiCop(Vector3 position)
        {
            Assert.NotNull(position, "position cannot be null");
            var ped = new Ped(ModelUtils.GetPoliceFbiCop(), position, 3f)
            {
                IsPersistent = true,
                BlockPermanentEvents = true,
                KeepTasks = true
            };

            AddWeaponsToCopPed(ped, new []
            {
                ModelUtils.Weapons.HeavyRifle,
            });
            return ped;
        }

        public static Ped CreateSwatCop(Vector3 position)
        {
            Assert.NotNull(position, "position cannot be null");
            var ped = new Ped(ModelUtils.GetLocalCop(position), position, 3f)
            {
                IsPersistent = true,
                BlockPermanentEvents = true,
                KeepTasks = true
            };

            AddWeaponsToCopPed(ped, new []
            {
                ModelUtils.Weapons.HeavyRifle,
                ModelUtils.Weapons.Shotgun,
            });
            return ped;
        }

        /// <summary>
        /// Get the vector which is on the ground for the given coordinates.
        /// </summary>
        /// <param name="position">The position to get the ground level of.</param>
        /// <returns>Returns the new vector which is on the ground.</returns>
        public static Vector3 OnGround(Vector3 position)
        {
            var newPosition = new Vector3(position.X, position.Y, 0f);

            NativeFunction.Natives.GET_GROUND_Z_FOR_3D_COORD<bool>(position.X, position.Y, position.Z + 15f, out float newHeight, 0);
            newPosition.Z = newHeight;

            return newPosition;
        }

        private static void AddWeaponsToCopPed(Ped ped, IEnumerable<string> weapons)
        {
            foreach (var weapon in weapons)
            {
                var asset = new WeaponAsset(weapon);
                
                ped.Inventory.GiveNewWeapon(asset, -1, false);
                ped.Inventory.AddComponentToWeapon(asset, ModelUtils.Weapons.Attachments.PistolFlashlight);
                ped.Inventory.AddComponentToWeapon(asset, ModelUtils.Weapons.Attachments.RifleFlashlight);
            }

            ped.Inventory.GiveFlashlight();
            NativeFunction.Natives.SET_PED_CAN_SWITCH_WEAPON(ped, true);
        }
    }
}