using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    SpriteRenderer renderz;

    // Use this for initialization
    void Start()
    {
        renderz = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void checkDead()
    {
        if(health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void flash(Color color)
    {
        StartCoroutine(flashEnum(color));
    }

    IEnumerator flashEnum(Color color)
    {
        renderz.color = color;
        yield return new WaitForSeconds(0.1f);
        renderz.color = Color.white;
    }
}
