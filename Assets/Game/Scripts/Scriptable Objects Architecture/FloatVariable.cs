using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Float Variable", fileName = "new FloatVariable")]
public class FloatVariable : GameEvent, ISerializationCallbackReceiver
{
    public float InitialValue = 0f;
    public float RuntimeValue;

    public float Value
    {
        get
        {
            return RuntimeValue;
        }
        set
        {
            RuntimeValue = value;
            Raise();
        }
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        RuntimeValue = InitialValue;
    }
}