using TMPro;
using UnityEngine;

public class BulletCountUI : GameEventListener
{
    private void Start()
    {
        UpdateTextValue();
    }

    private void OnValidate()
    {
        UpdateTextValue();
    }

    public void UpdateTextValue()
    {
        TextMeshProUGUI textMeshPro = GetComponent<TextMeshProUGUI>();
        int value = ((IntVariable)Event).Value;

        textMeshPro.text = value.ToString();
    }

    //When the attached IntVariable is modified, call this function
    public override void OnEventRaised()
    {
        base.OnEventRaised();

        UpdateTextValue();
    }
}
