using BehaviorTree;
using UnityEngine;

public class CheckEnemyInAttackRange : Node
{
    PlayerPeerController m_player;
    public float attackRange = 5f;

    public CheckEnemyInAttackRange(PlayerPeerController player) : base()
    {
        m_player = player;
    }

    public override NodeState Evaluate()
    {
        object currentTarget = Parent.GetData("currentTarget");
        if (currentTarget == null)
        {
            _state = NodeState.FAILURE;
            return _state;
        }

        Transform target = (Transform)currentTarget;

        if (!target)
        {
            Parent.ClearData("currentTarget");
            _state = NodeState.FAILURE;
            return _state;
        }

        float d = Vector3.Distance(m_player.transform.position, target.position);
        bool isInRange = d <= attackRange;
        if (!isInRange)
            m_player.OnFire(false);

        _state = isInRange ? NodeState.SUCCESS : NodeState.FAILURE;
        return _state;
    }
}