using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Block"))
        {
            GameManager.Instance.Lose();
        }
    }
}
