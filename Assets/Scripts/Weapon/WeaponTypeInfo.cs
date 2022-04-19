using UnityEngine;

namespace Assets.Scripts.Weapon
{
    [CreateAssetMenu(fileName = "WeaponType_", menuName = "ScriptableObjects/WeaponType")]
    public class WeaponTypeInfo : ScriptableObject
    {
        [SerializeField] private Sprite projectileSprite;
        [SerializeField] private WeaponType weaponType;

        public Sprite ProjectileSprite => projectileSprite;
        public WeaponType WeaponType => weaponType;
    }
}
