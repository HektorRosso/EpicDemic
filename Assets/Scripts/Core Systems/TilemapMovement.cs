using UnityEngine;
using System.Collections;

public class TilemapMovement : MonoBehaviour
{
    [SerializeField] private float movementTime = 1f;
    [SerializeField] private float tileSpacing = 1.25f;

    private bool isMoving;

    void Update()
    {
        if (isMoving)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider == null)
            {
                Debug.Log("Nothing clicked.");
                return;
            }

            Debug.Log("Clicked: " + hit.collider.name);

            Vector3 clickedPos = hit.collider.transform.position;
            Vector3 difference = clickedPos - transform.position;

            Vector3 direction = Vector3.zero;
            const float tolerance = 0.1f;

            bool adjacentHorizontally =
                Mathf.Abs(Mathf.Abs(difference.x) - tileSpacing) < tolerance &&
                Mathf.Abs(difference.y) < tolerance;

            bool adjacentVertically =
                Mathf.Abs(Mathf.Abs(difference.y) - tileSpacing) < tolerance &&
                Mathf.Abs(difference.x) < tolerance;

            if (adjacentHorizontally)
            {
                direction = difference.x > 0 ? Vector3.right : Vector3.left;
            }
            else if (adjacentVertically)
            {
                direction = difference.y > 0 ? Vector3.up : Vector3.down;
            }

            if (direction != Vector3.zero)
            {
                StartCoroutine(MovePlayer(direction * tileSpacing));
            }
        }
    }

    private IEnumerator MovePlayer(Vector3 movement)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + movement;

        float elapsed = 0f;

        while (elapsed < movementTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / movementTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        isMoving = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        // Draw the four adjacent tile positions
        Gizmos.DrawWireCube(transform.position + Vector3.up * tileSpacing, Vector3.one * 0.9f);
        Gizmos.DrawWireCube(transform.position + Vector3.down * tileSpacing, Vector3.one * 0.9f);
        Gizmos.DrawWireCube(transform.position + Vector3.left * tileSpacing, Vector3.one * 0.9f);
        Gizmos.DrawWireCube(transform.position + Vector3.right * tileSpacing, Vector3.one * 0.9f);

        // Draw the player's current tile
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.9f);
    }
}