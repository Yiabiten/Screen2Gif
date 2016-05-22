namespace ScreenToGif.Util
{
    /// <summary>
    /// Class that holds the current culture selected by the user.
    /// </summary>
    public static class CultureUtil
    {
        private static string _lang = "";

        /// <summary>
        /// The current UI language.
        /// </summary>
        public static string Lang
        {
            get { return _lang; }
            set { _lang = value; }
        }
    }
}
