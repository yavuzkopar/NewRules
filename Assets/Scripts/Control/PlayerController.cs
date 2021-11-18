
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

using RPG.Core;


namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] float speed;
        public float rotationSpeed;
        Vector3 inputVector;
        Health health;
       public GameObject equipedWeapon;
        [SerializeField] Transform lookPoint;
        public Transform rightHand;
        public bool isRunning = false;
        public Animator animator;
        Vector3 direction;
        RaycastObject raycastObject;
        private ScriptibleStats stats;
        public Transform followCam;

        Vector3 vectorrr;

        void Start() {
            health = GetComponent<Health>();
            animator = GetComponent<Animator>();
            raycastObject = lookPoint.gameObject.GetComponent<RaycastObject>();
            stats = GetComponent<BaseStats>().Stats;
        }

        // Update is called once per frame
        void Update()
        {            
            if (health.IsDead())
            {

                return;
            }
            
            if (MoveTo()) return;
        }
        public bool MoveTo()
        {
            Vector3 bisey = followCam.forward;
            Vector3 bisey2 = followCam.right;
            
          //  Debug.Log("bisey " + bisey);
         //   Debug.Log("bisey2 " + bisey2);
       
            inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (Input.GetKey(KeyCode.W))
            {
              //  Vector3 bisey = followCam.forward;
                transform.position += followCam.forward.normalized * speed * Time.deltaTime;
                vectorrr = followCam.forward;
            }  
            if (Input.GetKey(KeyCode.S))
            {
            //    Vector3 bisey = followCam.forward;
                transform.position += -followCam.forward.normalized * speed * Time.deltaTime;
                vectorrr = -followCam.forward;
            }   
            if (Input.GetKey(KeyCode.A))
            {
             //   Vector3 bisey = followCam.forward;
                transform.position += -followCam.right.normalized * speed * Time.deltaTime;
                vectorrr = -followCam.right;
            }   
            if (Input.GetKey(KeyCode.D))
            {
              //  Vector3 bisey = followCam.forward;
                transform.position += followCam.right.normalized * speed * Time.deltaTime;
                vectorrr = followCam.right.normalized;
            }   
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
            {
                vectorrr = Vector3.zero;
            } 
            direction = new Vector3(lookPoint.position.x, transform.position.y, lookPoint.position.z);
            Vector3 dir = direction - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir.normalized);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            if (inputVector.magnitude > 0.2)
            {
               // transform.position += inputVector.normalized * speed * Time.deltaTime;
              
                 
                UpdateAnimator();
                return true;
            }
            return false;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity;
            velocity = /*GetComponent<NavMeshAgent>().velocity; */inputVector;
         // velocity = followCam.forward;
         Vector3 dir = direction - transform.position;
            Vector3 localVelocity = transform.InverseTransformDirection(vectorrr);
            float fspeedZ = localVelocity.z;
            float fspeedX = localVelocity.x;
            Debug.Log(vectorrr);
            animator.SetFloat("V", fspeedZ);
            animator.SetFloat("H", fspeedX);

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isRunning = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
                isRunning = false;

            if (isRunning && fspeedZ>.9f)
            {
                speed =10f;
                animator.ResetTrigger("isWalking");
                animator.SetTrigger("isRunning");
            }
            else
            {
                speed = 3f;
                animator.ResetTrigger("isRunning");
                animator.SetTrigger("isWalking");
            }

        }
        void Hit()
        {
            //Animasyon
            float dist;
            if (RaycastObject.combatTarget != null)
            {
               dist  = Vector3.Distance(transform.position, RaycastObject.combatTarget.transform.position);
                if (dist <= 5f)
                {
                    RaycastObject.combatTarget.TakeDamage(10f);
                }
            }
            if (Vector3.Distance(raycastObject.interactableObj.transform.position,transform.position)<4f)
            {
                if (raycastObject.interactableObj.GetComponent<Enteractible>())
                {
                    raycastObject.interactableObj.transform.parent = rightHand;
                    raycastObject.interactableObj.transform.localPosition = Vector3.zero;
                   
                    raycastObject.interactableObj.layer = LayerMask.NameToLayer("Default");
                }
            }
        }
    }
}

