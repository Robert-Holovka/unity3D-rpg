using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform playerPosition;

        private void LateUpdate()
        {
            transform.position = playerPosition.position;
        }
    }
}