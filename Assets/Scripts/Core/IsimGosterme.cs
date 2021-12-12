using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsimGosterme : MonoBehaviour
{
    [SerializeField]Text isimtext;
    Transform camm;
    void Start()
    {
        isimtext.text = this.gameObject.name;
        camm = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        isimtext.transform.forward=camm.forward;
    }
}
