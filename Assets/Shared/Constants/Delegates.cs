using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains common delegates. 
/// Events have parameters and can have return values, 
/// while actions only have return values.
/// </summary>
public class Delegates
{
    #region actions
    public delegate void Action(); //Same as System.Action
    public delegate bool BoolAction();
    #endregion
    #region events
    public delegate void IntEvent(int value);
    public delegate void BoolEvent(bool value);
    public delegate void BoolIntEvent(bool value, int value2);
    public delegate bool IntToBoolEvent(int value);
    public delegate void FloatEvent(float value);
    public delegate void Float2Event(float x, float y);
	public delegate void StringEvent(string text);
	public delegate void ColorEvent(Color value);
    public delegate void RigidbodyEvent(Rigidbody rigidbody);
	public delegate void Vector3Event(Vector3 vector);
    public delegate void Vector3Vector3Event(Vector3 vector1, Vector3 vector2);
    public delegate void BoolArrayEvent(bool[] array);

    //Program specifics
    
    #endregion
}
