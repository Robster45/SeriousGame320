using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cell : MonoBehaviour
{
    // moving vars
    public float maxSpeed;
    public float mass;
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 direction;
    public bool isStopped;

    // manager stuff
    public GameObject managerObj;
    public EntityManager emScript;
    public EconomyManager ecoScript;

    // Start is called before the first frame update
    void Start()
    {
        if (managerObj == null)
        {
            managerObj = GameObject.Find("Manager");
            emScript = managerObj.GetComponent<EntityManager>();
            ecoScript = managerObj.GetComponent<EconomyManager>();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        // checks if the cell is stopped
        if (!isStopped)
        {
            // updates the acceleration
            CalculateForces();

            // adds to other vecs
            velocity += acceleration * Time.deltaTime;
            position += velocity * Time.deltaTime;

            // updates gameobject vectors
            direction = velocity.normalized;
            transform.up = direction;
            acceleration = Vector3.zero;

            // updates position
            transform.position = position;
        }
    }

    /// <summary>
    /// Takes a target position and makes the cell
    /// start to head in that direction
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns>Force calculated to seek target position</returns>
    public Vector3 Seek(Vector3 targetPosition)
    {
        // Step 1: Find desired velocity
        Vector3 desiredVelocity = targetPosition - position;

        // Step 2: Scale vel to max speed
        desiredVelocity.Normalize();
        desiredVelocity = desiredVelocity * maxSpeed;

        // Step 3: Calculate seeking steering force
        Vector3 seekingVelocity = desiredVelocity - velocity;

        // Step 4: Return force
        return seekingVelocity;
    }

    /// <summary>
    /// Overloaded Seek for game objects
    /// </summary>
    /// <param name="target">GameObject of the target</param>
    /// <returns>Force calculated to seek the desired target</returns>
    public Vector3 Seek(GameObject target)
    {
        return Seek(target.transform.position);
    }

    /// <summary>
    /// Special seeks the targets future position
    /// </summary>
    /// <param name="target">A target cell to be pursued</param>
    /// <returns>A calculated force for the targets future position</returns>
    public Vector3 Pursue(GameObject target)
    {
        Cell script = target.GetComponent<Cell>();
        return Seek(script.position + (script.velocity * 2));
    }

    /// <summary>
    /// Flee method going to be used by the enemies on their
    /// way to the goal
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    public Vector3 Flee(Vector3 targetPosition)
    {
        // Step 1: Find DV (desired velocity)
        Vector3 desiredVelocity = position - targetPosition;

        // Step 2: Scale vel to max speed
        desiredVelocity.Normalize();
        desiredVelocity = desiredVelocity * maxSpeed;

        // Step 3:  Calculate seeking steering force
        Vector3 seekingVelocity = desiredVelocity - velocity;

        // Step 4: Return force
        return seekingVelocity;
    }

    public Vector3 Flee(GameObject obj)
    {
        return Flee(obj.transform.position);
    }

    /// <summary>
    /// Calculates the forces of where the cell should go
    /// </summary>
    public abstract void CalculateForces();
}
