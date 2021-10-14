using System.Collections.Generic;

using BehaviorTree;

[UnityEngine.RequireComponent(typeof(PlayerPeerController))]
public class PlayerBT : Tree
{
    PlayerPeerController m_player;

    private void Awake()
    {
        m_player = GetComponent<PlayerPeerController>();
    }

    protected override Node SetupTree()
    {
        Node _root;

        // prepare our subtrees...
        Sequence moveToTargetSequence = new Sequence(new List<Node> {
            new CheckHasTarget()
        });

        Sequence shootSequence = new Sequence(new List<Node> {
            new CheckEnemyInAttackRange(m_player)
        });

        shootSequence.Attach(new TaskShoot(m_player));

        moveToTargetSequence.Attach(new Selector(new List<Node> {
            shootSequence,
            new TaskChase(m_player)
        }));

        // ... then stitch them together under the root
        _root = new Sequence(new List<Node> {
            new CheckEnemyInFOVRange(m_player),
            new Sequence(new List<Node>
            {
                moveToTargetSequence,
            }),
        });

        return _root;
    }
}
