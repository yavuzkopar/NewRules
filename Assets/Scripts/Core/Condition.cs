using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Condition
{
    [SerializeField]
    string predicate;
    [SerializeField]
    string[] paramaters;

   public bool Check(IEnumerable<IPredicateEvaluator> evaluators){
        foreach (var item in evaluators)
        {
            bool? result = item.Eveluate(predicate,paramaters);
            if(result == null){
                continue;
            }
            if(result == false) return false;
        }
        return true;
    }
}
