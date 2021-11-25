using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalProperties : MonoBehaviour
{
    [SerializeField] float potential;
    [SerializeField] float capacity; //capacity of the battery in Ah
    [SerializeField] string manufacturer;
    [SerializeField] float soc;
    [SerializeField] bool charging;
    [SerializeField] float r_ohm = 1; //represents the resistance of the contacts in Ohm TODO: what would be a logical default value for this?
    [SerializeField] float act_elec_a = 0.4f; //the active electrode area (for the butler volmer equation) TODO: what is a logical value for this? can we initialize it depending on the real dimensions of the electrode?
    private int parentID;

    // Start is called before the first frame update
    void Start()
    {
        parentID = this.transform.parent.GetInstanceID(); //does what you think it does ;) in order to recognize which pole corresponds to which battery
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

    public int GetParentId() {
        return parentID;
    }

    public float GetPotential() {
        if (!HasElectrolyte()) {
            Debug.LogWarning("No Electrolyte class connected to this battery chassis!");
        }
        return CalcUGleichgewicht() + CalcUWiderstand() + CalcUReaktion() + CalcUDiffusion();
    }

    public float CalcUGleichgewicht() {
        return -1 * this.transform.parent.GetComponent<Electrolyte>().GetDeltaG() / (this.transform.parent.GetComponent<Electrolyte>().GetZ() * 96485); //U_o = - deltaG / (z * F) //TODO: wouldn't it be better to calculate this inside the Electrolyte class?
    }

    public float CalcUWiderstand() {
        return GetCurrentNow() * r_ohm; //resistance losses proportional to the current passing through the battery and the resistance of the contacts
    }

    public float CalcUReaktion() {
        return this.transform.parent.GetComponent<Electrolyte>().GetButlerVolmer(act_elec_a, GetCurrentNow());
    }

    public float CalcUDiffusion () {
        return 2.3f; //TODO: implement this formula inside the Electrolyte class!
    }

    public float GetCurrentNow() { //fetch the current flowing through the battery in this time (now)
        return 10.0f; //TODO: from what does the current depend?
    }

    public bool HasElectrolyte() {
        return this.transform.parent.GetComponent<Electrolyte>() != null;
    }
}
