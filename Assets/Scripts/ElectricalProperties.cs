using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalProperties : MonoBehaviour
{
    [SerializeField] float potential;
    public float capacity;
    public string manufacturer;
    public float soc;
    public bool charging;

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
        return potential;
    }

}
