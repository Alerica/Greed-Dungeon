using System.Collections;
using UnityEngine;

public class DraggableCard : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private Vector3 originalPosition;
    private bool returning = false;
    private float returnSpeed = 15f; // higher = faster snap

    [SerializeField] private LayerMask enemyLayer;
    private Transform targetEnemy = null;
    private bool snappingToEnemy = false;
    private float snapSpeed = 20f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (returning)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * returnSpeed);

            if (Vector3.Distance(transform.position, originalPosition) < 0.05f)
            {
                transform.position = originalPosition;
                returning = false;
            }
        }

        if (snappingToEnemy && targetEnemy != null)
        {
            transform.position = Vector3.Lerp(transform.position, targetEnemy.position, Time.deltaTime * snapSpeed);

            if (Vector3.Distance(transform.position, targetEnemy.position) < 0.1f)
            {
                snappingToEnemy = false;
                Debug.Log("Card effect resolved on enemy: " + targetEnemy.name);
               
                StartCoroutine(DissolveAndDestroy()); 
            }
        }
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPos();
        originalPosition = transform.position;
        isDragging = true;
        returning = false;
        snappingToEnemy = false;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        Collider2D hit = IsOverEnemy();
        if (hit != null)
        {
            targetEnemy = hit.transform;
            snappingToEnemy = true; // start snap animation
            Debug.Log("Card snapped to enemy: " + hit.name);
            gameObject.GetComponent<CardView>().PlayCard(hit.gameObject);
        }
        else
        {
            returning = true;
            Debug.Log("Card returned to hand.");
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = 10f;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private Collider2D IsOverEnemy()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return Physics2D.OverlapPoint(mousePos, enemyLayer);
    }
    
    private IEnumerator DissolveAndDestroy()
    {
        float duration = 0.5f; // half a second
        float time = 0f;

        Color startColor = spriteRenderer.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, time / duration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}
