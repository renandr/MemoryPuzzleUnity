using System;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// Converts integers to roman numeral strings
    /// Reference: https://www.daniweb.com/programming/software-development/code/377055/three-ways-to-convert-an-integer-to-a-roman-numeral
    /// </summary>
    public static class RomanNumeralConverter
    {
        private struct RomanDigit
        {
            public string RomanString;
            public int Value;
        }

        private static readonly RomanDigit[] romanDigits =
        {
            new RomanDigit{RomanString = "M",  Value = 1000},
            new RomanDigit{RomanString = "CM", Value = 900},
            new RomanDigit{RomanString = "D",  Value = 500},
            new RomanDigit{RomanString = "CD", Value = 400},
            new RomanDigit{RomanString = "C",  Value = 100},
            new RomanDigit{RomanString = "XC", Value = 90},
            new RomanDigit{RomanString = "L",  Value = 50},
            new RomanDigit{RomanString = "XL", Value = 40},
            new RomanDigit{RomanString = "X",  Value = 10},
            new RomanDigit{RomanString = "IX", Value = 9},
            new RomanDigit{RomanString = "V",  Value = 5},
            new RomanDigit{RomanString = "IV", Value = 4},
            new RomanDigit{RomanString = "I",  Value = 1},
        };

        /// <summary>
        /// Converts a number to a roman numeral
        /// </summary>
        /// <param name="number">The number</param>
        /// <returns>The roman numeral</returns>
        public static string ToRomanNumeral(int number)
        {
            // Handle the most common cases (low numbers) directly
            switch (number)
            {
                case 0:
                    return "0";
                case 1:
                    return "I";
                case 2:
                    return "II";
                case 3:
                    return "III";
                case 4:
                    return "IV";
                case 5:
                    return "V";
                case 6:
                    return "VI";
                case 7:
                    return "VII";
                case 8:
                    return "VIII";
                case 9:
                    return "IX";
                case 10:
                    return "X";
            }

            // Otherwise switch to the more complex solution
            string result = "";
            if (number < 0)
            {
                result = "-";
                number = Math.Abs(number);
            }
            for (int i = 0; number > 0; i++)
            {
                while (romanDigits[i].Value <= number)
                {
                    result += romanDigits[i].RomanString;
                    number -= romanDigits[i].Value;
                }
            }
            return result;
        }
    }
}