using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyPW_UP : MonoBehaviour
{
    BubbleManager bubbleManager;
    public PWUpsManager pwUpsManager;
    public BubblePlayerController spriteChanger;

    /*
     * Solo puede haber un powerUp a la vez en escena, se deben generar en puntos de spawn
     * ya predefinidos, tras agarrar un powerUp deben pasar 30 segundos para que spawnee
     * nuevamente otro de los powerUps.
     */

    private void Start()
    {
        bubbleManager = FindObjectOfType<BubbleManager>();
        pwUpsManager = FindObjectOfType<PWUpsManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bubbleManager = collision.collider.GetComponent<BubbleManager>();
        bubbleManager.ActivateButterflyPowerUp();

        pwUpsManager.NotifyPowerUpCollected();

        spriteChanger = collision.collider.GetComponent<BubblePlayerController>();
        spriteChanger.ChangeSpriteButter();

        Destroy(gameObject);
    }

}
