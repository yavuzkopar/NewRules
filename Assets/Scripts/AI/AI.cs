using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace RPG.AI
{
    public class AI : MonoBehaviour
    {
        NavMeshAgent agent;
        Animator anim;
        Transform player;
        State currentState;
        [SerializeField] GameObject sph;
        public Material material;
        Health health;
        [SerializeField] float visDist;
        [SerializeField] Text nameText;
        public float visAngle;
        void Start()
        {

            agent = this.GetComponent<NavMeshAgent>();
            anim = this.GetComponent<Animator>();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            currentState = new Idle(this.gameObject, agent, anim, player);
            material = sph.GetComponent<MeshRenderer>().material;
            health = GetComponent<Health>();
            nameText.text = gameObject.name;
            nameText.transform.rotation = Camera.main.transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {

            
            currentState = currentState.Progress();
            Debug.Log(currentState);

        }
        public void Hit()
        {
            player.gameObject.GetComponent<Health>().TakeDamage(5);
            Debug.Log("Vurduuu");
        }

    }

}