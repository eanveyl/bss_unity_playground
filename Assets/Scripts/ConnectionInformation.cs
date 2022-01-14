using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionInformation : MonoBehaviour
{
    private RaycastHit first_object;
    private RaycastHit second_object;

    public bool has_same_parent;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //broken function, consider removing
    public void InitializeConnections(RaycastHit first, RaycastHit second) {
        first_object = first;
        second_object = second;      
    }

    public RaycastHit GetFirstHit() {
        return first_object;
    }

    public RaycastHit GetSecondHit() {
        return second_object;
    }
}
