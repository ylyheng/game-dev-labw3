
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [Header("Click palette")]
    [SerializeField] private Color[] clickColors =
    {
        Color.cyan,
        Color.magenta,
        Color.yellow
    };

    [Header("Bob & spin")]
    [SerializeField] private float bobHeight = 0.15f;
    [SerializeField] private float spinSpeed = 90f;

    private Vector3 basePosition;
    private Vector3 baseScale;
    private int clickIndex;

        // ---- drag state ----
    private bool isDragging;
    private Plane dragPlane;         // horizontal plane the orb slides on
    private Vector3 dragOffset;      // keeps the grab point under the cursor

    private Renderer orbRenderer;

    void Awake()
    {
        basePosition = transform.position;
        baseScale = transform.localScale;

        // Get the Renderer component once.
        orbRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // Bob up and down.
        if (!isDragging)
        {
            float bob = Mathf.Sin(Time.time * 3f) * bobHeight;
            transform.position = basePosition + Vector3.up * bob;
        }

        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
    }
    
    private void OnMouseDown()
    {
        // Color cycle
        clickIndex = (clickIndex + 1) % clickColors.Length;
        GetComponent<Renderer>().material.color = clickColors[clickIndex];
        Debug.Log($"[Orb] Clicked → {clickColors[clickIndex]}");

        // Build a HORIZONTAL plane at the orb's current Y.
        // Normal = up, point = orb position → a flat floor plane.
        dragPlane = new Plane(Vector3.up, transform.position);

        // Remember the offset between the grab point and the orb center
        if (TryGetMouseOnPlane(out Vector3 hit))
            dragOffset = transform.position - hit;
        else
            dragOffset = Vector3.zero;

        isDragging = true;
        basePosition = transform.position;
    }

    void OnMouseDrag()
    {
        if (TryGetMouseOnPlane(out Vector3 hit))
            transform.position = hit + dragOffset;
    }

    void OnMouseUp()
    {
        isDragging = false;
        basePosition = transform.position;   // re-anchor the bob here
    }

    private bool TryGetMouseOnPlane(out Vector3 worldPos)
    {
        worldPos = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float enter))
        {
            worldPos = ray.GetPoint(enter);
            return true;
        }
        return false;
    }

}
