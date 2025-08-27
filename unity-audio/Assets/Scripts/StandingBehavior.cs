using UnityEngine;

public class StandingBehavior : StateMachineBehaviour
{
    private PlayerController _playerController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController = animator.GetComponentInParent<PlayerController>();

        if (_playerController == null)
        {
            Debug.LogWarning("AnimationEndBehaviour: No PlayerController found on the Animator's GameObject!");
        }

    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController.StandBackUp();
    }
}
