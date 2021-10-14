using BehaviorTree;
using UnityEngine;

public class TaskChase : Node
{
    PlayerPeerController m_player;
    Vector3 _lastTargetPosition;

    public float stoppingDistance = 3f;

    public TaskChase(PlayerPeerController player) : base()
    {
        m_player = player;
        _lastTargetPosition = Vector3.zero;
    }

    public override NodeState Evaluate()
    {
        object currentTarget = GetData("currentTarget");
        Vector3 targetPosition = GetTargetPosition((Transform)currentTarget);


        m_player.MoveTo(targetPosition, m_player.maxSpeed, m_player.movementSharpness);
        _lastTargetPosition = targetPosition;


        _state = NodeState.RUNNING;
        return _state;
    }

    private Vector3 GetTargetPosition(Transform target)
    {
        return target.position;
    }
}