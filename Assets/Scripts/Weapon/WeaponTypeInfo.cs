using UnityEngine;

namespace Assets.Scripts.Weapon
{
    [CreateAssetMenu(fileName = "WeaponType_", menuName = "ScriptableObjects/WeaponType")]
    public class WeaponTypeInfo : ScriptableObject
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private WeaponType weaponType;

        public GameObject ProjectilePrefab => projectilePrefab;
        public WeaponType WeaponType => weaponType;
    }
}
