using UnityEngine;

[RequireComponent(typeof(ProjectileBase))]
public class ProjectileStandard : MonoBehaviour
{
    [Header("General")]
    [Tooltip("Radius of this projectile's collision detection")]
    public float radius = 0.01f;
    [Tooltip("LifeTime of the projectile")]
    public float maxLifeTime = 5f;

    [Header("Movement")]
    [Tooltip("Speed of the projectile")]
    public float speed = 20f;
    [Tooltip("Determines if the projectile inherits the velocity that the weapon's muzzle had when firing")]
    public bool inheritWeaponVelocity = false;

    [Header("Debug")]
    [Tooltip("Projectile radius debug view color")]
    public Color radiusColor = Color.red * 0.2f;

    ProjectileBase m_ProjectileBase;
    Vector3 m_Velocity;
    float m_ShootTime;

    private void OnEnable()
    {
        m_ProjectileBase = GetComponent<ProjectileBase>();
        DebugUtility.HandleErrorIfNullGetComponent<ProjectileBase, ProjectileStandard>(m_ProjectileBase, this, gameObject);

        m_ProjectileBase.onShoot += OnShoot;

        Destroy(gameObject, maxLifeTime);
    }

    void OnShoot()
    {
        m_ShootTime = Time.time;
        m_Velocity = m_ProjectileBase.initialDirection * speed;
        transform.position += m_ProjectileBase.inheritedMuzzleVelocity * Time.deltaTime;
    }

    void Update()
    {
        // Move
        transform.position += m_Velocity * Time.deltaTime;
        if (inheritWeaponVelocity)
        {
            transform.position += m_ProjectileBase.inheritedMuzzleVelocity * Time.deltaTime;
        }

        // Orient towards velocity
        transform.up = m_Velocity.normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = radiusColor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}