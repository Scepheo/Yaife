namespace Yaife
{
    public interface IFrame
    {
        /// <summary>
        /// Returns an array of strings for this frame; one for each column.
        /// </summary>
        object[] ToStrings();

        /// <summary>
        /// Parses an array of strings (one for each column) into a frame.
        /// </summary>
        void Parse(string[] strings);
    }
}
