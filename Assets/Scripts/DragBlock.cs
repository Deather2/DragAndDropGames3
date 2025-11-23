using UnityEngine;

public class DragBlock : MonoBehaviour
{
    private Vector3 offset;
    private bool dragging = false;
    private Rigidbody2D rb;
    public AudioClip clickSound;
    public AudioClip dropSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameManager.Instance.gameOver) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchWorld = GetTouchWorld(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                if (IsTouchingThis(touchWorld))
                {
                    StartDrag(touchWorld);
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (dragging)
                {
                    EndDrag();
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = GetMouseWorld();
            if (IsTouchingThis(mouseWorld))
            {
                StartDrag(mouseWorld);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (dragging)
            {
                EndDrag();
            }
        }

        if (dragging)
        {
            Vector3 pos = (Input.touchCount > 0 ? GetTouchWorld(Input.GetTouch(0).position) : GetMouseWorld()) + offset;
            pos.z = transform.position.z;
            transform.position = pos;
        }
    }

    bool IsTouchingThis(Vector3 worldPos)
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            return col.OverlapPoint(worldPos);
        }
        return false;
    }

    void StartDrag(Vector3 worldPos)
    {
        if (GameManager.Instance.gameOver) return;

        dragging = true;
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
        offset = transform.position - worldPos;

        Block block = GetComponent<Block>();
        if (block != null && block.currentTower != null)
        {
            block.currentTower.blocks.Remove(block);
            block.currentTower = null;
        }

        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }

    void EndDrag()
    {
        if (dropSound != null)
            audioSource.PlayOneShot(dropSound);

        dragging = false;
        rb.gravityScale = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null || collision.gameObject == null)
            return;

        if (!dragging && rb.linearVelocity.y < 0.01f)
        {
            if (collision.gameObject.CompareTag("Floor"))
            {
                float targetX = collision.transform.position.x;
                transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

                Tower tower = collision.transform.parent.GetComponent<Tower>();
                if (tower != null)
                {
                    AssignToTower(GetComponent<Block>(), tower);
                }
            }
            else if (collision.gameObject.CompareTag("Block"))
            {
                if (transform.position.y > collision.transform.position.y)
                {
                    float targetX = collision.transform.position.x;
                    transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

                    Block otherBlock = collision.gameObject.GetComponent<Block>();
                    if (otherBlock != null && otherBlock.currentTower != null)
                    {
                        AssignToTower(GetComponent<Block>(), otherBlock.currentTower);
                    }
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

    Vector3 GetTouchWorld(Vector2 touchPos)
    {
        Vector3 touch = touchPos;
        touch.z = 10;
        return Camera.main.ScreenToWorldPoint(touch);
    }

    public void AssignToTower(Block block, Tower tower)
    {
        if (block == null || tower == null) return;

        if (block.currentTower != null)
            block.currentTower.blocks.Remove(block);

        block.currentTower = tower;
        tower.blocks.Add(block);
        WinManager.Instance.TryWin();
    }
}