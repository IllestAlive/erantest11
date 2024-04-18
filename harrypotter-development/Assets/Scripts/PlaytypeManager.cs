using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaytypeManager : Instancable<PlaytypeManager>
{
    public enum PlayType
    {
        WithCustomizedCharacters, 
        WithoutCustomizedCharacters,
        OriginalCharacters
    }

    public PlayType SelectedPlayType;

    public GameObject customizedCharactersUI, nonCustomizedCharactersUI, originalCharacterUI;

    private void Start()
    {
        if (SelectedPlayType == PlayType.WithCustomizedCharacters)
        {
            CharacterSelection.Instance.characters[0].SetActive(true);

            for (int i = 0; i < CommanderNFTSelection.Instance.commanderNftList.Count; i++)
            {
                CommanderNFTSelection.Instance.commanderNftList[i].SetActive(false);
            }
            
            OriginalCharacterHolder.Instance.originalCharacter.SetActive(false);
            
            customizedCharactersUI.SetActive(true);
            nonCustomizedCharactersUI.SetActive(false);
            originalCharacterUI.SetActive(false);

            OfflineUIManager.Instance.selectionUI = OfflineUIManager.Instance.customizedUI;

        }

        if (SelectedPlayType == PlayType.WithoutCustomizedCharacters)
        {
            CommanderNFTSelection.Instance.commanderNftList[0].SetActive(true);
            
            for (int i = 0; i < CharacterSelection.Instance.characters.Count; i++)
            {
                CharacterSelection.Instance.characters[i].SetActive(false);
            }
            
            OriginalCharacterHolder.Instance.originalCharacter.SetActive(false);

            nonCustomizedCharactersUI.SetActive(true);
            customizedCharactersUI.SetActive(false);
            originalCharacterUI.SetActive(false);

            OfflineUIManager.Instance.selectionUI = OfflineUIManager.Instance.nonCustomizedUI;
        }

        if (SelectedPlayType == PlayType.OriginalCharacters)
        {
            OriginalCharacterHolder.Instance.originalCharacter.SetActive(true);

            for (int i = 0; i < CharacterSelection.Instance.characters.Count; i++)
            {
                CharacterSelection.Instance.characters[i].SetActive(false);
            }
            for (int i = 0; i < CommanderNFTSelection.Instance.commanderNftList.Count; i++)
            {
                CommanderNFTSelection.Instance.commanderNftList[i].SetActive(false);
            }
            
            originalCharacterUI.SetActive(true);
            nonCustomizedCharactersUI.SetActive(false);
            customizedCharactersUI.SetActive(false);
        }
    }
}
