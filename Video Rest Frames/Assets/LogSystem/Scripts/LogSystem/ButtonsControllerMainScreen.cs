using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LogSystem;
using System.IO;

namespace LogSystem
{
    public class ButtonsControllerMainScreen : MonoBehaviour
    {
        public static Action<string> OnSetUserID = delegate { };
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button startButton;
        [SerializeField] private TMP_Text lockMessage;

        private void Start()
        {
            LockNextButton();
            lockMessage.gameObject.SetActive(false);
        }

        bool isUnlocked = false;

        private void Update()
        {
            //LockNextButton();

            //isUnlocked = inputField.text == "" ? false : true;
            //isUnlocked = int.TryParse(inputField.text, out _) ? false : true;
            //isUnlocked = !ValidateID() ? false : true;
            if (inputField.text == "") { return; }
            if (int.TryParse(inputField.text, out _) == false) { LockNextButton(); return; }
            if (!ValidateID()) { LockNextButton(); return; }
            //if (!isUnlocked)
            //{
            //    LockNextButton();
            //    return;
            //}

            UnlockNextButton();
        }

        private bool ValidateID()
        {
            return IDValidator.ValidateID(inputField.text);
        }

        private void UnlockNextButton()
        {
            startButton.gameObject.SetActive(true);
            lockMessage.gameObject.SetActive(false);
            startButton.interactable = true;
        }

        private void LockNextButton()
        {
            startButton.gameObject.SetActive(false);
            lockMessage.gameObject.SetActive(true);
            startButton.interactable = false;
        }

        public void OnClick()
        {
            OnSetUserID(inputField.text);
        }


        public void Key(string _key)
        {
            inputField.text += _key;
        }

        public void KeyClear()
        {
            inputField.text = "";
        }
    }
}