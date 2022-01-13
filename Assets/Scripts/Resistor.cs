using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resistor : ElectricalComponent
{
    [SerializeField] float Resistance = 0;

    public override float GetResistance()
    {
        return Resistance;
    }
    public override float GetPotenial()
    {
        Debug.Log("Potenial of Resistor is 0");
        return 0 ;
    }
    public override void Initialize() { }


    public override float GetZ()
    {
        throw new System.NotImplementedException();
    }

    public override float GetTemp()
    {
        throw new System.NotImplementedException();
    }

    public override float GetArea()
    {
        throw new System.NotImplementedException();
    }

    public override float GetJ()
    {
        throw new System.NotImplementedException();
    }

    public override float GetAlpha()
    {
        throw new System.NotImplementedException();
    }

    public override int GetComponentType()
    {
        return 2;
    }


}
