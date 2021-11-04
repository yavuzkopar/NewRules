using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Control;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour
    {
        [SerializeField] Dialogue dialogue;

        public void StartDialogue(PlayerConversant conversant)
        {
            conversant.StartDialogue(this,dialogue);
        }
    }
}
