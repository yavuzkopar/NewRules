using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Core;


namespace RPG.Control
{
    public class ActionSelection : MonoBehaviour
    {
        public List<GameObject> objects = new List<GameObject>();
        public GameObject prefabObject;
        [SerializeField] GameObject atilacak;
        [SerializeField] Transform atisyeri;
        public Transform parentObject;
        public int selectedAction = 0;
        public delegate void ActionToUse();
        public List<ActionToUse> actions = new List<ActionToUse>();
        private PlayerController playerController;
        public InHandActions allActions;

        public KeyCode keyCode;
        void Start()
        {
            playerController = GetComponent<PlayerController>();
          //  WeaponDeneme();

            for (int i = 0; i < objects.Count; i++)
            {
                if (selectedAction == i)
                {
                    objects[selectedAction].GetComponent<Image>().color = Color.green;
                }
                else
                {
                    objects[i].GetComponent<Image>().color = Color.red;
                }
                objects[i].GetComponent<Image>().sprite = allActions.handActions[i].icon;
            }
        }

        public void WeaponDeneme()
        {
            if (playerController.equipedWeapon == null)
            {
                objects.Clear();
                 for (int i = 0; i < parentObject.childCount; i++)
            {
                Destroy(parentObject.GetChild(i).gameObject);
            }
            actions.Clear();
            return;
            }
            allActions = playerController.equipedWeapon.GetComponent<InHandActions>();
            objects.Clear();
            for (int i = 0; i < parentObject.childCount; i++)
            {
                Destroy(parentObject.GetChild(i).gameObject);
            }
            for (int i = 0; i < allActions.handActions.Length; i++)
            {
                GameObject obj = Instantiate(prefabObject, parentObject);
                objects.Add(obj);
                objects[i].GetComponent<Image>().sprite = allActions.handActions[i].icon;
            }

            GetActions();
            selectedAction = 0;
            ColorSelection();
        }

        private void GetActions()
        {
            foreach (var item in allActions.handActions)
            {
                switch (item.actionName)
                {
                    case "Attack":
                        actions.Add(Throw);
                        break;
                    case "Savun":
                        actions.Add(Savun);
                        break;
                    case "Sapla":
                        actions.Add(Sapla);
                        break;

                    case "Savur":
                        actions.Add(Savur);
                        break;
                    case "DropItem":
                        actions.Add(DropItem);
                        break;    

                    default:
                        break;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                selectedAction = 0;
                ColorSelection();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                selectedAction = 1;
                ColorSelection();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                selectedAction = 2;
                ColorSelection();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                selectedAction = 3;
                ColorSelection();
            }
            if (Input.GetMouseButtonDown(0))
            {
                if(actions.Count >=1)
                    actions[selectedAction]();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                WeaponDeneme();
            }
        }

        void ColorSelection()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (selectedAction == i)
                {
                    objects[selectedAction].GetComponent<Image>().color = Color.green;
                }
                else
                {
                    objects[i].GetComponent<Image>().color = Color.red;
                }
            }
        }
#region ActionList
        void Savun()
        {
            Debug.Log("Savun");
        }
        void Sapla()
        {
           GameObject obj = Instantiate(atilacak);
           obj.transform.position= atisyeri.position;
           

        }
        void DropItem()
        {
            
            GameObject raycastObject = GameObject.FindGameObjectWithTag("lokk");
            GameObject obj =playerController.equipedWeapon;
            playerController.equipedWeapon = null;
            obj.transform.position = raycastObject.transform.position;
            obj.transform.rotation = Quaternion.identity;
            obj.transform.SetParent(null);
            
            Debug.Log("DropItem");
            WeaponDeneme();
        }
        void Savur()
        {
            playerController.equipedWeapon.GetComponent<Rigidbody>().isKinematic = false;
            playerController.equipedWeapon.GetComponent<Rigidbody>().AddForce((transform.forward + Vector3.up *.5f) * 600);
             playerController.equipedWeapon.GetComponent<Rigidbody>().useGravity = true;
            playerController.equipedWeapon.transform.SetParent(null);
            
            playerController.equipedWeapon = null;
            WeaponDeneme();
            Debug.Log("Savur");
        }
        void Throw()
        {
            
            playerController.animator.SetTrigger("attack");
        }
        void Read()
        {
            Debug.Log("3. aksiyon");
        }
        void Bagla()
        {
            Debug.Log("3. aksiyon");
        }
        void BagiCoz()
        {
            Debug.Log("3. aksiyon");
        }
        void OkAt()
        {
            Debug.Log("3. aksiyon");
        }
        void Kullan()
        {
            Debug.Log("3. aksiyon");
        }
        void PlayEnstruman()
        {
            Debug.Log("3. aksiyon");
        }
#endregion
    }

}