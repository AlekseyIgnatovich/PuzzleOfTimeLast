// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("X4ArwfyPp6x9Z/ct/4DB1gBZvxQMK65FRuqtbGgZMAM0x6sLfJchosZNzdFSGbSOOqdi4ohDKVDfiw2/LyZwQGiHwln9DnBgPh0mcAPAIOi6sVV0RcI+S06h5KU59fRWkvwjbCkwza5CIvSQiu7HJALeFh/J/hYqFfhd+D/oYmpWUWUxXT/1IVQeB3DrDsJqfEkRGfltei6kwZLZWvulcTiKCSo4BQ4BIo5Ajv8FCQkJDQgLKWcYfhoklKXZoE+2zd3UuHCcbVQVUUKF0QXqx0y5tAFVizhONZMzdIoJBwg4igkCCooJCQi2s37Ik90vZbqMnhE5rD4O4JHvqSjq1squPV/9/5PiIXnEvw75VWGeLoWbyV3F7Z5RN/GGqmQn9woLCQgJ");
        private static int[] order = new int[] { 10,13,2,3,9,7,11,7,10,10,10,13,13,13,14 };
        private static int key = 8;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
