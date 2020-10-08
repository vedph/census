using System.Text;

namespace Census.Import
{
    public sealed class WhitespaceTextFilter : ITextFilter
    {
        public string Apply(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            StringBuilder sb = new StringBuilder();
            bool prevWS = true;

            foreach (char c in text)
            {
                if (char.IsWhiteSpace(c))
                {
                    if (prevWS) break;
                    sb.Append(' ');
                    prevWS = true;
                    continue;
                }
                else
                {
                    sb.Append(c);
                    prevWS = false;
                }
            }

            // right trim
            if (sb.Length > 0 && sb[^1] == ' ')
                sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }
    }
}
