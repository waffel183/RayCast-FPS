using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {
    
	void Start () {
        Connect();
	}

    void Connect()
    {
        //connect to network
        PhotonNetwork.ConnectUsingSettings("v1.0.0");
    }

    void OnGUI()
    {
        //creates GUI to view network
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
