using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core{
    public class MeshRenderControl : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Wallvanisher") && other.gameObject.layer == LayerMask.NameToLayer("CanInteract"))
        {
            other.gameObject.layer = LayerMask.NameToLayer("Invisible Wall");
        }

        //  other.gameObject.GetComponent<Collider>().enabled = false;
        //Debug.Log("girdiii");
        //bo = other.gameObject;


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wallvanisher") && other.gameObject.layer == LayerMask.NameToLayer("Invisible Wall"))
        {
            other.gameObject.layer = LayerMask.NameToLayer("CanInteract");

        }

    }
}

}