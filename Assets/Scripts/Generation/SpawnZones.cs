using UnityEngine;

namespace Assets.Scripts.Generation
{
    [CreateAssetMenu(fileName = "SpawnZone_", menuName = "ScriptableObjects/SpawnZone")]
    public class SpawnZones : ScriptableObject
    {
        [SerializeField] private float minX;
        [SerializeField] private float maxX;
        [SerializeField] private float minY;
        [SerializeField] private float maxY;

        public float MinX => minX;
        public float MaxX => maxX;
        public float MinY => minY;
        public float MaxY => maxY;
    }
}
