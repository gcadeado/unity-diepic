using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TrackIntTextUI : GameEventListener
{

    [SerializeField]
    private string preNumberText = "";

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

        textMeshPro.text = preNumberText + value.ToString();
    }

    //When the attached IntVariable is modified, call this function
    public override void OnEventRaised()
    {
        base.OnEventRaised();

        UpdateTextValue();
    }

}
