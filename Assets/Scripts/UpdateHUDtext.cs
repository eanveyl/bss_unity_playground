using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UpdateHUDtext : MonoBehaviour
{
    public Text info_HUD;

    // Start is called before the first frame update
    void Start()
    {
        info_HUD = GetComponent<Text>();    
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //Press the space key to change the Text message
        if (Input.GetKey(KeyCode.Space))
        {
            info_HUD.text = "My text has now changed.";
        }
        */
    }

    public void RefreshInfo(string display_this_text) {
        info_HUD.text = display_this_text;
    }
}
