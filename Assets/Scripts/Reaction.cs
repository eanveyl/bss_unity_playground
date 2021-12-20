using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction //: MonoBehaviour
{
    private string Anode;
    private string Cathode;
    private List<Element> Products;
    private List<Element> Reactants;

    public Reaction(string anode, string cathode, List<Element> reactants, List<Element> products)
    {
        Anode = anode;
        Cathode = cathode;
        Reactants = reactants;
        Products = products;

    }

    public List<Element> getReactants()
    {
        return Reactants;
    }

    public List<Element> getProducts()
    {
        return Products;
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
