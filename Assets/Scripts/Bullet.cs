using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage = 10f;
    public float explosionRadius = 0f;
    private bool rebound = false;
    private bool handled = false;
    public GameObject floatingText;
    private float lifetime = 5f;
        
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!handled)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (enemy)
            {
                print(enemy.health);

                if (collision.collider.tag == "critical")
                {
                    
                    float dealtDamage = Calcs.damage(damage, true, false, false, false);
                    enemy.health -= dealtDamage;
                    
                    TextMesh textcomp = floatingText.GetComponent<TextMesh>();
                    textcomp.text = dealtDamage.ToString();
                    textcomp.color = Color.red;
                    Instantiate(floatingText, transform.position, Quaternion.identity);
                    enemy.flash(Color.red);

                    //oof, ow, zing, ouch
                    handled = true;
                }
                else if (collision.collider.tag == "armoured")
                {
                    float dealtDamage = Calcs.damage(damage, false, true, false, false);
                    enemy.health -= dealtDamage;

                    TextMesh textcomp = floatingText.GetComponent<TextMesh>();
                    textcomp.text = dealtDamage.ToString();
                    textcomp.color = Color.yellow;
                    Instantiate(floatingText, transform.position, Quaternion.identity);
                    enemy.flash(Color.yellow);

                    //nope, meh, nah, 
                    handled = true;
                }

                enemy.checkDead();
                print(enemy.health);
            }

            if (!rebound)
            {
                Destroy(gameObject);
            }
        }
    }
}
