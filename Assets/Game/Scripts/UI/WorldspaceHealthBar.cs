using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class WorldspaceHealthBar : MonoBehaviour
{
    [Tooltip("Image component displaying health left")]
    public Image healthBarImage;
    [Tooltip("The floating healthbar pivot transform")]
    public Transform healthBarPivot;
    [Tooltip("Whether the health bar is visible when at full health or not")]
    public bool hideFullHealthBar = true;

    private Health m_health;

    void Awake()
    {
        m_health = GetComponent<Health>();
        DebugUtility
            .HandleErrorIfNullGetComponent
            <Health, WorldspaceHealthBar>(m_health,
            this,
            gameObject);
    }

    void Update()
    {
        // update health bar value
        healthBarImage.fillAmount = m_health.currentHealth / m_health.maxHealth;

        // hide health bar if needed
        if (hideFullHealthBar)
            healthBarPivot.gameObject.SetActive(healthBarImage.fillAmount != 1);
    }
}
