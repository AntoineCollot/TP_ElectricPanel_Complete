using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGateNot : LogicGate
{
    public override bool UseGate()
    {
        return !connectionPins[0].IsOn;
    }
}
