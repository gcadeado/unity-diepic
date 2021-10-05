using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectileBase))]
public class ProjectileStandard : MonoBehaviour
{
    [Header("General")]
    [Tooltip("Radius of this projectile's (used for sprite and collisions)")]
    public float radius = 1f;

    [Tooltip("LifeTime of the projectile")]
    public float maxLifeTime = 5f;

    [Tooltip("Layers this projectile can collide with")]
    public LayerMask hittableLayers = -1;

    [Header("Movement")]
    [Tooltip("Speed of the projectile")]
    public float speed = 3f;

    [Tooltip("Determines if the projectile inherits the velocity that the weapon's muzzle had when firing")]
    public bool inheritWeaponVelocity = false;

    [Header("Damage")]
    [Tooltip("Damage of the projectile")]
    public float damage = 40f;

    [Header("Debug")]
    [Tooltip("Projectile radius debug view color")]
    public Color radiusColor = Color.red * 0.2f;

    ProjectileBase m_ProjectileBase;
    Vector3 m_Velocity;
    float m_ShootTime;

    List<Collider> m_IgnoredColliders;

    CircleCollider2D m_Collider;

    SpriteRenderer m_SpriteRenderer;

    void OnEnable()
    {
        m_ProjectileBase = GetComponent<ProjectileBase>();
        DebugUtility.HandleErrorIfNullGetComponent<ProjectileBase, ProjectileStandard>(m_ProjectileBase, this, gameObject);

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        DebugUtility.HandleErrorIfNullGetComponent<SpriteRenderer, ProjectileStandard>(m_SpriteRenderer, this, gameObject);

        m_Collider = GetComponent<CircleCollider2D>();
        if (m_Collider == null)
        {
            m_Collider = gameObject.AddComponent<CircleCollider2D>();
            m_Collider.radius = radius * 0.2f;
        }

        m_ProjectileBase.onShoot += OnShoot;
    }

    void Start()
    {
        transform.localScale = new Vector3(radius, radius, 1f);
        Destroy(gameObject, maxLifeTime);
    }

    void OnShoot()
    {
        m_ShootTime = Time.time;
        m_Velocity = m_ProjectileBase.initialDirection * speed;
        transform.position += m_ProjectileBase.inheritedMuzzleVelocity * Time.deltaTime;

        m_IgnoredColliders = new List<Collider>();
        // Ignore colliders of owner
        Collider[] ownerColliders = m_ProjectileBase.owner.GetComponentsInChildren<Collider>();
        m_IgnoredColliders.AddRange(ownerColliders);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((hittableLayers.value & 1 << collider.gameObject.layer) == 1 << collider.gameObject.layer)
            OnHit(collider);
    }

    void OnHit(Collider2D collider)
    {
        // damage
        Damageable damageable = collider.GetComponent<Damageable>();
        if (damageable && m_ProjectileBase.owner != collider.gameObject)
        {
            damageable.InflictDamage(damage, m_ProjectileBase.owner);
            StartCoroutine(m_SpriteRenderer.SetColorAnimation(m_SpriteRenderer.color, new Color(0f, 0f, 0f, 0f), 0.05f));
            m_Collider.enabled = false;
            Destroy(this.gameObject, 0.05f); // TODO use object pool
        }
    }

    void Update()
    {
        // Move
        transform.position += m_Velocity * Time.deltaTime;
        if (inheritWeaponVelocity)
            transform.position += m_ProjectileBase.inheritedMuzzleVelocity * Time.deltaTime;

        // Orient towards velocity
        transform.up = m_Velocity.normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = radiusColor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}