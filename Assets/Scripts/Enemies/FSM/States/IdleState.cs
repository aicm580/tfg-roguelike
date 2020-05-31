using UnityEngine;

public class IdleState : State
{
    public IdleState(GameObject enemy, StateType state) : base(enemy, state) { }

    public override void UpdateState()
    {
        if (enemy.GetComponentInChildren<Renderer>().isVisible)
        {
            enemy.GetComponent<FiniteStateMachine>().EnterNextState();
        }
    }
}
