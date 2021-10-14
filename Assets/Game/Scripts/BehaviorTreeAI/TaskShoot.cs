using BehaviorTree;
using UnityEngine;

public class TaskShoot : Node
{
    PlayerPeerController m_player;

    public TaskShoot(PlayerPeerController player) : base()
    {
        m_player = player;
    }

    public override NodeState Evaluate()
    {
        object currentTarget = GetData("currentTarget");
        Vector3 targetPosition = GetTargetPosition((Transform)currentTarget);
        m_player.LookTo(targetPosition);
        m_player.OnFire(true);
        _state = NodeState.SUCCESS;
        return _state;
    }

    private Vector3 GetTargetPosition(Transform target)
    {
        return target.position;
    }
}