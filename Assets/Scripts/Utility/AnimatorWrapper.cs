using UnityEngine;

public class AnimatorWrapper : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    public string CurrentState { get; private set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayContinue(string state)
    {
        if (CurrentState.CompareTo(state) == 0)
            return;

        animator.Play(state);
        CurrentState = state;
    }

    public void PlayRestart(string state)
    {
        animator.Play(state);
        CurrentState = state;
    }
}