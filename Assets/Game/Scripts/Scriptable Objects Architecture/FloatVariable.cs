using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Float Variable", fileName = "new FloatVariable")]
public class FloatVariable : GameEvent, ISerializationCallbackReceiver
{

    //Initial value when starting the scene
    public float InitialValue;
    //Runtime value that'll be edited during play mode
    public float RuntimeValue = 0f;

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