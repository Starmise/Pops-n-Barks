using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorsePW_Up : MonoBehaviour
{
    BubbleManager bubbleManager;
    private PWUpsManager pwUpsManager;
    public BubblePlayerController spriteChanger;

    private void Start()
    {
        bubbleManager = FindObjectOfType<BubbleManager>();
        pwUpsManager = FindObjectOfType<PWUpsManager>();
        spriteChanger = FindObjectOfType<BubblePlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bubbleManager.ActivateElephantPowerUp();
        pwUpsManager.NotifyPowerUpCollected();
        spriteChanger.ChangeSpriteEle();
        Destroy(gameObject);
    }
}
