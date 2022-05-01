using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGateAnd : LogicGate
{
    public override bool UseGate()
    {
        return connectionPins[0].IsOn && connectionPins[1].IsOn;
    }
}
