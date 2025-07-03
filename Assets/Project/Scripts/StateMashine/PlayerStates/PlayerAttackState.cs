using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PlayerAttackState : EntityAttackState
{
    private PlayerInputController _inputController;

    private System.Action _buttonCanceledHandler;
    private System.Action _buttonStartedHandler;

    public PlayerAttackState(Player player, CollisionHandler collisionHandler,
        Collider collider, Rigidbody rigidbody,
        AnimatorController animatorController,
        BallHolder ballHolder, TargetScanner targetScanner,
        TargetProvider targetProvider, List<Entity> teammates,
        PlayerInputController inputController, BallThrower ballThrower) :
        base(player, collisionHandler, collider, rigidbody,
            animatorController, ballHolder, targetScanner,
            targetProvider, teammates, ballThrower)
    {
        _inputController = inputController;
    }

    public override void Enter()
    {
        Debug.Log("Enter Player Attack State");
        base.Enter();

        _buttonStartedHandler = OnButtonClicked;
        _buttonCanceledHandler = () => _ = OnButtonReleasedAsync();

        _inputController.ActionButtonStarted += _buttonStartedHandler;
        _inputController.ActionButtonCanceled += _buttonCanceledHandler;
    }

    private void OnButtonClicked()
    {
        Debug.Log("OnButtonClicked");
        if (TargetProvider.Target != null)
        {
            Debug.Log("Start Attack");
            StartAttack();
        }
    }

    private async Task OnButtonReleasedAsync()
    {
        _inputController.ActionButtonStarted -= _buttonStartedHandler;
        _inputController.ActionButtonCanceled -= _buttonCanceledHandler;


        StateSwitcher.SwitchState<PlayerIdleState>();
        await ThrowBall();
    }
}