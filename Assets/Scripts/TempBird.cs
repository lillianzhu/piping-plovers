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

        MoveToCursor();
        LookAtVelocity(); 
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
