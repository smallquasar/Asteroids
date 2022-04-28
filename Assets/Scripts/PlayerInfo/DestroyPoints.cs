using UnityEngine;

namespace Assets.Scripts.PlayerInfo
{
    [CreateAssetMenu(fileName = "DestroyPoints_", menuName = "ScriptableObjects/DestroyPoints")]
    public class DestroyPoints : ScriptableObject
    {
        [SerializeField] private Achievement achievement;
        [SerializeField] private int points;

        public Achievement Achievement => achievement;
        public int Points => points;
    }
}
