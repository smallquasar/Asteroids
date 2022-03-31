using UnityEngine;

namespace Assets.Scripts
{
    public class AsteroidsGenerator
    {
        private GameObject _prefab;
        private Transform _prefabContainer;
        private int _initialCount;

        private AsteroidsPool _asteroidsPool;

        public AsteroidsGenerator(GameObject prefab, Transform prefabContainer, int initialCount)
        {
            _prefab = prefab;
            _prefabContainer = prefabContainer;
            _initialCount = initialCount;
        }

        public void Start()
        {
            _asteroidsPool = new AsteroidsPool(_prefab, _prefabContainer, _initialCount);

            for (int i = 0; i < _initialCount; i++)
            {
                var asteroid = _asteroidsPool.Get();
                asteroid?.Init();
                asteroid.SetActive(true);
            }
        }
    }
}
