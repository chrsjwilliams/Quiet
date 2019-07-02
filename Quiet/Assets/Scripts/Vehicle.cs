using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    Vector2 location;
    Vector2 velocity;
    Vector2 acceleration;
    Vector2 target = new Vector2(0, 0);

    public Rigidbody2D rigidbody;

    float maxSpeed = 7f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // steering force = desired velocity - current velocity
    private void seek(Vector2 target)
    {
        Vector2 desiredVel = target - location;
        desiredVel.Normalize();
        desiredVel = desiredVel * maxSpeed;

        Vector2 steer = desiredVel - velocity;
        rigidbody.AddForce(steer);
    }
    

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
