using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimaciones : MonoBehaviour
{
    private Animator animator;
    private string animacionActual;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void CambiarAnim(string nuevaAnim)
    {
        if(animacionActual == nuevaAnim)
        {
            return;
        }

        animator.Play(nuevaAnim);
        animacionActual = nuevaAnim;
    }

    public void AnimIdle()
    {
        CambiarAnim("Idle");
    }
    public void AnimCorrer()
    {
        CambiarAnim("Running");
    }
    public void AnimSaltar()
    {
        CambiarAnim("Jump");
    }
    public void AnimDeslizar()
    {
        CambiarAnim("Slice");
    }
    public void AnimColision()
    {
        CambiarAnim("Dead");
    }
}
