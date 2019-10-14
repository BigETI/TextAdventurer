using System.Collections.Generic;
using System.Runtime.Serialization;

/// <summary>
/// Text Adventurer data namespace
/// </summary>
namespace TextAdventurer.Data
{
    /// <summary>
    /// State data contract class
    /// </summary>
    [DataContract]
    internal class StateDataContract
    {
        /// <summary>
        /// Theme
        /// </summary>
        [DataMember]
        private string theme;

        /// <summary>
        /// Text
        /// </summary>
        [DataMember]
        private string text;

        /// <summary>
        /// SUccess
        /// </summary>
        [DataMember]
        private bool success = default;

        /// <summary>
        /// Options
        /// </summary>
        [DataMember]
        private Dictionary<string, string> options;

        /// <summary>
        /// States
        /// </summary>
        [DataMember]
        private Dictionary<string, StateDataContract> states;

        /// <summary>
        /// Theme
        /// </summary>
        public string Theme
        {
            get
            {
                if (theme == null)
                {
                    theme = string.Empty;
                }
                return theme;
            }
        }

        /// <summary>
        /// Text
        /// </summary>
        public string Text
        {
            get
            {
                if (text == null)
                {
                    text = string.Empty;
                }
                return text;
            }
        }

        /// <summary>
        /// Success
        /// </summary>
        public bool Success => success;

        /// <summary>
        /// Options
        /// </summary>
        public IReadOnlyDictionary<string, string> Options
        {
            get
            {
                if (options == null)
                {
                    options = new Dictionary<string, string>();
                }
                return options;
            }
        }

        /// <summary>
        /// States
        /// </summary>
        public Dictionary<string, StateDataContract> States
        {
            get
            {
                if (states == null)
                {
                    states = new Dictionary<string, StateDataContract>();
                }
                return states;
            }
        }
    }
}
