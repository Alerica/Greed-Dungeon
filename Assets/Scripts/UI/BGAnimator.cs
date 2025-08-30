using UnityEngine;

public class BGAnimator : MonoBehaviour
{
    public Animator animator;
    public string triggerName = "Start";

    public void StartAnimation()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (animator != null && !string.IsNullOrEmpty(triggerName))
            animator.SetTrigger(triggerName);
    }
}
