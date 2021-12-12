using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Node
{
    public delegate Status Tick();
    public Tick PrcessMethod;

    public Leaf(){}
    public Leaf(string n,Tick pm)
    {
      name = n;
      PrcessMethod = pm;  
    }

    public override Status Process()
    {
        if(PrcessMethod != null)
            return PrcessMethod();
        return Status.FAILURE;
    }
}
