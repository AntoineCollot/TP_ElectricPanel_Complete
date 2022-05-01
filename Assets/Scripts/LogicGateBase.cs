using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LogicGate : MonoBehaviour
{
    public ConnectionPin[] connectionPins;

    public abstract bool UseGate();
}
