using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Int Variable", fileName = "new IntVariable")]
public class IntVariable : GameEvent, ISerializationCallbackReceiver
{
    public int InitialValue = 0;
    public int RuntimeValue;

    public int Value
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
        //When stopping play, return Runtime value to Initial Value.
        RuntimeValue = InitialValue;
    }
}