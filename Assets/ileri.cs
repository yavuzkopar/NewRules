using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ileri : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject lokk = GameObject.FindGameObjectWithTag("lokk");
            transform.LookAt(lokk.transform.position + new Vector3(0,0,.3f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime *100f);
    }
}
