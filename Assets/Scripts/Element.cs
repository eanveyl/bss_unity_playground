using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    private string Reactant = "";
    private float H_0 = 0 ;
    private float G_0 = 0;
    private float MoleWeight = 0;
    private float MoleVolume = 0;
    private float Density = 0;
    private float Conductivity = 0;
    private float ThermalCapacity = 0;
    private int Index = 0;

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
