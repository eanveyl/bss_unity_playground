using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction : MonoBehaviour
{
    [SerializeField] string Anode;
    [SerializeField] string Cathode;



    private Element e;
    private Element[] Products;
    private Element[] Reactants;

    public Reaction(string anode, string cathode, Element[] reactants, Element[] products)
    {
        Anode = anode;
        Cathode = cathode;
        Reactants = reactants;
        Products = products;

    }

    
    public void setReactants(params Element[] element)
    {
        for (int i = 0; i < element.Length; i++)
        {
            Reactants[i] = element[i];
        }
    }

    public void setProducts(params Element[] element)
    {
        for (int i = 0; i < element.Length; i++)
        {
            Products[i] = element[i];
        }
    }

    public Element[] getReactants()
    {
        return Reactants;
    }

    public Element[] getProducts()
    {
        return Products;
    }




    public Reaction PbPbO2 = new Reaction("Pb", "PbO2", new Element[] { }, new Element[] { });
    



    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        setReactants(e.Pb, e.PbO2, e.H2SO4);
        setProducts(e.PbSO4, e.H2O);

    }
}
