namespace thehinc.ioc {
    /// <summary>
    /// Attribute for Interfaces so that we can inject via property automatically
    /// </summary>
    [Serializable]
    public class RequiresInjectionAttribute : Attribute {
        public string OverrideName { get; }

        public IDictionary<string, string> ConstructorDictionary { get; }
        /// <summary>
        /// Overload constructor for providing an overload name
        /// </summary>
        /// <param name="Name"></param>
        public RequiresInjectionAttribute (string Name = null, params string[] ConstructorValues) {
            OverrideName = Name;
            //Created a constructor values dictionary
            if (ConstructorValues?.Length > 0 && ConstructorValues.Length % 2 == 0) {
                ConstructorDictionary = new Dictionary<string, string> ();
                for (int i = 0, j = 1; j < ConstructorValues.Length; i += 2, j = i + 1)
                    ConstructorDictionary.Add (ConstructorValues[i], ConstructorValues[j]);
            }
        }
    }
}