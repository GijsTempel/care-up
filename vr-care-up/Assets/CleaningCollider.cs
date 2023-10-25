using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningCollider : MonoBehaviour
{
    public GameObject marker;
    public GameObject particles;
    public AudioSource wipeAudio;
    public bool isCleaned = false;

    public TableCleaningAction cleaningMaster;

    void Start()
    {
        wipeAudio.pitch = Random.RandomRange(0.85f, 1.15f);
    }

    void TriggerCleanup()
    {
        if (isCleaned)
            return;
 
        isCleaned = true;
        // marker.SetActive(true);
        particles.SetActive(true);
        wipeAudio.gameObject.SetActive(true);
        cleaningMaster.CleanActionCount();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Cloth")
        {
            TriggerCleanup();
        }
    }
}
