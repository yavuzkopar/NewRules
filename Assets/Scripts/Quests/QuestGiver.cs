using UnityEngine;

public class QuestGiver : MonoBehaviour {
    [SerializeField] Quest quest;
    QuestList questList;
     private void Start() {
        questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
    }
    public void GiveQuest()
    {
            Debug.Log("Quest Gived");
            questList.AddQuest(quest);
    }
        public void ComplateQuest()
    {
            Debug.Log("Quest Complated");
            questList.RemoveQuest(quest);
    }
}