
using UnityEngine;

using UnityEngine.UI;


namespace RPG.Core
{
    public class RaycastObject : MonoBehaviour
    {
        // Start is called before the first frame update


   
     
        public static GameObject gameObjectt;

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
        }
        private void OnTriggerEnter(Collider other)
        {
            
                gameObjectt = other.gameObject;
        }
        private void OnTriggerStay(Collider other)
        {       if(gameObjectt== null)
                    gameObjectt = other.gameObject;

          
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