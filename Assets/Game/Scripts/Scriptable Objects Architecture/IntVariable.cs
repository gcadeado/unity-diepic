using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Int Variable", fileName = "new IntVariable")]
public class IntVariable : GameEvent, ISerializationCallbackReceiver
{

    //Initial value when starting the scene
    public int InitialValue;
    //Runtime value that'll be edited during play mode
    public int RuntimeValue = 0;

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