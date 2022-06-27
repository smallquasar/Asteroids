using Assets.Scripts.Weapon;
using System;
using UnityEngine;

namespace Assets.Scripts.Events.SpaceEventArgs
{
    public class WeaponShotEventArgs : EventArgs
    {
        public readonly Vector3 Direction;
        public readonly WeaponType WeaponType;

        public WeaponShotEventArgs(Vector3 direction, WeaponType weaponType)
        {
            Direction = direction;
            WeaponType = weaponType;
        }
    }
}
