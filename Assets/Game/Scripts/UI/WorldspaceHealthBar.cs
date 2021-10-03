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

    [Tooltip("Full health bar color")]
    public Color colorFullHeath = Color.green;

    [Tooltip("Depleted health bar color")]
    public Color colorDepletedHeath = Color.red;

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
        float healthRatio = m_health.currentHealth / m_health.maxHealth;
        healthBarImage.color = Color.Lerp(colorDepletedHeath, colorFullHeath, healthRatio);
        healthBarImage.fillAmount = healthRatio;

        // hide health bar if needed
        if (hideFullHealthBar)
            healthBarPivot.gameObject.SetActive(healthBarImage.fillAmount != 1);
    }
}
