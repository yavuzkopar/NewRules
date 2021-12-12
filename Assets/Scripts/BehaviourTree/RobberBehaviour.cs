using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : MonoBehaviour {

    public GameObject diamond;
    public GameObject van;
    public GameObject van2;
    public GameObject backDoor;
    public GameObject frontDoor;
    NavMeshAgent agent;

    BehaviourTree tree;
    public enum ActionState{IDLE,WORKING}
    ActionState state = ActionState.IDLE;

    [Range(0,1000)]
    public int money;
    Node.Status treeStatus = Node.Status.RUNNING;
    private void Start() {
        agent = GetComponent<NavMeshAgent>();


        tree = new BehaviourTree();

        Selector selector = new Selector("Selector");
            Sequence steal = new Sequence("Steal Something");
                Leaf needMoney = new Leaf("Has Money",NeedMoney);
                Selector openDoor = new Selector("Open Door");
                    Leaf goToBackdoor = new Leaf("Go To Backdoor",GoToBackDoor);
                    Leaf goToFrontdoor = new Leaf("Go To Frontdoor",GoToFrontDoor);
                Leaf goToDiamond = new Leaf("Go To Diamond",GoToDiamond);
                Leaf goToVan = new Leaf("Go To Van",GoToVan);
            Sequence tepe = new Sequence("Tepe");
                Leaf goToVan2 = new Leaf("Go To Van2",GoToVaniki);

        //condition
        steal.AddChild(needMoney);
        //selector
        openDoor.AddChild(goToBackdoor);
        openDoor.AddChild(goToFrontdoor);
        
        // sequence
        steal.AddChild(openDoor);
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        
        //sequence
        tepe.AddChild(goToVan2);

        //Selector
        selector.AddChild(steal);
        selector.AddChild(tepe);

        //tree
        tree.AddChild(selector);
        tree.PrintTree();
        
    }

    Node.Status NeedMoney()
    {
        if(money<500)
        {
            return Node.Status.SUCCESS;
        }
        return Node.Status.FAILURE;
    }
#region moveToes
    Node.Status GoToDiamond()
    {
         Node.Status s = GoToDestination(diamond.transform.position);
         if(s == Node.Status.SUCCESS)
         {
             diamond.transform.parent = this.transform;
         }
         return s;
    }
    Node.Status GoToBackDoor()
    {
         return GoToDoor(backDoor);
    }
     Node.Status GoToFrontDoor()
    {
         return GoToDoor(frontDoor);
    }
    Node.Status GoToVan()
    {
       return GoToDestination(van.transform.position);
    }
     Node.Status GoToVaniki()
    {
       return GoToDestination(van2.transform.position);
    }
    Node.Status GoToDoor(GameObject door)
    {
        Node.Status s = GoToDestination(door.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            if (!door.GetComponent<Lock>().isLocked)
            {
                door.SetActive(false);
                return Node.Status.SUCCESS;
            }
            Debug.Log("fail");
            return Node.Status.FAILURE;
        }
        else
            return s;
    }
    Node.Status GoToDestination(Vector3 destination)
    {
        float distance = Vector3.Distance(destination,this.transform.position);
        if (state == ActionState.IDLE)
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
        return Node.Status.RUNNING;
    }
#endregion
    private void Update() {
        if (treeStatus != Node.Status.SUCCESS)
        {
           treeStatus = tree.Process();
        }
    }
}