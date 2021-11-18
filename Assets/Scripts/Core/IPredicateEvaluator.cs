using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPredicateEvaluator{

    bool? Eveluate(string predicate, string[] paramaters);

}
