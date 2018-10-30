using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class katy : MonoBehaviour
{
    int weaponID = 0;
    float bulletForce = 5f;
    public GameObject bullet;

    private float health = 100f;

    private float shootCooldown = 0.5f;
    private float shootTimeCount = 0f;
    private float maxSlopeAngle = 3f * Mathf.PI / 8f;

    private float jumpSquatTime = 5f / 60f;
    private float accumulatedJumpTime = 0f;

    //private bool airdashAvailable = true;
    //private float airdashVelocity = 5f;
    private float frictionScale = 0.5f;
    //private Vector2 airDashOffset = Vector2.up;
    private float jumpVeclocity = 10f;
    public float runForce = 10f;
    private bool grounded = false;
    private bool sliding = false;
    private float groundAngle = 0f;
    private bool water = false;
    private BoxCollider2D box;
    private Rigidbody2D rb;
    private int layermask;

    private void Start()
    {
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        //tilda flips the bits! (so ignores player)
        layermask = ~LayerMask.GetMask("playerLayer");
    }

    private void groundUpdate(float dt)
    {

    }

    private void waterUpdate(float dt)
    {

    }

    private void airUpdate(float dt)
    {

    }

    void Update()
    {
        float dt = Time.deltaTime;

        sliding = Input.GetAxisRaw("Vertical") == -1f;
       
        Bounds bounds = box.bounds;
        box.bounds.Expand(-2f * 0.01f);

        Vector2 bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        Vector2 bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        
        //left foot
        RaycastHit2D blDown = Physics2D.Raycast(bottomLeft, Vector2.down, 0.03f, layermask);
        Debug.DrawRay(bottomLeft,Vector2.down * 0.03f, Color.red);

        RaycastHit2D blLeft = Physics2D.Raycast(bottomLeft, Vector2.left, 0.03f, layermask);
        Debug.DrawRay(bottomLeft, Vector2.left * 0.03f, Color.red);

        //right foot
        RaycastHit2D brDown = Physics2D.Raycast(bottomRight, Vector2.down, 0.03f, layermask);
        Debug.DrawRay(bottomRight, Vector2.down * 0.03f, Color.red);

        RaycastHit2D brRight = Physics2D.Raycast(bottomRight, Vector2.right, 0.03f, layermask);
        Debug.DrawRay(bottomRight, Vector2.right * 0.03f, Color.red);

        //on point laser
        RaycastHit2D onPoint = Physics2D.Raycast(bottomRight + Vector2.down * 0.02f, Vector2.left, bounds.max.x - bounds.min.x, layermask);
        Debug.DrawRay(bottomRight + Vector2.down * 0.02f, Vector2.left * (bounds.max.x - bounds.min.x), Color.blue);

        grounded = false;
        
        if (blDown)
        {
            groundAngle = Vector2.SignedAngle(Vector2.up, blDown.normal) * Mathf.Deg2Rad;
            grounded = true;
            //airdashAvailable = true;
        }
        else if(brDown)
        {
            groundAngle = Vector2.SignedAngle(Vector2.up, brDown.normal) * Mathf.Deg2Rad;
            //airdashAvailable = true;
            grounded = true;
        }
        else if(onPoint)
        {
            groundAngle = 0f;
            //airdashAvailable = true;
            grounded = true;
        }

        if (rb.velocity.x < 0)
        {
            if (blLeft)
            {
                float testAngle = Vector2.SignedAngle(Vector2.up, blLeft.normal) * Mathf.Deg2Rad;
                if(Mathf.Abs(testAngle) < maxSlopeAngle)groundAngle = testAngle;
            }
        }
        else
        {
            if (brRight)
            {
                float testAngle = Vector2.SignedAngle(Vector2.up, brRight.normal) * Mathf.Deg2Rad;
                if (Mathf.Abs(testAngle) < maxSlopeAngle) groundAngle = testAngle;
            }
        }

        if(!sliding)
        {
            //shoot bullet while not sliding
            if(Input.GetMouseButtonDown(0))
            {
                GameObject bulletz = Instantiate(bullet,new Vector2(transform.position.x,transform.position.y), Quaternion.identity);
                Physics2D.IgnoreCollision(box, bulletz.GetComponent<CircleCollider2D>());

                Vector2 bulletDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
                float bulletAngle = Vector2.SignedAngle(Vector2.right, bulletDirection);
                bulletz.GetComponent<Rigidbody2D>().AddForce(bulletDirection.normalized * bulletForce, ForceMode2D.Impulse);
                bulletz.transform.Rotate(0f, 0f, bulletAngle);
            }
        }

        //if sliding and grounded make the gravityscale 3f or something big
        if(grounded)
        {
            if(Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x,jumpVeclocity);
            }
            else if(sliding)
            {
                rb.gravityScale = 1f;
            }
            else
            {
                Vector2 resultRun = new Vector2(Input.GetAxisRaw("Horizontal") * Mathf.Cos(groundAngle), Input.GetAxisRaw("Horizontal") * Mathf.Sin(groundAngle));
                rb.AddForce(resultRun * runForce, ForceMode2D.Force);
                rb.gravityScale = 0f;
                
                //friction
                //Vector2 friction = rb.velocity * -1f * frictionScale;
                //rb.AddForce(friction, ForceMode2D.Force);
            }

        }
        else
        {
            /*
            if(Input.GetButtonDown("DashShield") && airdashAvailable)
            {
                Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
                Vector2 dash = inputDirection * airdashVelocity;

                rb.velocity = dash;
                airdashAvailable = false;
            }
            */
            Vector2 resultDrift = new Vector2(Input.GetAxisRaw("Horizontal"),0f);
            rb.AddForce(resultDrift * runForce, ForceMode2D.Force);
            rb.gravityScale = 1f;
        }



        if (grounded) groundUpdate(dt);
        else if (water) waterUpdate(dt);
        else airUpdate(dt);
    }
}
