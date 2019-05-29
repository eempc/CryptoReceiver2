using System;

namespace AddressDisplay.ExtraTools {
    public static class ExtensionMethods {
        public static int WordCount(this String str) {
            return str.Split(new char[] { ' ', '?', '!' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static bool IsUpperString(this String str) {
            foreach (char c in str) {
                if (char.IsLower(c)) return false;
            }
            return true;
        }
    }
}
