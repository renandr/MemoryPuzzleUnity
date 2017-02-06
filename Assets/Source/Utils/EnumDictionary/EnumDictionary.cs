using System.Collections.Generic;

namespace GGS.CakeBox.Utils
{
    public class EnumDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : struct
    {
        public EnumDictionary() : base(new EnumComparer<TKey>())
        {
        }
    }
}