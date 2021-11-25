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

    // Start is called before the first frame update
    void Start()
    {
        
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
    private void FixedUpdate() {
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

    private void ShootPrimaryRay() {
        Vector3 fwd = Camera.main.transform.forward;//transform.TransformDirection(Vector3.forward);

        
        Debug.DrawRay(transform.position, fwd * 10, Color.red, 5);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, fwd, out hit, (float) 10)) {
            Debug.Log("There is something in front of the object!");
            //Debug.Log(hit.collider.GetComponent<Rigidbody>().transform.position);
            Material hit_material = hit.collider.GetComponent<Renderer>().material;
            first_hit_object = hit; //save for future reference
            hit_material.color = Color.magenta;
            first_hit_object_position = hit.transform.position;
            Debug.Log("First Ray hit:");
            Debug.Log(first_hit_object_position);

        } else {
            Debug.Log("Nothing hit!");
        }
        
        /* //also works fine!!
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100)) {
            Debug.DrawLine(ray.origin, hit.point);
        }
        */
    }

    private void ShootSecondaryRay() {
        Vector3 fwd = Camera.main.transform.forward;//transform.TransformDirection(Vector3.forward);

        
        Debug.DrawRay(transform.position, fwd * 10, Color.blue, 5);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, fwd, out hit, (float) 10)) {
            Debug.Log("There is something in front of the object!");
            //Debug.Log(hit.collider.GetComponent<Rigidbody>().transform.position);
            Material hit_material = hit.collider.GetComponent<Renderer>().material;
            second_hit_object = hit; //save for future reference
            hit_material.color = Color.magenta;
            second_hit_object_position = hit.transform.position;
            Debug.Log("Second Ray hit:");
            Debug.Log(second_hit_object_position);

        } else {
            Debug.Log("Nothing hit!");
        }
        
        /* //also works fine!!
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100)) {
            Debug.DrawLine(ray.origin, hit.point);
        }
        */
    }

    private void ConnectAndDrawLineFirst2SecondObj() {
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
        
        
        if (first_hit_object.collider.GetComponent<ElectricalProperties>() != null && second_hit_object.collider.GetComponent<ElectricalProperties>() != null) { // only if both objects have electrical properties
            Debug.Log("Both objects have electrical properties. A connection is valid.");
            myLine.AddComponent<ConnectionInformation>();
            myLine.GetComponent<ConnectionInformation>().InitializeConnections(first_hit_object, second_hit_object); // attach the object information to the line to be able to retrieve it later
            
            connected_lines.Add(myLine); // add the current line to the connected lines 

            InformHUDAboutNewConnectedObjects();
        } else { // in the case user tries to connect objects without electrical properties
            Debug.LogWarning("One or more objects do not have electrical properties. A connection is invalid. Deleting in 3 seconds.");
            GameObject.Destroy(myLine, 3f); //10 seconds duration
        }
        
        
    }
    private void InformHUDAboutNewConnectedObjects() {
        string bat_info = first_hit_object.collider.GetComponent<ElectricalProperties>().PrettyPrintDataSheet();  // battery information will be printed from the first object that was touched. 
        float voltage =  GetVoltageFromConnectedObjects();// calculate the potential difference between two touched objects
        Debug.Log(bat_info);

        GameObject player_hud_battery_info = GameObject.Find("BatteryInformationText"); // get player HUD for battery info
        player_hud_battery_info.GetComponent<UpdateHUDtext>().RefreshInfo(bat_info); // update its text

        GameObject player_hud_multimeter = GameObject.Find("MeasuredVoltageText"); // get player HUD for the multimeter
        player_hud_multimeter.GetComponent<UpdateHUDtext>().RefreshInfo(PrettyPrintThisVoltage(voltage)); // update its text
    }

    private string PrettyPrintThisVoltage(float v) {
        string output = "Voltage Information\n----------------------------------\n";
        output += v + " V \n";
        
        return output;
    }

    private float GetVoltageFromConnectedObjects() {
        return first_hit_object.collider.GetComponent<ElectricalProperties>().GetPotential() - second_hit_object.collider.GetComponent<ElectricalProperties>().GetPotential();
    }
}
