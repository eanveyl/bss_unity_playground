using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electrolyte : MonoBehaviour
{
    [SerializeField] bool force_original_butler_volmer = true;
    [SerializeField] float threshold_butler_volmer = 10; //current threshold in Ampere for deciding to use simplifications of the Butler-Volmer Equations //TODO: what is a reasonable value here?
    // Start is called before the first frame update
    [SerializeField] float r_chargetransfer = 1; //TODO: is there a logical starting value for this?
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetDeltaG() { //Gibbsche free energy
        return 42.0f; //TODO: get the real energy from the chemical composition
    }

    public float GetZ() {
        return 6f; //TODO: get the freigesetzte elektronenmenge (mol) from the chemical composition
    }

    public float GetButlerVolmer(float active_electrode_area, float current) { //TODO: obviously we're gonna have more input parameters, this is just an example...
        float temp_result;

        if (force_original_butler_volmer) {
            temp_result = 2.5f * r_chargetransfer; //active_electrode_area * I_o ... TODO: implement this formula TODO: remember that deltaU = R_ct * I (see VL2, folie 66-67) so it's butler-volmer * r_chargetransfer for the original case
        } else if (current < threshold_butler_volmer) {
            temp_result = 1.2f; //näherung für kleine Ströme deltaU = (R*T*I)/(F*A*i_o) here we don't need to consider the charge transfer resistance
        } else {
            temp_result = 0.5f; //TODO: implement näherung für große Ströme (Tafel Gleichung) deltaU = (R*T)/(alpha*f) * ln(abs(I/(A*i_o)))
        }
        
        return temp_result;
    }
}
