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
    [SerializeField] float temprature = 0f;
    [SerializeField] float current = 0f;
    [SerializeField] float area = 0f;
    [SerializeField] float currentDensity = 0f;
    [SerializeField] float z = 1f;
    [SerializeField] float alpha = 0.5f;

    private Element e;
    private Reaction r;


    //implemted functions


    //Gibbsche free energy
    public float GetDeltaG() { 
        return 42.0f; 
    }

    public float GetZ() {
        return z; 
    }

    public float GetTemprature()
    {
        return temprature;
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

        float z = GetZ();
        float T = GetTemprature();
        float I = GetCurrent();
        float A = GetArea();
        float j_0 = GetCurrentDensity();
        float alpha = GetAlpha();


        //approximation for small currents

        if (I < 1) {
                return (R * T) / (z * F) * (current / A * j_0);
        } else { //approximation for large currents
                float argument = Mathf.Abs(current / (A * j_0));
                return (((R * T) / (alpha * z * F))) * Mathf.Log(argument, 2.71f);
        }

    }


    //calculate UReaction

    public float I_Butt() {

            float z = GetZ(); //TODO: remove these variable assignments
            float T = GetTemprature();
            float I = GetCurrent();
            float A = GetArea();
            float j_0 = GetCurrentDensity();
            float alpha = GetAlpha();

            return A*j_0*(Mathf.Exp((alpha*z*F*deltaU())/R*T)-Mathf.Exp((-1)*((1-alpha)*z*F)/R*T*deltaU()));
    }

    public float UReaction()
    {
        float I = GetCurrent();
        return deltaU() / I_Butt() * I;
    }
  


    //calculate OSV
    //_____________


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
        return sumReactionProducts(reaction.getProducts()) - sumReactionReactants(reaction.getReactants());          
    }

    public float CalculateOSV(Reaction reaction)
    {
        return -1 * calculateDeltaG(reaction) / z * F;
    }

    //______________________________________________________________________________________

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


}

