
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    [SerializeField] private Color newColor = Color.red;

    [SerializeField] private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
    }

    void OnMouseDown(){
        rend.material.color = Random.ColorHSV();
    }
}
