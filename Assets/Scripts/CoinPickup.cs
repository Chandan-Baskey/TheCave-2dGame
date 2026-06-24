using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPick;
    bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && ! wasCollected)
        {
            wasCollected = true;
            AudioSource.PlayClipAtPoint(coinPick,transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);

        }
    }
}
