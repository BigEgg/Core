using System.Text;

namespace BigEgg.Framework.UnitTesting
{
    public static class TestHelper
    {
        /// <summary>
        /// Create a string with multiple character for testing.
        /// </summary>
        /// <param name="character">The specific character.</param>
        /// <param name="count">The time of the character in the string.</param>
        /// <returns>The string.</returns>
        public static string CreateString(char character, int count)
        {
            StringBuilder builder = new StringBuilder(count);
            for (int i = 0; i < count; i++)
            {
                builder.Append(character);
            }
            return builder.ToString();
        }
    }
}
