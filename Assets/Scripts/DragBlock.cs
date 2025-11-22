using UnityEngine;

public class DragBlock : MonoBehaviour
{
    private Vector3 offset;
    private bool dragging = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMouseDown()
    {
        dragging = true;
        rb.gravityScale = 0; 
        offset = transform.position - GetMouseWorld();
    }

    void OnMouseUp()
    {
        dragging = false;
        rb.gravityScale = 1;
    }

    void Update()
    {
        if (dragging)
        {
            Vector3 pos = GetMouseWorld() + offset;
            pos.z = transform.position.z;
            transform.position = pos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Transform snap = collision.transform.GetChild(0); 
            if (snap != null)
            {
                transform.position = new Vector3(snap.position.x, transform.position.y, transform.position.z);
                Debug.Log("Block snapped to X=" + snap.position.x);
            }
        }
    }

    Vector3 GetMouseWorld()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = 10; 
        return Camera.main.ScreenToWorldPoint(mouse);
    }
}
