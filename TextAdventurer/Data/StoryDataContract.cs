using System.Collections.Generic;
using System.Runtime.Serialization;

/// <summary>
/// Text adventurer data namespace
/// </summary>
namespace TextAdventurer.Data
{
    /// <summary>
    /// Story data contract class
    /// </summary>
    [DataContract]
    internal class StoryDataContract
    {
        /// <summary>
        /// Title
        /// </summary>
        [DataMember]
        private string title;

        /// <summary>
        /// Author
        /// </summary>
        [DataMember]
        private string author;

        /// <summary>
        /// License
        /// </summary>
        [DataMember]
        private string license;

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        private string description;

        /// <summary>
        /// Themes
        /// </summary>
        [DataMember]
        private Dictionary<string, ThemeDataContract> themes;

        /// <summary>
        /// States
        /// </summary>
        [DataMember]
        private StateDataContract initialState;

        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get
            {
                if (title == null)
                {
                    title = string.Empty;
                }
                return title;
            }
        }

        /// <summary>
        /// Author
        /// </summary>
        public string Author
        {
            get
            {
                if (author == null)
                {
                    author = string.Empty;
                }
                return author;
            }
        }

        /// <summary>
        /// License
        /// </summary>
        public string License
        {
            get
            {
                if (license == null)
                {
                    license = string.Empty;
                }
                return license;
            }
        }

        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get
            {
                if (description == null)
                {
                    description = string.Empty;
                }
                return description;
            }
        }

        /// <summary>
        /// Themes
        /// </summary>
        public IReadOnlyDictionary<string, ThemeDataContract> Themes
        {
            get
            {
                if (themes == null)
                {
                    themes = new Dictionary<string, ThemeDataContract>();
                }
                return themes;
            }
        }

        /// <summary>
        /// Initial state
        /// </summary>
        public StateDataContract InitialState
        {
            get
            {
                if (initialState == null)
                {
                    initialState = new StateDataContract();
                }
                return initialState;
            }
        }
    }
}
