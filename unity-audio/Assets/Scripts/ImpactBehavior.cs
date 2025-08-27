using UnityEngine;

public class ImpactBehavior : StateMachineBehaviour
{
    private PlayerController _playerController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController = animator.GetComponentInParent<PlayerController>();

        if (_playerController == null)
        {
            Debug.LogWarning("AnimationEndBehaviour: No PlayerController found on the Animator's GameObject!");
        }
        _playerController.TriggerImpactSound();
    }
}
