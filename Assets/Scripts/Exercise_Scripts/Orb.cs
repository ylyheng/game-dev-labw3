
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
        float bob = Mathf.Sin(Time.time * 3f) * bobHeight;

        transform.position = basePosition + Vector3.up * bob;

        // Rotate the orb.
        transform.Rotate(
            Vector3.up,
            spinSpeed * Time.deltaTime,
            Space.World
        );
    }

    private void OnMouseDown()
    {
        if (clickColors.Length == 0)
            return;

        // Move to the next color.
        clickIndex = (clickIndex + 1) % clickColors.Length;

        // Change orb color.
        if (orbRenderer != null)
        {
            orbRenderer.material.color = clickColors[clickIndex];
        }

        Debug.Log("[Orb] Clicked and color changed!");
    }
}
