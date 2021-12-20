using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element //: MonoBehaviour
{
    private string Reactant;
    private float H_0;
    private float G_0;
    private float MoleWeight;
    private float MoleVolume;
    private float Density;
    private float Conductivity;
    private float ThermalCapacity;
    private int Index;

    public Element(string reactant, float h_0, float g_0, float moleWeight, float moleVolume, float density, float conductivity, float thermalCapacity, int index)
    {
        Reactant = reactant;
        H_0 = h_0;
        G_0 = g_0;
        MoleVolume = moleVolume;
        MoleWeight = moleWeight;
        Density = density;
        Conductivity = conductivity;
        ThermalCapacity = thermalCapacity;
        Index = index;
    }

    public float getG_0()
    {
        return this.G_0;
    }

    public int getIndex()
    {
        return this.Index;
    }

    // Start is called before the first frame update
    void Start()
    {

         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
