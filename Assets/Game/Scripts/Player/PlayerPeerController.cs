using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerPeerController : PlayerController
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Move(0, new Vector3(1f, 0, 0), 1, 15);
        m_weaponController.isFiring = (int)Time.timeSinceLevelLoad % 2 == 0;
    }

    protected override void OnDie(GameObject source)
    {
        Destroy(gameObject);
    }
}
