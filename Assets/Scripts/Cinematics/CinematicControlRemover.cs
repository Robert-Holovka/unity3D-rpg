using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private GameObject player;
        private PlayerController playerController;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            playerController = player.GetComponent<PlayerController>();
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void DisableControl(PlayableDirector pd)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            playerController.enabled = false;
        }

        private void EnableControl(PlayableDirector pd)
        {
            playerController.enabled = true;
        }
    }
}

