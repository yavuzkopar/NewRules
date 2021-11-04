using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.Dialogue.UI
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button nextButton;
        [SerializeField] Button quitButton;
        [SerializeField] Transform choiseRoot;
        [SerializeField] GameObject choisePefab;
        [SerializeField] GameObject AIResponse;
        PlayerConversant playerConversant;
        void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.onConversationUpdated += UpdateUI;
            nextButton.onClick.AddListener(() => playerConversant.Next());
            quitButton.onClick.AddListener(() => playerConversant.Quit());
            UpdateUI();
        }
        //void Next()
        //{
        //    playerConversant.Next();
        //   // UpdateUI();
        //}
        // Update is called once per frame
        void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive());
            if (!playerConversant.IsActive())
            {
                return;
            }
            AIResponse.SetActive(!playerConversant.IsChoosing());
            choiseRoot.gameObject.SetActive(playerConversant.IsChoosing());
            if (playerConversant.IsChoosing())
            {
                BuildChoiseList();
            }
            else
            {
                AIText.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
            }
            Debug.Log("ttttttt");
        }

        private void BuildChoiseList()
        {
            foreach (Transform item in choiseRoot)
            {
                Destroy(item.gameObject);
            }

            foreach (DialogueNode choise in playerConversant.GetChoises())
            {
                GameObject choiseInst = Instantiate(choisePefab, choiseRoot);
                var textComp = choiseInst.GetComponentInChildren<TextMeshProUGUI>();
                textComp.text = choise.GetText();
                Button button = choiseInst.GetComponentInChildren<Button>();
                button.onClick.AddListener(() =>
                {
                    playerConversant.SelectChoise(choise);
                    // UpdateUI();
                });
            }
        }
    }
}
