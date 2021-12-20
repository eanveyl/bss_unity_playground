using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Electrolyte : MonoBehaviour
{
    /*
    [SerializeField] bool force_original_butler_volmer = true;
    [SerializeField] float threshold_butler_volmer = 10; //current threshold in Ampere for deciding to use simplifications of the Butler-Volmer Equations //TODO: what is a reasonable value here?
    // Start is called before the first frame update
    [SerializeField] float r_chargetransfer = 1; //TODO: is there a logical starting value for this?
    */
    [SerializeField] float F = 96485f; // [C/mol]
    [SerializeField] float R = 8.314f; // [J/mol*K]
    [SerializeField] float temperature = 293.15f; //20°C
    [SerializeField] float current; //TODO: on what does this value depend?
    [SerializeField] float area = 0.01f; 
    [SerializeField] float currentDensity; //TODO: dependency of this value? 
    [SerializeField] float z; //TODO: default value here?
    [SerializeField] float alpha = 0.5f;


    //Gibbsche free energy
    public float GetDeltaG() { //TODO: do we need this function if we have the calculateDeltaG() method?
        return 42.0f; 
    }

    public float GetZ() {
        return z; 
    }

    public float GetTemperature()
    {
        return temperature;
    }

    public float GetCurrent() {
        return current;
    }

    public float GetArea() {
        return area;
    }

    public float GetCurrentDensity() {
        return currentDensity;
    }

    public float GetAlpha() {
        return alpha;
    }


    //calculates deltaU
    public float deltaU() {

        if (current < 1) { //approximation for small currents
            return (R * temperature) / (z * F) * (current / area * currentDensity);
        } else { //approximation for large currents
            float argument = Mathf.Abs(current / (area * currentDensity));
            return (((R * temperature) / (alpha * z * F))) * Mathf.Log(argument, 2.71f);
        }

    }

    //calculate UReaction
    public float I_Butt() {
        return area*currentDensity*(Mathf.Exp((alpha*z*F*deltaU())/R*temperature)-Mathf.Exp((-1)*((1-alpha)*z*F)/R*temperature*deltaU()));
    }

    public float UReaction()
    {
        float I = GetCurrent();
        return deltaU() / I_Butt() * I;
    }

    //takes a list of Products, returns the sum of theor G_0 value
    public float sumReactionProducts(List<Element> element)
    {

        float tempSum = 0;
        for (int i = 0; i < element.Count; i++)
        {
           tempSum += element[i].getG_0();
        }
        return tempSum;
    }

    //takes a list of Reactants, returns the sum of their G_0 value
    public float sumReactionReactants(List<Element> element)
    {

        float tempSum = 0;
        for (int i = 0; i < element.Count; i++)
        {
            tempSum += element[i].getG_0();
        }
        return tempSum;
       
    }

    //takes a reaction and gets the list of Reactants/Producuts for the sumX functions
    public float calculateDeltaG(Reaction reaction)
    {
        float total = sumReactionProducts(reaction.getProducts()) - sumReactionReactants(reaction.getReactants());
        Debug.Log("delta_g =" + total.ToString());
        return total;          
    }

    public float CalculateOSV(Reaction reaction)
    {
        return -1 * calculateDeltaG(reaction) / z * F;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


}

