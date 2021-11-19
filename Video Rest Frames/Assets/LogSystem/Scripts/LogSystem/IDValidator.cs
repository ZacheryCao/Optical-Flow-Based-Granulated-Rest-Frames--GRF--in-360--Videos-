using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace LogSystem
{
    public static class IDValidator 
    {

        public static bool ValidateID(string text)
        {
            if (text.Length != 5) return false;
            if (text == "00000") return false;
            List<int> l1 = new List<int> { 23, 43, 59, 79, 97};
            List<int> l2 = new List<int> { 503,509,521,523,541,547,557,563,569,571};

            int ID;
            if(!int.TryParse(text, out ID))
            {
                return false;
            }
            foreach(int i in l1)
            {
                if (l2.Contains(ID / i))
                {
                    return true;
                }
            }
            return false;
        }

    }
}