using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestList : MonoBehaviour, IPredicateEvaluator
{
    public List<Quest> quests = new List<Quest>();

    public void AddQuest(Quest quest){
        quests.Add(quest);
        Instantiate(prefab,panel);
    }
    public Transform panel;
    public GameObject prefab;
    public void RemoveQuest(Quest quest)
    {
        quests.Remove(quest);
    }
    public bool HasQuest(string quest)
    {
        foreach (var item in quests)
        {
            if (item.name == quest)
            {
                return true;
            }
        }
        return false;
    }

    public bool? Eveluate(string predicate, string[] paramaters)
    {
        
        switch (predicate)
        {
            case "HasQuest":
            return HasQuest(paramaters[0]);
            
        }
        return null;
    }
}
