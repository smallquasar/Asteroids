using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "SpawnZone_", menuName = "ScriptableObjects/SpawnZone")]
    public class SpawnZones : ScriptableObject
    {
        [SerializeField] private float _minX;
        [SerializeField] private float _maxX;
        [SerializeField] private float _minY;
        [SerializeField] private float _maxY;

        public float MinX => _minX;
        public float MaxX => _maxX;
        public float MinY => _minY;
        public float MaxY => _maxY;
    }
}
