using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool alreadyPlayed;

        private void OnTriggerEnter(Collider other)
        {
            if (!alreadyPlayed && other.CompareTag("Player"))
            {
                GetComponent<PlayableDirector>().Play();
                alreadyPlayed = true;
            }
        }
    }
}

