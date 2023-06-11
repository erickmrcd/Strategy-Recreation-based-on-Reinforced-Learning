using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class SelectedUnitUI : MonoBehaviour
{
    [SerializeField] private Transform characterPortraitContainerTransform;
    [SerializeField] private Transform characterPortraitPrefab;
    [SerializeField] private Transform unitInfoPrefab;

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += Instance_OnSelectedUnitChanged;
    }


    private void Instance_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateCharacterPortrait();
    }

    private void CreateCharacterPortrait()
    {
        foreach (Transform portrait in characterPortraitContainerTransform)
        {
            Destroy(portrait.gameObject);
        }

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        if (selectedUnit != null)
        {
            Transform characterPortraitTransform = Instantiate(characterPortraitPrefab, characterPortraitContainerTransform);
            GameObject characterPortraitObject = new GameObject(selectedUnit.GetUnitName());
            Image characterPortrait = characterPortraitObject.AddComponent<Image>();
            characterPortrait.sprite = selectedUnit.CharacterPortrait;
            characterPortrait.transform.SetParent(characterPortraitTransform, false);

        }

    }
}
