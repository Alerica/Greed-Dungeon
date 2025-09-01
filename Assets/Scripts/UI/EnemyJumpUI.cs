using UnityEngine;

public class EnemyJumpUI : MonoBehaviour
{
    [SerializeField] private RectTransform target;
    [SerializeField] private float jumpHeight = 100f;
    [SerializeField] private float scaleMultiplier = 2f;
    [SerializeField] private float duration = 1f; // full cycle
    [SerializeField] private Player player;
    private Enemy enemyScript;
    [SerializeField] private GameObject SpawnPoint;

    public void PlayJump()
    {
        StopAllCoroutines();
        StartCoroutine(JumpAnim());
    }

    private System.Collections.IEnumerator JumpAnim()
    {
        Vector3 startPos = target.localPosition;
        Vector3 upPos = startPos + Vector3.up * jumpHeight;
        Vector3 startScale = target.localScale;
        Vector3 bigScale = startScale * scaleMultiplier;
        enemyScript = SpawnPoint.GetComponentInChildren<Enemy>();
        float halfDuration = duration / 2f;
        float t = 0f;

        // -------- PHASE 1 --------
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            float normalized = t / halfDuration;

            // first half: up + scale up, then down
            float yCurve = Mathf.Sin(normalized * Mathf.PI); // up then down
            target.localPosition = Vector3.Lerp(startPos, upPos, yCurve);

            // scale only grows here
            target.localScale = Vector3.Lerp(startScale, bigScale, normalized);

            yield return null;

        }
        Debug.Log($"Enemy attack with {enemyScript.GetAttackPower()}");
        player.TakeDamage(enemyScript.GetAttackPower());
        BattleManager.Instance.UpdatePlayerHP();

        // -------- PHASE 2 --------
        t = 0f;
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            float normalized = t / halfDuration;

            // second half: up again then down
            float yCurve = Mathf.Sin(normalized * Mathf.PI);
            target.localPosition = Vector3.Lerp(startPos, upPos, yCurve);

            // scale shrinks back
            target.localScale = Vector3.Lerp(bigScale, startScale, normalized);

            yield return null;
        }

        // make sure it ends exactly at start
        target.localPosition = startPos;
        target.localScale = startScale;
    }
    public float GetJumpDuration()
    {
        return duration;
    }
}
