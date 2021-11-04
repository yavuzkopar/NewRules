using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.AI
{
    public class State
    {
        public enum STATE
        {
            IDLE, PATROL, PURSUE, ATTACK, WALK, DEAD, TALKING
        };
        public enum EVENT
        {
            ENTER, UPDATE, EXIT
        };

        public STATE name;
        protected EVENT stage;
        protected GameObject npc;
        protected Animator anim;
        protected Transform player;
        protected State nextState;
        protected NavMeshAgent agent;

        public float visDist = 15f;
        public float visAngle = 10f;
        public float attackDist = 5f;
        protected float unutmaSuresi;

        public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        {

            npc = _npc;
            agent = _agent;
            anim = _anim;
            stage = EVENT.ENTER;
            player = _player;
        }
        public virtual void Enter() { stage = EVENT.UPDATE; }
        public virtual void Update() { stage = EVENT.UPDATE; }
        public virtual void Exit() { stage = EVENT.EXIT; }

        public State Progress()
        {
            if (stage == EVENT.ENTER) Enter();
            if (stage == EVENT.UPDATE) Update();
            if (stage == EVENT.EXIT)
            {
                Exit();
                return nextState;
            }
            return this;

        }

        public bool CanSeePlayer()
        {
            visAngle = npc.GetComponent<AI>().visAngle;
            Vector3 direction = player.position - npc.transform.position;
            float angle = Vector3.Angle(direction, npc.transform.forward);
            if (direction.magnitude < visDist && angle < visAngle)
            {
                unutmaSuresi = 0f;
                return true;
            }
            return false;
        }
        public bool CanAttackPlayer()
        {
            Vector3 direction = player.position - npc.transform.position;
            if (direction.magnitude < attackDist && !player.gameObject.GetComponent<Health>().IsDead())
            {
                return true;
            }
            return false;
        }

    }
    public class Idle : State
    {
        public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
            : base(_npc, _agent, _anim, _player)
        {
            name = STATE.IDLE;
        }
        public override void Enter()
        {

            anim.SetTrigger("isIdle");
            npc.GetComponent<AI>().material.color = Color.white;
            base.Enter();
        }
        public override void Update()
        {
            if (npc.GetComponent<Health>().IsDead())
            {
                nextState = new Dead(npc, agent, anim, player);
                stage = EVENT.EXIT;
                return;
            }

            if (CanSeePlayer() && !player.gameObject.GetComponent<Health>().IsDead() && npc.CompareTag("Gangster"))
            {
                nextState = new Pursue(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
            else if (Random.Range(0, 100) < 10 && npc.CompareTag("Gangster"))
            {
                nextState = new Patrol(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
            else if (npc.CompareTag("Civil"))
            {
                nextState = new Walk(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }


        }
        public override void Exit()
        {
            anim.ResetTrigger("isIdle");
            base.Exit();
        }
    }
    public class Dead : State
    {
        float time = 5f;
        public Dead(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
            : base(_npc, _agent, _anim, _player)
        {
            name = STATE.DEAD;
            agent.speed = 0f;
            agent.isStopped = true;
        }
        public override void Enter()
        {


            //  anim.SetTrigger("die");
            //  anim.ResetTrigger("isIdle");
            npc.GetComponent<AI>().material.color = Color.white;

            base.Enter();
        }
        public override void Update()
        {
            //
            time -= Time.deltaTime;
            if (time <= 0)
            {
                npc.SetActive(false);
                stage = EVENT.EXIT;
            }


        }
        public override void Exit()
        {
            anim.ResetTrigger("die");
            base.Exit();
        }
    }
    public class Walk : State
    {

        public Walk(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
            : base(_npc, _agent, _anim, _player)
        {
            name = STATE.WALK;
            agent.speed = 2f;
            agent.isStopped = false;
        }
        public override void Enter()
        {


            anim.SetTrigger("isWalking");
            npc.GetComponent<AI>().material.color = Color.blue;
            agent.SetDestination(GameEnvironment.Singleton.Goalpoints[Random.Range(0, GameEnvironment.Singleton.Goalpoints.Count)].transform.position);

            base.Enter();
        }
        public override void Update()
        {
            if (npc.GetComponent<Health>().IsDead())
            {
                nextState = new Idle(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
            if (agent.remainingDistance < 1)
            {

                agent.SetDestination(GameEnvironment.Singleton.Goalpoints[Random.Range(0, GameEnvironment.Singleton.Goalpoints.Count)].transform.position);
            }


        }
        public override void Exit()
        {
            anim.ResetTrigger("isWalking");
            base.Exit();
        }
    }
    public class Patrol : State
    {
        int currentIndex = -1;
        public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
            : base(_npc, _agent, _anim, _player)
        {
            name = STATE.PATROL;
            agent.speed = 2f;
            agent.isStopped = false;
        }
        public override void Enter()
        {

            currentIndex = 0;
            anim.SetTrigger("isWalking");
            npc.GetComponent<AI>().material.color = Color.yellow;
            base.Enter();
        }
        public override void Update()
        {
            if (npc.GetComponent<Health>().IsDead())
            {
                nextState = new Idle(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
            if (agent.remainingDistance < 1)
            {
                if (currentIndex >= GameEnvironment.Singleton.Checkpoints.Count - 1)
                    currentIndex = 0;
                else
                    currentIndex++;

                agent.SetDestination(GameEnvironment.Singleton.Checkpoints[currentIndex].transform.position);
            }
            if (CanSeePlayer() && !player.gameObject.GetComponent<Health>().IsDead())
            {
                //  nextState = new Pursue(npc, agent, anim, player);
                nextState = new Idle(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }

        }
        public override void Exit()
        {
            anim.ResetTrigger("isWalking");
            base.Exit();
        }
    }
    public class Pursue : State
    {

        public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
            : base(_npc, _agent, _anim, _player)
        {
            name = STATE.PURSUE;
            agent.speed = 5f;
            agent.isStopped = false;
        }
        public override void Enter()
        {


            anim.SetTrigger("isRunning");
            npc.GetComponent<AI>().material.color = Color.red;
            base.Enter();
        }
        public override void Update()
        {
            agent.SetDestination(player.position);
            if (agent.hasPath)
            {
                if (CanAttackPlayer())
                {
                    nextState = new Attack(npc, agent, anim, player);
                    stage = EVENT.EXIT;
                }
                else if (!CanSeePlayer())
                {
                    nextState = new Patrol(npc, agent, anim, player);
                    stage = EVENT.EXIT;
                }
            }

        }
        public override void Exit()
        {
            anim.ResetTrigger("isRunning");
            base.Exit();
        }
    }
    public class Attack : State
    {
        float rotationSpeed = 100f;

        public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
            : base(_npc, _agent, _anim, _player)
        {
            name = STATE.ATTACK;
        }
        public override void Enter()
        {

            anim.SetTrigger("isAttacking");
            npc.GetComponent<AI>().material.color = Color.black;
            agent.isStopped = true;

            base.Enter();
        }
        public override void Update()
        {

            Vector3 direction = player.position - npc.transform.position;
            float angle = Vector3.Angle(direction, npc.transform.forward);
            //  direction.y = 0;
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
                                            Quaternion.LookRotation(direction),
                                            Time.deltaTime * rotationSpeed);
            //  npc.GetComponent<AI>().Hit();
            if (!CanAttackPlayer())
            {
                nextState = new Idle(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }

        }
        public override void Exit()
        {
            anim.ResetTrigger("isAttacking");
            base.Exit();
        }
    }
}