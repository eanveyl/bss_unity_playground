using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElectricalComponent : MonoBehaviour
{
    public abstract float GetPotenial();
    public abstract float GetResistance();
    public abstract void Initialize();

    public abstract float GetZ();
    public abstract float GetTemp();
    public abstract float GetArea();
    public abstract float GetJ();
    public abstract float GetAlpha();
    public abstract int GetComponentType();
    
}




                /*
                    DATABANK
                        Pb = new Element("Pb", 0, 0, 207.2f, 18.27f, 11.34f, Mathf.Pow(5.3f, 3f), 26.46f,1);
                        PbO2 = new Element("PbO2", -277.4f, -217.4f, 239.2f, 25.72f, 9.3f, Mathf.Pow(1.35f, 2f), 64.68f,1);
                        PbSO4 = new Element("PbSO4", -919.9f, -813.2f, 303.3f, 48.22f, 6.29f, Mathf.Pow(1f, -8f), 103.16f,2);
                        H_plus = new Element("H+", 0, 0, 1.008f, float.NaN, float.NaN, float.NaN, float.NaN,1);
                        HSO4 = new Element("HSO4", -887.3f, -755.4f, 97.08f, float.NaN, float.NaN, float.NaN, float.NaN,1);
                        H2O = new Element("H2O", -285.8f, -237.2f, 18.02f, 18.02f, 1f, float.NaN, 75.6f,2);
                        H2SO4 = new Element("H2SO4", -814f, -690.1f, 98.08f, float.NaN, float.NaN, Mathf.Pow(1.22f, -2f), 303.8f,2);
                */