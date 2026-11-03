using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// STARTER — Player controller for the "Orb Collector" lab goal.
/// Attach to a cube named "Player" (add a Rigidbody, freeze rotation X/Z).
///
/// ALREADY WORKS: WASD movement with GetKey + GetAxis, frame-rate independent
/// via Time.deltaTime. A Debug.Log heartbeat proves Update runs every frame.
///
/// YOUR LAB TASKS live in the TODO comments. Each TODO uses one function
/// from this week's lecture. Complete them all to finish the game.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Lab wiring (drag your objects here)")]
    [SerializeField] private GameObject orbPrefab;   // TODO 5: used to spawn a bonus orb
    [SerializeField] private Transform cameraTransform; // used by TODO 3 (camera follow)

    private Rigidbody rb;
    private float h;
    private float v;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Debug.Log("[PlayerController] Ready. Collect all orbs to win.");
    }

    void Update()
    {
        // Poll input in Update so key presses are never missed
        Debug.Log($"h = {h}   |   v = {v}");   // ← add this
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Horizontal movement direction (camera-relative if you want later)
        Vector3 moveDir = new Vector3(h, 0f, v).normalized;

        // Preserve current vertical velocity (gravity / falling / jumping)
        Vector3 vel = moveDir * moveSpeed;
        vel.y = rb.linearVelocity.y;

        // Hand the velocity to the physics engine — it resolves wall collisions for us
        rb.linearVelocity = vel;
    }


    // ---------------------------------------------------------------
    // TODO 1 (OnCollisionEnter): log the name of any wall you bump.
        void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit: " + collision.gameObject.name);
    }

    // TODO 2 (OnTriggerEnter): if other.CompareTag("Orb"), Destroy the orb
    //and Debug.Log the collection. (Orb prefab already tagged "Orb".)
        void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            GameManager.Instance.Collect();
            Destroy(other.gameObject);

            Debug.Log("Orb collected!");
        }
    }
    // TODO 3 (LateUpdate): move cameraTransform to
    //       player position + offset so it follows you smoothly.
        void LateUpdate()
    {
        if (cameraTransform != null)
        {
            Vector3 offset = new Vector3(0f, 18f, -12f);

            cameraTransform.position = Vector3.Lerp(
                cameraTransform.position,
                transform.position + offset,
                5f * Time.deltaTime
            );

            cameraTransform.LookAt(transform);
        }
    }
    // TODO 4 (OnMouseDown is on the Orb script, not here — see Orb.cs.)
    // TODO 5 (OnDestroy): when the Player is destroyed, Instantiate one
        //       bonus orbPrefab at your last position.
    // ---------------------------------------------------------------
        void OnDestroy()
    {
        if (orbPrefab != null)
        {
            Instantiate(
                orbPrefab,
                transform.position,
                Quaternion.identity
            );

            Debug.Log("Bonus orb spawned!");
        }
    }
}
