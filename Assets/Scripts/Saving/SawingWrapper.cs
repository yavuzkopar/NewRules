using UnityEngine;

public class SawingWrapper : MonoBehaviour {
    const string defaultSaveFile = "save";

    private void Update() {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
         if (Input.GetKeyDown(KeyCode.L))
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
            
        }
    }
}