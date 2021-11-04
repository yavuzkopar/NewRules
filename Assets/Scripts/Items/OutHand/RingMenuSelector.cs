using UnityEngine;
using RPG.Core;
using System.Collections.Generic;
using UnityEngine.UI;
using RPG.Dialogue;

public class RingMenuSelector : MonoBehaviour {
    public List<GameObject> objects = new List<GameObject>();
        public GameObject prefabObject;
        public Transform parentObject;
        RingMenu ringMenu;
        RaycastObject raycastObject;
    private Vector2 normalizedMousePos;
    public float currentAngle;
    public int selection;
    private int prviousSelection;
     public delegate void ActionToUse();
        public List<ActionToUse> actions = new List<ActionToUse>();

        bool isSelectionActive;
        PlayerConversant PlayerConversant;

    private void Start() {
            raycastObject = GameObject.FindGameObjectWithTag("lokk").GetComponent<RaycastObject>();
            PlayerConversant = GetComponent<PlayerConversant>();
        }
        private void Update() 
        {
            if (Input.GetMouseButtonDown(1))
            {
                parentObject.gameObject.SetActive(true);
                parentObject.position = Input.mousePosition;
                    MenuDeneme();
                Time.timeScale = 0;
            }
            if (Input.GetMouseButtonUp(1))
            {
              Time.timeScale = 1;
                parentObject.gameObject.SetActive(false);  
                if(isSelectionActive)
                {
                    actions[selection](); 
                    isSelectionActive = false;
                }
                 objects.Clear();
                for (int i = 0; i < parentObject.childCount; i++)
                {
                    Destroy(parentObject.GetChild(i).gameObject);
                } 
            }
            if(objects.Count == 0) return;

             normalizedMousePos = new Vector2(Input.mousePosition.x - parentObject.position.x, Input.mousePosition.y - parentObject.position.y);
            currentAngle = Mathf.Atan2(normalizedMousePos.y, normalizedMousePos.x) * Mathf.Rad2Deg;
            currentAngle = (currentAngle + 360) % 360;
            selection = (int)currentAngle / (360 / objects.Count);
            if (/*selection != prviousSelection &&*/ parentObject.childCount != 0)
            {
              selection = Mathf.Clamp(selection,0,parentObject.childCount - 1);
          
               objects[prviousSelection].GetComponent<Image>().color = Color.red;
               prviousSelection = selection;
                   if(normalizedMousePos.magnitude >80f && normalizedMousePos.magnitude < 200f)
              {
                objects[selection].GetComponent<Image>().color = Color.green;
                isSelectionActive = true;
              }
              else
              {
                  isSelectionActive=false;
              }
            
            }  
        }
        
        void MenuDeneme()
    {
        prviousSelection = 0;
        if (raycastObject.gameObjectt.GetComponent<RingMenu>() == null) return;
        ringMenu = raycastObject.gameObjectt.GetComponent<RingMenu>();
        objects.Clear();
        for (int i = 0; i < parentObject.childCount; i++)
        {
            Destroy(parentObject.GetChild(i).gameObject);
        }
        for (int i = 0; i < ringMenu.items.Length; i++)
        {
            GameObject obj = Instantiate(prefabObject, parentObject);
            objects.Add(obj);
            objects[i].GetComponent<Image>().color = Color.red;
            objects[i].GetComponent<Image>().sprite = ringMenu.items[i].icon;
            obj.transform.rotation = Quaternion.Euler(0, 0, 360 / (ringMenu.items.Length * 2));
            obj.transform.Rotate(Vector3.forward, i * (360 / ringMenu.items.Length));
        }
        GetActions();
    }

    private void GetActions()
    {
        foreach (var item in ringMenu.items)
        {
            switch (item.name)
            {
                case "Talk":
                    actions.Add(Talk);
                    break;
                case "PickPocket":
                    actions.Add(PickPocket);
                    break;
                case "Take":
                    actions.Add(Take);
                    break;
                case "Sit":
                    actions.Add(Sit);
                    break;

                default:
                    break;
            }
        }
    }
#region ActionList
    void Talk()
        {
            Debug.Log("Talk"); // konusma aksiyonu
            raycastObject.gameObjectt.GetComponent<AIConversant>().StartDialogue(PlayerConversant);
        }
        void PickPocket()
        {
            Debug.Log("Pickpocket"); // konusma aksiyonu
        }
        void Take()
        {
            Debug.Log("Take"); // konusma aksiyonu
        }
        void Sit()
        {
            Debug.Log("Sit"); // konusma aksiyonu
        }
        void Open()
        {
            Debug.Log("Open"); // konusma aksiyonu
        }
#endregion
}