 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;

 [RequireComponent(typeof(Collider2D))]
public class TeleportTrigger : MonoBehaviour
{
    public enum TriggerType { Enter, Exit };

    [Tooltip("The Transform to teleport to")]
    [SerializeField] Transform teleportTo = null;

    [Tooltip("The filter Tag")]
    [SerializeField] string filterTag = "Player";

    [Tooltip("Trigger Event to Teleport")]
    [SerializeField] TriggerType type = TriggerType.Enter; // random default value

    private AudioSource doorSound;
    private BoxCollider2D boxCollider;

    void Start()
    {
        doorSound = gameObject.GetComponent<AudioSource>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (type != TriggerType.Enter)
            return;

        if (filterTag == string.Empty || other.CompareTag(filterTag))
            other.transform.position = teleportTo.position;

            if (other.tag == "Player"){
            doorSound.Play();
            }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (type != TriggerType.Exit)
            return;

        if (filterTag == string.Empty || other.CompareTag(filterTag))
            other.transform.position = teleportTo.position;
    }
}
