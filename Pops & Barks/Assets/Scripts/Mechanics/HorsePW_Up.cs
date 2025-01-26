using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorsePW_Up : MonoBehaviour
{
    BubbleManager bubbleManager;
    private PWUpsManager pwUpsManager;

    private void Start()
    {
        bubbleManager = FindObjectOfType<BubbleManager>();
        pwUpsManager = FindObjectOfType<PWUpsManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bubbleManager.ActivateElephantPowerUp();
        pwUpsManager.NotifyPowerUpCollected();
        Destroy(gameObject);
    }
}
