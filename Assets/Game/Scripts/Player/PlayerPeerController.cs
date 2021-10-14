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
    }

    protected override void OnDie(GameObject source)
    {
        Destroy(gameObject);
    }
}
