using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardBehaviour : MonoBehaviour
{
    BehaviourTree tree;
    NavMeshAgent agent;
    public Transform[] checkpoints;
    int currentPoint = 0;

    public float visAngle;
    public float visDist;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
       

        tree = new BehaviourTree();
        Selector basSelector = new Selector("Bas Selector");
        Sequence wander = new Sequence("Wander");
        Sequence attack = new Sequence("Attack");
        Leaf goToCheckpoint = new Leaf("Go To Checkpoint",GoToCheckpoint);
        Leaf canSeePlayer = new Leaf("Can See Player",CanSeePlayer);
        Leaf goToPlayer = new Leaf("Go To Player",GoToPlayer);
        Leaf dur = new Leaf("Dur",Dur);

        attack.AddChild(canSeePlayer);
        wander.AddChild(goToCheckpoint);

        attack.AddChild(goToPlayer);
        attack.AddChild(dur);
        basSelector.AddChild(attack);
        basSelector.AddChild(wander);
        
        tree.AddChild(basSelector);
      
        tree.PrintTree();
    }

    // Update is called once per frame
    void Update()
    {
        tree.Process();
      
    }
    bool CanSee()
    {
         Vector3 direction = GameWorld.player.transform.position - this.transform.position;
        float angle = Vector3.Angle(direction,this.transform.forward);
        if(direction.magnitude <= visDist && angle <= visAngle)
            return true;
        return false;
    }
    Node.Status GoToCheckpoint()
    {
        float distance = Vector3.Distance(checkpoints[currentPoint].position,this.transform.position);
     /*   if (state == ActionState.IDLE)
        {
         
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }
        else if (Vector3.Distance(agent.pathEndPosition,destination) >=2)
        {
           
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }
        else if (distance <2)
        {
        
            state = ActionState.IDLE;
            return Node.Status.SUCCESS;
        }
        return Node.Status.RUNNING;*/
        agent.speed = 5f;
        agent.SetDestination(checkpoints[currentPoint].position);
        
        if (CanSee())
        {
           // currentPoint = 0;
            return Node.Status.FAILURE;
        }
        
       else if (distance <= 2f)
        {
            if(currentPoint < checkpoints.Length-1)
            {currentPoint ++;}
        else
        {
            currentPoint = 0;
        }
            return Node.Status.SUCCESS;
            
        }
        
        
       
        
        return Node.Status.RUNNING;
    }
    Node.Status CanSeePlayer()
    {
        Vector3 direction = GameWorld.player.transform.position - this.transform.position;
        float angle = Vector3.Angle(direction,this.transform.forward);
        if(direction.magnitude <= visDist && angle <= visAngle)
            return Node.Status.SUCCESS;
        return Node.Status.FAILURE;
    }
    Node.Status GoToPlayer()
    {
        GameObject player = GameWorld.player;
        float dis = Vector3.Distance(GameWorld.player.transform.position, this.transform.position);
     //   if(dis >=5f)
        agent.speed = 5f;
        agent.SetDestination(player.transform.position);
        if(!CanSee())
        {
            
            return Node.Status.FAILURE;
        }
        else if (dis <=5f)
        {
            
            return Node.Status.SUCCESS;
            
        }    
       return Node.Status.RUNNING;
    }
    Node.Status Dur()
    {
        agent.speed =5f;
        float dis = Vector3.Distance(GameWorld.player.transform.position, this.transform.position);
        
          if(CanSee() && dis<=5f)
        {
            return Node.Status.RUNNING;

        }
        print("playera gidiying");
        return GoToPlayer();
    }
}
