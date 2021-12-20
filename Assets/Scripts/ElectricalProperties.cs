using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalProperties : MonoBehaviour
{
    [SerializeField] float capacity; //capacity of the battery in Ah 
    [SerializeField] string manufacturer;
    [SerializeField] float soc;
    [SerializeField] bool charging;
    [SerializeField] bool is_cathode;
    [SerializeField] float r_ohm = 1; //represents the resistance of the contacts in Ohm TODO: what would be a logical default value for this?
    //[SerializeField] float act_elec_a = 0.4f; //the active electrode area (for the butler volmer equation) TODO: what is a logical value for this? can we initialize it depending on the real dimensions of the electrode?
    private int parentID;
    private List<Element> availableChemicalElements = new List<Element>();
    public static Element Pb, PbO2, PbSO4, H_plus, HSO4, H2O, H2SO4; //declare the elements as GLOBAL variables (easier to access them everywhere)
    private Reaction object_reaction;

    // Start is called before the first frame update
    void Start()
    {
        parentID = this.transform.parent.GetInstanceID(); //does what you think it does ;) in order to recognize which pole corresponds to which battery
        InitializeDefaultChemicalElements(); //initializes the chemical elements that can be used
        
        List<Element> dummy_reactant_list = new List<Element>(){Pb, PbO2, HSO4, HSO4, H_plus, H_plus}; //TODO: this is just a dummy list for TESTING
        List<Element> dummy_product_list = new List<Element>(){PbSO4, PbSO4, H2O, H2O}; //TODO: this is just a dummy list for TESTING
        Reaction object_reaction = new Reaction("dummy1", "dummy2", dummy_reactant_list, dummy_product_list);
    } 

    // Update is called once per frame
    void Update()
    {
        
    }

    public string PrettyPrintDataSheet() {
        string output = "Battery Information\n----------------------------------\n";
        output += "Capacity: " + capacity.ToString() + " Ah" +"\n";
        output += "Manufacturer: " + manufacturer + "\n";
        output += "State of Charge: " + soc.ToString() + " %" + "\n";
        if (charging) {
            output += "Charging: True \n";
        } else {
            output += "Charging: False \n";
        }
        output += "Parent ID: " + parentID.ToString() + "\n";
        return output;
    }

    
    private void InitializeDefaultChemicalElements() { //initialize the chemical elements that can be used.
        //TODO: maybe we can initialize this on another place, where its better?
        Pb = new Element("Pb", 0, 0, 207.2f, 18.27f, 11.34f, Mathf.Pow(5.3f, 3f), 26.46f,1);
        PbO2 = new Element("PbO2", -277.4f, -217.4f, 239.2f, 25.72f, 9.3f, Mathf.Pow(1.35f, 2f), 64.68f,1);
        PbSO4 = new Element("PbSO4", -919.9f, -813.2f, 303.3f, 48.22f, 6.29f, Mathf.Pow(1f, -8f), 103.16f,2);
        H_plus = new Element("H+", 0, 0, 1.008f, float.NaN, float.NaN, float.NaN, float.NaN,1);
        HSO4 = new Element("HSO4", -887.3f, -755.4f, 97.08f, float.NaN, float.NaN, float.NaN, float.NaN,1);
        H2O = new Element("H2O", -285.8f, -237.2f, 18.02f, 18.02f, 1f, float.NaN, 75.6f,2);
        H2SO4 = new Element("H2SO4", -814f, -690.1f, 98.08f, float.NaN, float.NaN, Mathf.Pow(1.22f, -2f), 303.8f,2);

        availableChemicalElements.Add(Pb);
        availableChemicalElements.Add(PbO2);
        availableChemicalElements.Add(PbSO4);
        availableChemicalElements.Add(H_plus);
        availableChemicalElements.Add(HSO4);
        availableChemicalElements.Add(H2O);
        availableChemicalElements.Add(H2SO4);
    }

    public int GetParentId() {
        return parentID;
    }

    public float GetPotential() {
        if (!HasElectrolyte()) {
            Debug.LogWarning("No Electrolyte class connected to this battery chassis!");
        }

        if (is_cathode) {
            return CalcUGleichgewicht() + CalcUWiderstand() + CalcUReaktion() + CalcUDiffusion(); //only if we touch the cathode (plus pole) 
            //... we return the potential
        } else {
            return 0; //in the case this object is actually the anode (minus pole)
        }
        
    }

    public float CalcUGleichgewicht() {
        float u_gleichgewicht = -1 * this.transform.parent.GetComponent<Electrolyte>().GetDeltaG() / (this.transform.parent.GetComponent<Electrolyte>().GetZ() * 96485); //U_o = - deltaG / (z * F) //TODO: wouldn't it be better to calculate this inside the Electrolyte class?
        Debug.Log("u_gleichgewicht = " + u_gleichgewicht.ToString());
        return u_gleichgewicht;
    }

    public float CalcUWiderstand() {
        float u_widerstand = GetCurrentNow() * r_ohm; //resistance losses proportional to the current passing through the battery and the resistance of the contacts
        Debug.Log("u_widerstand = " + u_widerstand.ToString());
        return u_widerstand;
    }

    public float CalcUReaktion() {        
        float u_reaction = this.transform.parent.GetComponent<Electrolyte>().CalculateOSV(object_reaction);
        Debug.Log("u_reaction = " + u_reaction.ToString());
        return u_reaction;
    }

    public float CalcUDiffusion () { //TODO: implement this formula inside the Electrolyte class!
        float u_diffusion = 2.3f;
        Debug.Log("u_difussion = " + u_diffusion.ToString());
        return u_diffusion;
    }

    public float GetCurrentNow() { //fetch the current flowing through the battery in this time (now)
        return this.transform.parent.GetComponent<Electrolyte>().GetCurrent(); //TODO: from what does the current depend? //TODO: implement the resistance of the connection (ray)
    }

    public bool HasElectrolyte() {
        return this.transform.parent.GetComponent<Electrolyte>() != null;
    }
}
