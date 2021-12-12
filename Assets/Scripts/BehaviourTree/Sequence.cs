using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(string n)
    {
        name = n;
    }
    public override Status Process()
    {
        Status childStatus = children[currentChild].Process();
        if(childStatus == Status.RUNNING) 
        { 
            Debug.Log(children[currentChild].name + " =>  Running");
            return Status.RUNNING;
        }
        if(childStatus == Status.FAILURE)
        {
            Debug.Log(children[currentChild].name + " =>  Failed");
             return childStatus;
        }
        Debug.Log(children[currentChild].name + " =>  Successful");
        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.SUCCESS;
        } 

        return Status.RUNNING;
    }
}
