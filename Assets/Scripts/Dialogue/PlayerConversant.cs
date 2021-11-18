using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RPG.Control;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        Dialogue currentDialogue;
        //   [SerializeField] Dialogue testDialogue;
        DialogueNode currentNode = null;
        bool isChoosing = false;

        public event Action onConversationUpdated;
        AIConversant currentConversant;
        private void Awake()
        {

        }

        //private IEnumerator Start()
        //{
        //    yield return new WaitForSeconds(2);
        //    StartDialogue(testDialogue);
        //}
        public void StartDialogue(AIConversant newConversat, Dialogue newDialogue)
        {
            currentConversant = newConversat;
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
            onConversationUpdated();
            Debug.Log(currentDialogue.name);
            GetComponent<PlayerController>().rotationSpeed = 0;
        }
        public bool IsActive()
        {
            return currentDialogue != null;
        }
        public string GetText()
        {
            if (currentNode == null)
            {
                return "";
            }
            return currentNode.GetText();
        }
        public bool IsChoosing()
        {
            return isChoosing;
        }
        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
        {
            foreach (var node in inputNode)
            {
                if (node.CheckCondition(GetEvaluators()))
                {
                    yield return node;
                }
            }
        }
        IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
        }
        public void Next()
        {
            int numPlayerResponses = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitAction();
                onConversationUpdated();

                return;
            }

            DialogueNode[] children = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            TriggerExitAction();
            currentNode = children[randomIndex];
            Debug.Log("ASADF");
            TriggerEnterAction();
            onConversationUpdated();


        }
        void TriggerEnterAction()
        {
            if (currentNode != null && currentNode.GetOnEnterAction() != "")
            {
                TriggerAction(currentNode.GetOnEnterAction());
            }
        }
        void TriggerExitAction()
        {
            if (currentNode != null && currentNode.GetOnExitAction() != "")
            {
                TriggerAction(currentNode.GetOnExitAction());
            }
        }
        void TriggerAction(string action)
        {
            if (action == "") return;

            foreach (DialogueTrigger item in currentConversant.GetComponents<DialogueTrigger>())
            {
                item.Trigger(action);
            }
        }
        public void Quit()
        {

            currentDialogue = null;
            TriggerExitAction();
            currentConversant = null;
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
            GetComponent<PlayerController>().rotationSpeed = 5;
        }
        public bool HasNext()
        {
            return FilterOnCondition(currentDialogue.GetChildNodes(currentNode)).Count() > 0;
        }
        public IEnumerable<DialogueNode> GetChoises()
        {
            return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));

        }

        public void SelectChoise(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            TriggerEnterAction();
            isChoosing = false;
            Debug.Log("Sectiii");
            Next();
        }
    }
}
