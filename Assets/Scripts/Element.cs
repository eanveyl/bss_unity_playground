using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    [SerializeField] string Reactant = "";
    [SerializeField] float H_0 = 0 ;
    [SerializeField] float G_0 = 0;
    [SerializeField] float MoleWeight = 0;
    [SerializeField] float MoleVolume = 0;
    [SerializeField] float Density = 0;
    [SerializeField] float Conductivity = 0;
    [SerializeField] float ThermalCapacity = 0;
    [SerializeField] int Index = 0;

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

    public float getG_0(Element element)
    {
        return element.G_0;
    }

    public int getIndex(Element element)
    {
        return element.Index;
    }

    public Element Pb = new Element("Pb", 0, 0, 207.2f, 18.27f, 11.34f, Mathf.Pow(5.3f, 3f), 26.46f,1);
    public Element PbO2 = new Element("PbO2", -277.4f, -217.4f, 239.2f, 25.72f, 9.3f, Mathf.Pow(1.35f, 2f), 64.68f,1);
    public Element PbSO4 = new Element("PbSO4", -919.9f, -813.2f, 303.3f, 48.22f, 6.29f, Mathf.Pow(1f, -8f), 103.16f,2);
    public Element H_plus = new Element("H+", 0, 0, 1.008f, float.NaN, float.NaN, float.NaN, float.NaN,1);
    public Element HSO4 = new Element("HSO4", -887.3f, -755.4f, 97.08f, float.NaN, float.NaN, float.NaN, float.NaN,1);
    public Element H2O = new Element("H2O", -285.8f, -237.2f, 18.02f, 18.02f, 1f, float.NaN, 75.6f,2);
    public Element H2SO4 = new Element("H2SO4", -814f, -690.1f, 98.08f, float.NaN, float.NaN, Mathf.Pow(1.22f, -2f), 303.8f,2);

    // Start is called before the first frame update
    void Start()
    {

         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
