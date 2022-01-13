using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : ElectricalComponent 
{

    public override void Initialize() { }

    public override float GetPotenial()
    {
        return 0;
    }
    public override float GetResistance()
    {
        return 0;
    }

    public override float GetTemp()
    {
        throw new System.NotImplementedException();
    }

    public override float GetZ()
    {
        throw new System.NotImplementedException();
    }

    public override float GetAlpha()
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

    public override int GetComponentType()
    {
        return 3;
    }



}
