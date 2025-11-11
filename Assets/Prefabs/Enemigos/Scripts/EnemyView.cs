using UnityEngine;

public class EnemyView : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeToAnimation(string paramName, float targetValue)
    {
        animator.SetFloat(paramName, targetValue);
    }

}
