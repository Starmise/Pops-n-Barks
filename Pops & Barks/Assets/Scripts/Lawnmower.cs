using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lawnmower : MonoBehaviour
{

    public float leghtMov = 2.0f;
    public float speed = 2.0f;
    private Vector2 startPos;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Doggo")
        {
            Debug.Log("La podadora ha golpeado al perro");
        }
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        Vector2 vec = startPos;
        vec.x += leghtMov * Mathf.Sin(Time.time * speed);
        transform.position = vec;
    }
}
