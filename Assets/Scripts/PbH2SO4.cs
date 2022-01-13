using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PbH2SO4 : ElectricalComponent
{
    private readonly float F = 96485f; // [C/mol]
    private readonly List<Molecule> Reactants = new List<Molecule>();
    private readonly List<Molecule> Products = new List<Molecule>();
    

//---------------------------------------------------------------------------------------------------------------
    //Electrolyte Charachteristics 
    [SerializeField] readonly float z = 2f;                      //number of electrons being passed around per reaction
    [SerializeField] readonly float Resistance = 0.1f;             //electrolyte dependant
    [SerializeField] readonly float temperature = 293.15f;       //20°C
    [SerializeField] readonly float area = 0.01f;                //active area
    [SerializeField] readonly float currentDensity = 1;          //electrolyte dependant
    [SerializeField] readonly float alpha = 0.5f;                //electrolyte dependant



    //Hardsets the G_0 of the electrolyte reaction
    public override void Initialize()
    {
        if (Reactants.Count != 0) { return; }                           //not fill list twice if already full
        Reactants.Add(new Molecule() { G_0 = 0 });                      //Pb
        Reactants.Add(new Molecule() { G_0 = (-217.4f)*1000 });         //PbO2
        Reactants.Add(new Molecule() { G_0 = 2*(-755.4f)*1000 });       //2HSO4
        Reactants.Add(new Molecule() { G_0 = 0 });                      //2H+

        Products.Add(new Molecule() { G_0 = 2*(-813.2f)*1000 });        //2PBSO4
        Products.Add(new Molecule() { G_0 = 2*(-237.2f)*1000 });        //2H2O
        //Products.Add(new Molecule() { G_0 = 0 });                       
        //Products.Add(new Molecule() { G_0 = 0 });
    }
//---------------------------------------------------------------------------------------------------------------

    /*TODO: check whether setters/getters are needed here!!
    public List<Molecule> GetReactants()
    {
        return Reactants;
    }

    public List<Molecule> GetProducts()
    {
        return Products;
    }
    */

    //returns electrode potenial
    public override float GetPotenial()
    {
        float potenial = (-1)*GetDeltaG( Reactants, Products) / (z * F);
        return potenial;
    }

    private float GetDeltaG(List<Molecule> reactants, List<Molecule> products)
    {
        float reactantsSum = 0;
        float productsSum = 0;
        for (int i = 0; i < reactants.Count; i++)
        {
            reactantsSum += reactants[i].G_0;
        }
        for (int i = 0; i < products.Count; i++)
        {
            productsSum += products[i].G_0;
        }
        
        return productsSum - reactantsSum;

    }

    public override float GetResistance()
    {
        return Resistance;
    }


    // these getters are needed for ButlerVolmer
    public override float GetZ()
    {
        return z;
    }

    public override float GetTemp()
    {
        return temperature ;
    }

    public override float GetArea()
    {
        return area;
    }

    public override float GetJ()
    {
        return currentDensity;
    }

    public override float GetAlpha()
    {
        return alpha;
    }

    public override int GetComponentType()
    {
        return 1;
    }


}
