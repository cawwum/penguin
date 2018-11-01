using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleTest : MonoBehaviour
{
    Rigidbody2D rb;
    CapsuleCollider2D capCollider;
    private float groundAngle = 0f;
    private float maxAngle = 3f * Mathf.PI / 8f;
    private ContactPoint2D[] contactPoints;
    private List<ContactPoint2D> validContactPoints;
    private int maxContacts = 4;
    private float capWidth;
    private float capHeight;
    private float capSegment;

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

        contactPoints = new ContactPoint2D[maxContacts];
        validContactPoints = new List<ContactPoint2D>();
    }

    // Update is called once per frame
    void Update()
    {

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
            }
            else
            {
                //No valid ground points
            }
                
            print(groundAngle);
            
        }
    }
}
