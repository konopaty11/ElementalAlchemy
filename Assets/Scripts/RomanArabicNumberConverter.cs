using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RomanArabicNumerConverter", menuName = "Scriptable Objects/RomanArabicNumerConverter")]
public class RomanArabicNumberConverter : ScriptableObject
{
    [SerializeField] List<RomanArabicNumberSerializable> numbers;

    public string GetRomanNumber(int _arabicNumber)
    {
        foreach (RomanArabicNumberSerializable _numberPair in numbers)
        {
            if (_numberPair.arabicNumber == _arabicNumber)
                return _numberPair.romanNumber;
        }

        Debug.LogError("A arabic number is missing");
        return null;
    }
}

[Serializable]
class RomanArabicNumberSerializable
{
    public int arabicNumber;
    public string romanNumber;
}
