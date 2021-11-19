using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

    public class HandsRole : MonoBehaviour
    {
        public static Action<Hand> OnDominantHandSelected = delegate { };
        public static Action<Hand> OnNonDominantHandSelected = delegate { };
        public GameObject leftPointer;
        public GameObject rightPointer;

        public Hand leftHand;
        public Hand rightHand;


        private Hand dominantHand;
        private Hand nonDominantHand;
        
        void Awake()
        {
            SetDominantHand(rightHand);
        }

        public void SetNonDominantHand(Hand _hand)
        {
            nonDominantHand = _hand;
        }
        public void SetDominantHand(Hand _hand)
        {
            dominantHand = _hand;
            SetNonDominantHand(_hand.otherHand);

            if (_hand == leftHand)
            {
                leftPointer.SetActive(true);
                rightPointer.SetActive(false);
            }
            else if (_hand == rightHand)
            {
                leftPointer.SetActive(false);
                rightPointer.SetActive(true);
            }

            //if (_hand == leftHand)
            //{
            //    dominantHand = leftHand;
            //    nonDominantHand = rightHand;
            //}
            //else if (_hand == rightHand)
            //{
            //    dominantHand = rightHand;
            //    nonDominantHand = leftHand;
            //}
            //AddLaserPointer(dominantHand);
            //RemoveHapticPulse(dominantHand);
            //AddHapticPulse(nonDominantHand);
            //RemoveLaserPointer(nonDominantHand);
            UpdateListeners();
        }

        public void UpdateListeners()
        {
            OnDominantHandSelected(dominantHand);
            OnNonDominantHandSelected(nonDominantHand);
        }

        //private void AddLaserPointer(Hand _hand)
        //{
        //    _hand.gameObject.AddComponent<SteamVR_LaserPointer>();
        //}
        //private void RemoveLaserPointer(Hand _hand)
        //{
        //    var comp = _hand.gameObject.GetComponent<SteamVR_LaserPointer>();
        //    if (comp == null) return;
        //    Destroy(comp);
        //    var obj = _hand.transform.Find("New Game Object");
        //    Destroy(obj.gameObject);
        //}

        //private void AddHapticPulse(Hand _hand)
        //{
        //    _hand.gameObject.AddComponent<HapticPulse>();
        //}

        //private void RemoveHapticPulse(Hand _hand)
        //{
        //    var comp = _hand.gameObject.GetComponent<HapticPulse>();
        //    if (comp == null) return;
        //    Destroy(comp);
        //}

        public Hand GetDominantHand()
        {
            return dominantHand;
        }

        public Hand GetNonDominantHand()
        {
            return nonDominantHand;
        }

    }