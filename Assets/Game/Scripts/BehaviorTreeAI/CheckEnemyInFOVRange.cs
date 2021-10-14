using System.Collections.Generic;
using System.Linq;
using BehaviorTree;
using UnityEngine;

public class CheckEnemyInFOVRange : Node
{
    PlayerPeerController m_player;
    public float fovRadius = 10.0f; // test

    Vector3 m_pos;

    public CheckEnemyInFOVRange(PlayerPeerController player) : base()
    {
        m_player = player;
    }

    public override NodeState Evaluate()
    {
        m_pos = m_player.transform.position;
        IEnumerable<Collider2D> enemiesInRange =
            Physics2D.OverlapCircleAll(m_pos, fovRadius) // TODO COLLIDE ONLY WITH GIVEN MASK
            .Where(delegate (Collider2D c)
            {
                PlayerController player = c.GetComponent<PlayerController>();
                if (player == null || player == m_player)
                    return false;
                return true;
            });
        if (enemiesInRange.Any())
        {
            Debug.Log("[PlayerBT] Found " + enemiesInRange.Count() + " enemy");
            Parent.SetData(
                "currentTarget",
                enemiesInRange
                    .OrderBy(x => (x.transform.position - m_pos).sqrMagnitude)
                    .First()
                    .transform
            );
            _state = NodeState.SUCCESS;
            return _state;
        }
        _state = NodeState.FAILURE;
        return _state;
    }
}