
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
            inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            direction = new Vector3(lookPoint.position.x, transform.position.y, lookPoint.position.z);
            Vector3 dir = direction - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir.normalized);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            if (inputVector.magnitude > 0)
            {
                transform.position += inputVector.normalized * speed * Time.deltaTime;
                UpdateAnimator();
                return true;
            }
            return false;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity;
            velocity = /*GetComponent<NavMeshAgent>().velocity; */inputVector;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float fspeedZ = localVelocity.z;
            float fspeedX = localVelocity.x;

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

