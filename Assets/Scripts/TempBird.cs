using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBird : MonoBehaviour
{
    [Header("Settings")]
    public float cursorThreshold = 1;
    public float maxAcceleration = 30;
    public float maxSpeed = 5;
    public float maxRotationSpeed = 60;

    [Header("Variance Settings")]
    public float accelerationVariance = 0.5f;
    public float speedVariance = 0.1f;
    public float rotationVariance = 10f;

    [Header("Info")]
    public bool drowned;
    public float actualMaxAcceleration;
    public float actualMaxSpeed;
    public float actualMaxRotationSpeed;
    public TempBirdManager birdManager;

    [Header("Boid")]

    public GameObject bird_parent;

    private List<Rigidbody> boids;
    private int num_boids;

    // Boid control
    public float boid_com = 0.1f;
    public float boid_follow = 0f;
    public float boid_spacing = 1f;

    // How closely boids follow each other
    public float boid_coherence = 0.5f;
    public float boid_vel = 0.2f;
    public float boid_n_dist = 1f;
    public float boid_n_theta = 60f;
    public float boid_speed = 0.5f;
    public float boid_sensitivity = 0.05f;

    public float boid_max_speed = 0.5f;

    public float boid_rest = 0f;

    Rigidbody rb;
    Animator animator;
    
    void OnEnable()
    {
        birdManager = GetComponentInParent<TempBirdManager>();

        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        actualMaxAcceleration = maxAcceleration + Random.Range(-1f, 1f) * accelerationVariance;
        actualMaxSpeed = maxSpeed + Random.Range(-1, 1f) * speedVariance;
        actualMaxRotationSpeed = maxRotationSpeed + Random.Range(-1, 1f) * rotationVariance;

        animator.enabled = false;

        Invoke(nameof(DelayAnimatorEnable), Random.Range(0f, 0.4f));
        
        // Update boids
        boids = new List<Rigidbody>(); // Initialize followers list
        num_boids = bird_parent.transform.childCount;
        for (int i = 0; i < num_boids; i++) {
            Rigidbody boid = bird_parent.transform.GetChild(i).gameObject.GetComponent<Rigidbody>();
            boids.Add(boid);
        }

        // Seed boid parameters with random noise
        boid_com += Random.Range(-0.01f, 0.01f);
        boid_follow += Random.Range(-0.05f, 0.05f);
        boid_spacing += Random.Range(-0.05f, 0.05f);
        boid_vel += Random.Range(-0.1f, 0.1f);
        boid_n_dist += Random.Range(-0.1f, 0.1f);
        boid_n_theta += Random.Range(-15f, 15f);
        boid_speed += Random.Range(-0.1f, 0.1f);
        boid_sensitivity += Random.Range(-0.07f, 0.07f);
        boid_max_speed += Random.Range(-0.5f, 0.5f);
    }

    void DelayAnimatorEnable()
    {
        animator.enabled = true;
    }
    
    void Update()
    {
        if (!birdManager)
        {
            return;
        }

        if (!birdManager.birdController)
        {
            return;
        }

        if (drowned)
        {
            var v = rb.velocity;
            v.y = 20;
            rb.velocity = v;

            float scale = transform.localScale.x;
            scale -= Time.deltaTime;
            transform.localScale = Vector3.one * Mathf.Clamp(scale, 0, Mathf.Infinity);

            return;
        }
    
        animator.SetFloat("Speed", rb.velocity.magnitude / maxSpeed);

        // Update boids
        boids = new List<Rigidbody>(); // Initialize followers list
        num_boids = bird_parent.transform.childCount;
        for (int i = 0; i < num_boids; i++) {
            Rigidbody boid = bird_parent.transform.GetChild(i).gameObject.GetComponent<Rigidbody>();
            boids.Add(boid);
        }

        MoveAsBoid();
        LookAtVelocity(); 
    }

    void MoveAsBoid() {
        // Implement random dropout
        if (Random.Range(0f, 1f) < boid_rest) {
            rb.velocity = new Vector3(0f, 0f, 0f);
        }

        Vector3 leader = birdManager.birdController.cursorWorldPosition;

        Rigidbody bird = rb;
        // 0. Local Neighborhood Approach
        List<Rigidbody> neighbors = new List<Rigidbody>();
        for (int i = 0; i < num_boids; i++) {
            Rigidbody boid = boids[i];
            if (boid != bird) {
                Vector3 distance = boid.position - bird.position;
                Debug.Log(distance.magnitude);
                // Check within distance
                if (distance.magnitude < boid_n_dist) {
                    // Optional check within viewing angle
                    // Debug.Log(bird.rotation.eulerAngles);
                    if (Vector3.Angle(rb.velocity, distance) < boid_n_theta) {
                        neighbors.Add(boid);
                    }
                    neighbors.Add(boid);
                }
            }
        }

        int num_neighbors = neighbors.Count;
        // if (num_neighbors == 0) {
        //     return;
        // }

        // 1. Boids fly towards center of mass of its closest neighbors
        Vector3 centerOfMassComponent = new Vector3(0f, 0f, 0f);
        Vector3 com = new Vector3(0f, 0f, 0f);
        // 2. Boids keep a distance away from its closest neighbors
        Vector3 antiCollisionComponent = new Vector3(0f, 0f, 0f);
        // 3. Boids try to match the velocity of nearby boids
        Vector3 velocityMatchComponent = new Vector3(0f, 0f, 0f);
        Vector3 vel = new Vector3(0f, 0f, 0f);
        // Loop across all "closest neighbors"
        for (int i = 0; i < num_neighbors; i++) {
            Rigidbody boid = neighbors[i];
            if (boid != bird) {
                // Do not operate on self!
                // 1. 
                com += boid.position;
                // 2.
                Vector3 dist = boid.position - bird.position;
                if (dist.magnitude < boid_spacing) {
                    antiCollisionComponent -= dist * boid_coherence;
                }
                // 3. 
                vel += boid.velocity;
            }
        }
        // 1. post-processing
        // Let leader influence COM disproportionally
        // com = (com + 4 * leader) / (num_boids + 4);
        com /= num_boids; // Plus one because of leader.
        centerOfMassComponent = boid_com * (com - bird.position);
        // 3. post-processing
        vel /= num_boids;
        velocityMatchComponent = boid_vel * (vel - bird.velocity);
        // 4. Boids want to follow the leader (bonus rule)
        Vector3 followComponent = boid_follow * (leader - bird.position);
        if ((leader - bird.position).magnitude < 0.1) {
            followComponent *= 0f; // Cancel
        }
        
        // 5. Combine vectors
        Vector3 velocity = bird.velocity + centerOfMassComponent +
            antiCollisionComponent + velocityMatchComponent + followComponent;
        // Scale velocity
        velocity *= boid_speed;
       
       
        Debug.Log("Components of " + bird.velocity +  "com: " + centerOfMassComponent +
            "spacing: " + antiCollisionComponent + "vel: " + velocityMatchComponent +
            "follow: " +  followComponent);

        Debug.Log(velocity.magnitude);
        // Move bird if vector exceeds sensitivity threshold
        if (velocity.magnitude > boid_sensitivity) {
            // Move bird
            // bird.position = bird.position + Vector3.ClampMagnitude(velocity, boid_max_speed);
            
            rb.velocity = Vector3.ClampMagnitude(velocity, boid_max_speed);
            // Rotate bird (MAY BE IMPLEMENTED!)
            // Vector3 direction = velocity;
            // // direction += 0.1f * (target - bird.position);
            // direction = new Vector3(direction.x, 0f, direction.z);
            // bird.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    void MoveToCursor()
    {
        Vector3 cursorPos = birdManager.birdController.cursorWorldPosition;

        bool tooCloseToCursor = Vector3.Distance(transform.position, cursorPos) < cursorThreshold;

        Vector3 targetV;

        if (tooCloseToCursor)
        {
            targetV = Vector3.zero;
        }
        else
        {
            targetV = Vector3.Normalize(cursorPos - transform.position) * actualMaxSpeed;
        }

        Vector3 v = rb.velocity;

        v.x = Mathf.MoveTowards(v.x, targetV.x, actualMaxAcceleration * Time.deltaTime);
        v.z = Mathf.MoveTowards(v.z, targetV.z, actualMaxAcceleration * Time.deltaTime);

        rb.velocity = v;
    }

    void LookAtVelocity()
    {
        Vector3 velDir = rb.velocity + transform.forward * 0.01f;
        velDir.y = 0;

        Quaternion lookRot = Quaternion.LookRotation(velDir, Vector3.up);
        

        rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookRot, Time.deltaTime * actualMaxRotationSpeed));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(Layer.Water))
        {
            drowned = true;
            Destroy(gameObject, 1);
        }
    }
}
