using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleTest : MonoBehaviour
{
    Rigidbody2D rb;
    CapsuleCollider2D capCollider;
    private float groundAngle = 0f;
    private float maxAngle = 3f * Mathf.PI / 8f;
    private bool grounded = false;

    private ContactPoint2D[] contactPoints;
    private List<ContactPoint2D> validContactPoints;
    private int maxContacts = 4;
    private Vector2 groundedOffset;

    private float capWidth;
    private float capHeight;
    private float capSegment;

    private LayerMask selfMask;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capCollider = GetComponent<CapsuleCollider2D>();
        
        Bounds bounds = capCollider.bounds;
        capWidth = bounds.max.x - bounds.min.x;
        capHeight = bounds.max.y - bounds.min.y;
        
        //segment from center point to where radius begins
        capSegment = 0.5f * (capHeight - capWidth);

        groundedOffset = new Vector2(0f, -capHeight * 0.5f);

        contactPoints = new ContactPoint2D[maxContacts];
        validContactPoints = new List<ContactPoint2D>();

        selfMask = ~LayerMask.GetMask("playerLayer");
    }

    // Update is called once per frame
    void Update()
    {
        //so only changes grounded for 1st frame in the air
        groundCheck();
        print(grounded);

        if(grounded)
        {
            Vector2 resultRun = new Vector2(Input.GetAxisRaw("Horizontal") * Mathf.Cos(groundAngle), Input.GetAxisRaw("Horizontal") * Mathf.Sin(groundAngle));
            rb.AddForce(resultRun * 5f, ForceMode2D.Force);
            rb.gravityScale = 0f;
        }
        else
        {
            Vector2 resultRun = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
            rb.AddForce(resultRun * 5f, ForceMode2D.Force);
            rb.gravityScale = 1.5f;
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        validContactPoints.Clear();

        int contactCount = collision.contactCount;

        if (contactCount <= maxContacts)
        {
            //ASSUMING it clears the array OR overwrites from beginning ... this should work
            collision.GetContacts(contactPoints);
            
            //Get all contact points at the bottom part of the capsule
            for (int i=0;i<contactCount;i++)
            {
                if (contactPoints[i].point.y < capCollider.transform.position.y - capSegment)
                {
                    float pointAngle = Vector2.SignedAngle(Vector2.up, contactPoints[i].normal) * Mathf.Deg2Rad;

                    //Get all of those points with angles less than max angle
                    if (Mathf.Abs(pointAngle) < maxAngle)
                    {
                        validContactPoints.Add(contactPoints[i]);
                    }
                }
            }

            
            if (validContactPoints.Count > 0)
            {
                //Get the contact point furthest towards the side of direction
                ContactPoint2D furthestPoint = validContactPoints[0];
                
                for (int j=0;j<validContactPoints.Count;j++)
                {
                    if(Mathf.Abs(validContactPoints[j].point.x) > Mathf.Abs(furthestPoint.point.x))
                    {
                        furthestPoint = validContactPoints[j];
                    }
                }

                groundAngle = Vector2.SignedAngle(Vector2.up, furthestPoint.normal) * Mathf.Deg2Rad;
                groundedOffset = transform.InverseTransformPoint(furthestPoint.point);
            }
            else
            {
                //No valid ground points
            }            
        }
    }

    private void groundCheck()
    {
        RaycastHit2D ray = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y + 0.01f) + groundedOffset , new Vector2(Mathf.Sin(groundAngle),-Mathf.Cos(groundAngle)), 0.02f,selfMask);
        Debug.DrawRay((Vector2)transform.position + groundedOffset, new Vector2(Mathf.Sin(groundAngle), -Mathf.Cos(groundAngle)), Color.red);
        grounded = ray;
    }
}
