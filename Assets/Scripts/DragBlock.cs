using UnityEngine;

public class DragBlock : MonoBehaviour
{
    private Vector3 offset;
    private bool dragging = false;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnMouseDown()
    {
        if (GameManager.Instance.gameOver) return;

        dragging = true;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        offset = transform.position - GetMouseWorld();
    }

    void OnMouseUp()
    {
        dragging = false;
        rb.gravityScale = 1;
    }

    void Update()
    {
        if (GameManager.Instance.gameOver) return;

        if (dragging)
        {
            Vector3 pos = GetMouseWorld() + offset;
            pos.z = transform.position.z;
            transform.position = pos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || collision.gameObject == null)
            return;

        if (!dragging && rb.velocity.y < 0.01f)
        {
            if (collision.gameObject.CompareTag("Floor"))
            {
                float targetX = collision.transform.position.x;
                transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
            }
            else if (collision.gameObject.CompareTag("Block"))
            {
                if (transform.position.y > collision.transform.position.y)
                {
                    float targetX = collision.transform.position.x;
                    transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
                }
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