using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [Serializable]
    public class DialogueExchange
    {
        public List<Dialogue> dialogues = new List<Dialogue>();
        public string identifier = "";
        public bool hasBeenShown = false;
    }

    [Serializable]
    public class Dialogue
    {
        public Character character = Character.invalid;
        public Emotion emotion = Emotion.neutral;
        public string dialogue = "";
        public float duration = 0.0f;
        public string audioClipName = "";
        
    }

    [Serializable]
    public class Portrait
    {
        public Character character = Character.invalid;
        public Emotion emotion = Emotion.neutral;
        public Sprite sprite;
    }

    public enum Character
    {
        surgeon, nurse, grandfather, invalid
    }

    public enum Emotion
    {
        happy, too_happy, neutral, angry, sad, disappointed, invalid
    }

    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI speech;
    [SerializeField] GameObject dialogueContainer;
    [SerializeField] List<DialogueExchange> dialogueExchanges;
    [SerializeField] List<Portrait> portraits;

    private float timeShowingCurrentDialogue = 0.0f;
    private float currentDialogueTimeLimit = 0.0f;
    private string currentDialogueExchangeIdentifier = "";
    private int currentDialogueIndex = 0;
    private Dictionary<string, DialogueExchange> dialogueExchangeMap = new Dictionary<string, DialogueExchange>();
    private Dictionary<Character, Dictionary<Emotion, Portrait>> portraitMap = new Dictionary<Character, Dictionary<Emotion, Portrait>>();
    private string currentDialogueAudio = "";

    private void Start()
    {
        foreach(DialogueExchange exchange in dialogueExchanges)
        {
            if (dialogueExchangeMap.ContainsKey(exchange.identifier))
            {
                Debug.LogError("Cannot add dialogue exchange, identifier["+exchange.identifier+"] already exists.");
            }
            else
            {
                dialogueExchangeMap.Add(exchange.identifier, exchange);
            }
        }
        foreach(Portrait portrait in portraits)
        {
            if (!portraitMap.ContainsKey(portrait.character))
            {
                portraitMap.Add(portrait.character, new Dictionary<Emotion, Portrait>());
            }
            if (portraitMap[portrait.character].ContainsKey(portrait.emotion))
            {
                Debug.LogError("Cannot add portrait, character["+portrait.character+"] with emotion["+portrait.emotion+"] already exists.");
            }
            else
            {
                portraitMap[portrait.character].Add(portrait.emotion, portrait);
            }
        }
    }

    private void Update()
    {
        if(currentDialogueTimeLimit > 0.0f)
        {
            timeShowingCurrentDialogue += Time.deltaTime;
            if(timeShowingCurrentDialogue > currentDialogueTimeLimit)
            {
                currentDialogueIndex++;
                currentDialogueTimeLimit = 0.0f;
                timeShowingCurrentDialogue = 0.0f;
                if (currentDialogueAudio != "")
                {
                    AudioManager.Instance.StopSound(currentDialogueAudio);
                }

                currentDialogueAudio = "";
                if (dialogueExchangeMap[currentDialogueExchangeIdentifier].dialogues.Count <= currentDialogueIndex)
                {
                    dialogueContainer.SetActive(false);
                    currentDialogueIndex = 0;
                    currentDialogueExchangeIdentifier = "";
                }
                else
                {
                    updateCurrentDialogue();
                }
            }
        }
    }

    public void startDialogueExchange(string identifier)
    {
        if(dialogueExchangeMap.ContainsKey(identifier) &&
            !dialogueExchangeMap[identifier].hasBeenShown)
        {
            if(currentDialogueAudio != "")
            {
                AudioManager.Instance.StopSound(currentDialogueAudio);
                currentDialogueAudio = "";
            }

            currentDialogueExchangeIdentifier = identifier;
            currentDialogueIndex = 0;
            timeShowingCurrentDialogue = 0.0f;
            dialogueExchangeMap[identifier].hasBeenShown = true;

            updateCurrentDialogue();
        }
    }

    private void updateCurrentDialogue()
    {
        Dialogue currentDialogue = dialogueExchangeMap[currentDialogueExchangeIdentifier].dialogues[currentDialogueIndex];
        dialogueContainer.SetActive(true);

        if (!portraitMap.ContainsKey(currentDialogue.character))
        {
            Debug.LogError("Could not update portrait, character["+currentDialogue.character+"] has not been mapped.");
        }
        else
        {
            if (!portraitMap[currentDialogue.character].ContainsKey(currentDialogue.emotion))
            {
                Debug.LogError("Could not update portrait, emotion["+currentDialogue.emotion+"] for character["+currentDialogue.character+"] has not been mapped.");
            }
            else
            {
                portrait.sprite = portraitMap[currentDialogue.character][currentDialogue.emotion].sprite;
            }
        }

        speech.text = currentDialogue.dialogue;
        if(currentDialogue.audioClipName != "")
        {
            AudioManager.Instance.PlaySound(currentDialogue.audioClipName);
            currentDialogueAudio = currentDialogue.audioClipName;
        }

        currentDialogueTimeLimit = currentDialogue.duration;
    }
}
