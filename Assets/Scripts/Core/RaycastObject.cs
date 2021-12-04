
using UnityEngine;

using UnityEngine.UI;


namespace RPG.Core
{
    public class RaycastObject : MonoBehaviour
    {
        // Start is called before the first frame update


        public enum ActionState
        {
            GANSTER, WALK, SOMETHING, NULL, TALK
        }
        public ActionState action;
        public static Health combatTarget = null;
        public GameObject interactableObj;
        public GameObject player;
        public Text itemNametext;
        public GameObject gameObjectt;

        // Update is called once per frame
        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            //  bool hasHit = Physics.Raycast(ray, out hit,8000f,(1<<7));
            bool hasHit = Physics.SphereCast(ray, 0.1f, out hit, 500f, (1 << 7));


            if (hasHit)
            {
                transform.position = hit.point;

            }
            if (combatTarget == null) return;
          //  Debug.Log(combatTarget.name);
            //if (combatTarget.gameObject.GetComponent<AIConversant>() != null)
            //{
            //    if (Input.GetMouseButtonDown(0))
            //    {

            //        player.GetComponent<PlayerConversant>().StartDialogue(combatTarget.GetComponent<AIConversant>(), combatTarget.GetComponent<AIConversant>().dialogue); // daha optimize yol bulunacak
            //    }
            //}


        }
        private void OnTriggerEnter(Collider other)
        {
            
                gameObjectt = other.gameObject;
        }
        private void OnTriggerStay(Collider other)
        {       if(gameObjectt== null)
                    gameObjectt = other.gameObject;

            if (itemNametext != null)
            {
                itemNametext.text = gameObjectt.name;
            }

            if (other.gameObject.GetComponent<Health>() != null)
            {
                combatTarget = other.gameObject.GetComponent<Health>();
                action = ActionState.TALK;
                 //  Debug.Log(combatTarget.name);
            }
            else
                interactableObj = other.gameObject;
            if (other.gameObject.CompareTag("Gangster"))
            {
                action = ActionState.GANSTER;

                //  Debug.Log("Gangster");
            }
            else if (other.gameObject.CompareTag("Civil") || other.gameObject.CompareTag("Ganster"))
            {
                action = ActionState.WALK;
                //   Debug.Log("civil");


            }
            else
            {

                action = ActionState.NULL;
                //  Debug.Log("nullloooo");
                combatTarget = null;
            }
        }
        private void OnTriggerExit(Collider other)
        {

            // gameObjectt = other.gameObject;
            if (other.gameObject == gameObjectt)
            {
                gameObjectt = null;
            }
        }

    }

}