using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SpaceObjectsInfo
{
    [CreateAssetMenu(fileName = "SpaceObjectVariants_", menuName = "ScriptableObjects/SpaceObjectVariants")]
    public class SpaceObjectVariants : ScriptableObject
    {
        [SerializeField] protected List<GameObject> variants;

        protected List<GameObject> Variants => variants;

        public GameObject GetRandomVariant()
        {
            if (variants.Count == 0)
            {
                return null;
            }

            int randomIndex = Random.Range(0, variants.Count);
            return variants[randomIndex];
        }
    }
}
