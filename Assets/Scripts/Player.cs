using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool jumpKeyWasPressed;
    private float x_axis_movement;
    private float z_axis_movement;
    private bool isOnGround;
    private Vector3 first_hit_object_position;
    private RaycastHit first_hit_object;
    private Vector3 second_hit_object_position;
    private RaycastHit second_hit_object;
    [SerializeField] Material my_line_material;
    private List<GameObject> connected_lines = new List<GameObject>();
    private float F = 96485f; // [C/mol]
    private float R = 8.314f; // [J/mol*K]
    private float current { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true) {
            jumpKeyWasPressed = true;
        } else if (Input.GetKey(KeyCode.UpArrow) == true) { 
            x_axis_movement = 0.3f;
        } else if (Input.GetKey(KeyCode.DownArrow) == true) { //alternativ hierfuer auch horizontalInput = Input.GetAxis("Horizontal");
            x_axis_movement = -0.3f;
        } else if (Input.GetKey(KeyCode.RightArrow) == true) {
            z_axis_movement = 0.3f;
        } else if (Input.GetKey(KeyCode.LeftArrow) == true) {
            z_axis_movement = -0.3f;
        } else if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Boom!");
            ShootPrimaryRay();
        } else if (Input.GetMouseButtonDown(1)) {
            ShootSecondaryRay();
        } else if (Input.GetKeyDown(KeyCode.L) == true) {
            Debug.Log("Drawing line");
            ConnectAndDrawLineFirst2SecondObj();
        }        
    }
    

    //Fixed Update is called once every physic update
    private void FixedUpdate()
    {
        if (jumpKeyWasPressed) {
            if (!isOnGround) {
                return; //dont allow air jumps
            }
            Debug.Log("Jump!");
            GetComponent<Rigidbody>().AddForce(Vector3.up*6, ForceMode.Impulse);
            jumpKeyWasPressed = false;
        } else if (x_axis_movement != 0) {
            Debug.Log("Moving character forward or backwards!");
            GetComponent<Rigidbody>().AddForce(transform.forward * x_axis_movement, ForceMode.Impulse);
            x_axis_movement = 0;
        }
        if (z_axis_movement != 0) {
            Debug.Log("Moving character left or right!");
            GetComponent<Rigidbody>().AddForce(transform.right * z_axis_movement, ForceMode.Impulse);
            z_axis_movement = 0;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        isOnGround = true;
    }

    private void OnCollisionExit(Collision collision) {
        isOnGround = false;
    }

    public List<GameObject> getListofConnections()
    {
        return connected_lines;
    }

    private void ShootPrimaryRay()
    {
        Vector3 fwd = Camera.main.transform.forward;
        Debug.DrawRay(transform.position, fwd * 10, Color.red, 5);
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, fwd, out hit, (float) 10)) {
            Debug.Log("There is something in front of the object!");
            
            Material hit_material = hit.collider.GetComponent<Renderer>().material;
            first_hit_object = hit; //save for future reference
            hit_material.color = Color.magenta;
            first_hit_object_position = hit.transform.position;
            Debug.Log("First Ray hit:");
            Debug.Log(first_hit_object_position);

        }

        else
        {
            Debug.Log("Nothing hit!");
        }      
       
    }

    private void ShootSecondaryRay()
    {
        Vector3 fwd = Camera.main.transform.forward;//transform.TransformDirection(Vector3.forward);

        
        Debug.DrawRay(transform.position, fwd * 10, Color.blue, 5);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, fwd, out hit, (float) 10)) {
            Debug.Log("There is something in front of the object!");
            Material hit_material = hit.collider.GetComponent<Renderer>().material;
            second_hit_object = hit; //save for future reference
            hit_material.color = Color.magenta;
            second_hit_object_position = hit.transform.position;
            Debug.Log("Second Ray hit:");
            Debug.Log(second_hit_object_position);

        } else {
            Debug.Log("Nothing hit!");
        }    
    }

    private void ConnectAndDrawLineFirst2SecondObj()
    {
        //Draw Line
        GameObject myLine = new GameObject();
        myLine.transform.position = first_hit_object_position;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = my_line_material;
        float alpha = 0.6f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.red, 5.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lr.colorGradient = gradient;

        lr.startWidth = 0.02f;
        lr.endWidth = 0.02f;

        lr.SetPosition(0, first_hit_object_position);
        lr.SetPosition(1, second_hit_object_position);
        Debug.Log("Checking requirements for connection");
        
        
        if (first_hit_object.collider.GetComponentInParent<ElectricalComponent>() != null && second_hit_object.collider.GetComponentInParent<ElectricalComponent>() != null)
        { 
            Debug.Log("Both objects have electrical properties. A connection is valid.");
            myLine.AddComponent<ConnectionInformation>();
            myLine.GetComponent<ConnectionInformation>().InitializeConnections(first_hit_object, second_hit_object);
            
            connected_lines.Add(myLine); // add the current line to the connected lines 
            
            if (connected_lines[0].GetComponent<ConnectionInformation>().GetFirstHit().collider.GetComponentInParent<ElectricalComponent>().GetComponentType() == 1 && connected_lines[connected_lines.Count-1].GetComponent<ConnectionInformation>().GetSecondHit().collider.GetComponentInParent<ElectricalComponent>().GetComponentType() == 3) {
                Debug.Log("Connection OK and circuit was closed with ground");
                InformHUDAboutNewConnectedObjects();
            } else {
                Debug.Log("Connection OK but remember to close the circuit (Hint: first hit of the first line is expected to be a battery object and second hit of last line is expected to be ground");
            }
            
        }
        else // in the case user tries to connect objects without electrical properties
        { 
            Debug.LogWarning("One or more objects is not an Electrical Component. A connection is invalid. Deleting in 3 seconds.");
            GameObject.Destroy(myLine, 3f); //3 seconds duration
        }
        
        
    }
    
    private void InformHUDAboutNewConnectedObjects()
    {
        
        float voltage =  GetVoltageFromConnectedObjects();// calculate the potential difference between two touched objects
        float resistance = GetResistanceFromConnectedObjects(); //does what it says-omar
        float current = GetCurrentFromConnectedObjects(); //does what it say
        
        GameObject player_hud_multimeter = GameObject.Find("MeasuredVoltageText"); // get player HUD for the multimeter
        player_hud_multimeter.GetComponent<UpdateHUDtext>().RefreshInfo(PrettyPrintThisInfo(voltage,resistance,current));
        Debug.Log("U_Reaction = " + ReactionVoltage());// update its text
    }

    private string PrettyPrintThisInfo(float voltage, float resistance, float current)
    {
        string output = "Circuit\n----------------------------------\n";
        output += "Voltage: " + voltage + " V \n";
        output += "Resistance: " + resistance + "Ω \n";
        output += "Current: " + current + "A \n";

        return output;
    }

    private float GetVoltageFromConnectedObjects()
    {
        
        first_hit_object.collider.GetComponentInParent<ElectricalComponent>().Initialize();
        second_hit_object.collider.GetComponentInParent<ElectricalComponent>().Initialize();

        float v1 = first_hit_object.collider.GetComponentInParent<ElectricalComponent>().GetPotenial();
        float v2 = second_hit_object.collider.GetComponentInParent<ElectricalComponent>().GetPotenial();
       

        return v1- v2 ;
    }

    private float GetResistanceFromConnectedObjects()
    {
        //return first_hit_object.collider.GetComponentInChildren<ElectricalComponent>().GetResistance()
        //    + second_hit_object.collider.GetComponent<ElectricalComponent>().GetResistance();
        ResistanceSolver solver = new ResistanceSolver(connected_lines);
        return solver.LaunchResolveResistance();
    }

    
    public float GetCurrentFromConnectedObjects()
    {
        current = GetVoltageFromConnectedObjects() / GetResistanceFromConnectedObjects();
        Debug.Log("curr = "+current);
        return current;
    }

    //calculates Reaction voltage with ButlerVolmer TODO : make this work even if Battery is second_hit_object
    public float ReactionVoltage()
    {
        Debug.Log("curr2 = " + current);
        Debug.Log("error -1");
        ElectricalComponent electrolyte;
        float deltaU;
        Debug.Log("error 0");
        if (first_hit_object.collider.GetComponentInParent<ElectricalComponent>().GetComponentType() == 1)       //type 1 is Battery
        {
            Debug.Log("error 1");
            electrolyte = first_hit_object.collider.GetComponentInParent<ElectricalComponent>();
            Debug.Log("error 2");
        }
        else if (second_hit_object.collider.GetComponentInParent<ElectricalComponent>().GetComponentType() == 1)
        {
            electrolyte = second_hit_object.collider.GetComponentInParent<ElectricalComponent>();
        }
        else
        {
            return 0;
        }


        if (Mathf.Abs(current) < 1)
        { //approximation for small currents
            deltaU = (R * electrolyte.GetTemp()) / (electrolyte.GetZ() * F) * (current / electrolyte.GetArea() * electrolyte.GetJ());
        }
        else
        { //approximation for large currents
            float argument = current / (electrolyte.GetArea() * electrolyte.GetJ());
            deltaU = (((R * electrolyte.GetTemp()) / (electrolyte.GetAlpha() * electrolyte.GetZ() * F))) * Mathf.Log(argument, 2.71f);
        }

        float IButlerVolmer = electrolyte.GetArea() * electrolyte.GetJ() *
            (Mathf.Exp((electrolyte.GetAlpha() * electrolyte.GetZ() * F * deltaU) / (R * electrolyte.GetTemp()))
            - Mathf.Exp((-1) * ((1 - electrolyte.GetAlpha()) * electrolyte.GetZ() * F * deltaU)) / (R * electrolyte.GetTemp()));
        Debug.Log("Current = "+current);
        Debug.Log("deltaU = " +deltaU);
        Debug.Log("IButlerVolmer = " + IButlerVolmer);
        return (deltaU /IButlerVolmer) * current;

    }
}
