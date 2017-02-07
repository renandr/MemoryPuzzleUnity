using System.Collections.Generic;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// source: http://stackoverflow.com/questions/26280788/dictionary-enum-key-performance
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public struct EnumComparer<TEnum> : IEqualityComparer<TEnum> where TEnum : struct
    {
        private int ToInt(TEnum en)
        {
            return EnumInt32ToInt.Convert(en);
        }

        public bool Equals(TEnum firstEnum, TEnum secondEnum)
        {
            return ToInt(firstEnum) == ToInt(secondEnum);
        }

        public int GetHashCode(TEnum firstEnum)
        {
            return ToInt(firstEnum);
        }
    }
}