using UnityEngine;
using RPG.Core;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using RPG.Dialogue;
using RPG.Control;

public class RingMenuSelector : MonoBehaviour {

    public TextMeshProUGUI isimtext;
    public TextMeshProUGUI effectRangeText;

    public Transform isimci;
    public List<GameObject> objects = new List<GameObject>();
        public GameObject prefabObject;
        public Transform parentObject;
        OutHandOptions ringMenu;
    
    private Vector2 normalizedMousePos;
    public float currentAngle;
    public float effRange;
    public float rangeToitem;
    public int selection;
    private int prviousSelection;
     public delegate void ActionToUse();
        public List<ActionToUse> actions = new List<ActionToUse>();

        bool isSelectionActive;
        PlayerConversant PlayerConversant;
    public Color posColor;
    public Color negColor;    

    private void Start() {
            
            PlayerConversant = GetComponent<PlayerConversant>();
        }
        private void Update() 
        {
            if (Input.GetMouseButtonDown(1))
            { 
                parentObject.gameObject.SetActive(true);
                Debug.Log(parentObject.transform.position);
               
                parentObject.position = new Vector2(Mathf.Clamp(Input.mousePosition.x,300,1620),Mathf.Clamp(Input.mousePosition.y,300,780));
                effectRangeText.text =RaycastObject.gameObjectt.name + "\n" + "N/A";
                effectRangeText.color=Color.black;
                
                isimci.gameObject.SetActive(true);
                isimci.transform.position = parentObject.transform.position;
                    MenuDeneme();
                Time.timeScale = 0;
            }
            if (Input.GetMouseButtonUp(1))
            {
                
              Time.timeScale = 1;
                parentObject.gameObject.SetActive(false);  
                isimci.gameObject.SetActive(false);
                isimtext.text = "Nothing";
                
                
                if(isSelectionActive)
                {
                    actions[selection](); 
                    isSelectionActive = false;
                    
                }
                 objects.Clear();
                 actions.Clear();
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
               rangeToitem = Vector3.Distance(transform.position,RaycastObject.gameObjectt.transform.position);
                   if(normalizedMousePos.magnitude >150f && normalizedMousePos.magnitude < 350f)
              {
                objects[selection].GetComponent<Image>().color = Color.green;
              //  isimtext.text = ringMenu.items[selection].name;
                effRange = ringMenu.items[selection].effectRange;
                effectRangeText.text = RaycastObject.gameObjectt.name + "\n" + 
                ringMenu.items[selection].name + "\n" +"Effect Range: " + 
                effRange.ToString() + "\n" +"Range: " + 
                rangeToitem.ToString("F2");
                if (rangeToitem <= effRange)
                {
                    effectRangeText.color = posColor;
                }
                else
                    effectRangeText.color = negColor;
                     
                isSelectionActive = true;
              }
              else
              {
                  isSelectionActive=false;
                //  isimtext.text = "Nothing";
                  effectRangeText.text =RaycastObject.gameObjectt.name;
                  effectRangeText.color = Color.black;
                  
              }
            
            }  
        }
        
        void MenuDeneme()
    {
        prviousSelection = 0;
        if (RaycastObject.gameObjectt.GetComponent<OutHandOptions>() == null) return;
        ringMenu = RaycastObject.gameObjectt.GetComponent<OutHandOptions>();
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
             if(rangeToitem > effRange)
                return;
            Debug.Log("Talk"); // konusma aksiyonu
            RaycastObject.gameObjectt.GetComponent<AIConversant>().StartDialogue(PlayerConversant);
        }
        void PickPocket()
        {
            GameObject obj = RaycastObject.gameObjectt;
            Destroy(obj);
            Debug.Log("Pickpocket"); // konusma aksiyonu
        }
        void Take()
        {
            
            PlayerController player = GetComponent<PlayerController>();
            if(rangeToitem > effRange || player.equipedWeapon != null)
                return;
            GameObject obj = RaycastObject.gameObjectt;   
            obj.transform.parent = player.rightHand;
            player.equipedWeapon = obj;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localEulerAngles = Vector3.zero;
            if(player.equipedWeapon.GetComponent<Rigidbody>() != null){
                player.equipedWeapon.GetComponent<Rigidbody>().useGravity =false;
                player.equipedWeapon.GetComponent<Rigidbody>().isKinematic=true;
            }
            GetComponent<ActionSelection>().WeaponDeneme();
        }
        void Sit()
        {
            if(rangeToitem > effRange)
                return;
                GetComponent<Animator>().ResetTrigger("isWalking");
            GetComponent<Animator>().SetTrigger("sitt");
            GetComponent<PlayerController>().isMoving = false;
            Vector3 sitpos = RaycastObject.gameObjectt.transform.position;
            transform.position = RaycastObject.gameObjectt.transform.position; 
            transform.rotation = RaycastObject.gameObjectt.transform.rotation;
            Debug.Log("Sit"); // konusma aksiyonu
        }
        void Open()
        {
            Debug.Log("Open"); // konusma aksiyonu
        }
#endregion
}