using UnityEngine;

public class AnimatiosMenu : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TrueAnimation(string Nameanimation) { 
    
        animator.SetBool(Nameanimation, true);
    }

    public void FalseAnimation(string Nameanimation)
    {
        animator.SetBool(Nameanimation, false);
    }

}
