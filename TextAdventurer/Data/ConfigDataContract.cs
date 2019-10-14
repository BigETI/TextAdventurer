using System.Runtime.Serialization;

/// <summary>
/// Text Adventurer data namespace
/// </summary>
namespace TextAdventurer.Data
{
    /// <summary>
    /// Configuration data contract class
    /// </summary>
    [DataContract]
    public class ConfigDataContract
    {
        /// <summary>
        /// Default save states path
        /// </summary>
        private static readonly string defaultSaveStatesPath = "save-states.json";

        /// <summary>
        /// Default save states path
        /// </summary>
        [DataMember]
        private string saveStatesPath = defaultSaveStatesPath;

        /// <summary>
        /// Letter delay
        /// </summary>
        [DataMember]
        private double letterDelay = 0.03125;

        /// <summary>
        /// Number delay
        /// </summary>
        [DataMember]
        private double numberDelay = 0.03125;

        /// <summary>
        /// Symbol delay
        /// </summary>
        [DataMember]
        private double symbolDelay = 0.0625;

        /// <summary>
        /// Save states path
        /// </summary>
        public string SaveStatesPath
        {
            get
            {
                if (saveStatesPath == null)
                {
                    saveStatesPath = defaultSaveStatesPath;
                }
                else if (saveStatesPath.Trim().Length <= 0)
                {
                    saveStatesPath = defaultSaveStatesPath;
                }
                return saveStatesPath;
            }
        }

        /// <summary>
        /// Leter delay
        /// </summary>
        public double LetterDelay => letterDelay;

        /// <summary>
        /// Number delay
        /// </summary>
        public double NumberDelay => numberDelay;

        /// <summary>
        /// Symbol delay
        /// </summary>
        public double SymbolDelay => symbolDelay;
    }
}
