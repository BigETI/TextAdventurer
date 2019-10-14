using System;
using System.Runtime.Serialization;

/// <summary>
/// Text Adventurer data namespace
/// </summary>
namespace TextAdventurer.Data
{
    /// <summary>
    /// Save state data contract class
    /// </summary>
    [DataContract]
    public class SaveStateDataContract
    {
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        private string name;

        /// <summary>
        /// State
        /// </summary>
        [DataMember]
        private string state;

        /// <summary>
        /// Date and time
        /// </summary>
        [DataMember]
        private string dateTime;

        /// <summary>
        /// Date and time reference
        /// </summary>
        private DateTime? dateTimeRef;

        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get
            {
                if (name == null)
                {
                    name = string.Empty;
                }
                return name;
            }
            private set
            {
                if (value != null)
                {
                    name = value;
                }
            }
        }

        /// <summary>
        /// State
        /// </summary>
        public string State
        {
            get
            {
                if (state == null)
                {
                    state = string.Empty;
                }
                return state;
            }
            private set
            {
                if (value != null)
                {
                    state = value;
                }
            }
        }

        /// <summary>
        /// Date and time
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                if (dateTime == null)
                {
                    dateTimeRef = DateTime.Now;
                    dateTime = dateTimeRef.Value.ToString();
                }
                else
                {
                    DateTime date_time;
                    if (DateTime.TryParse(dateTime, out date_time))
                    {
                        dateTimeRef = date_time;
                    }
                    else
                    {
                        dateTimeRef = DateTime.Now;
                    }
                }
                return dateTimeRef.Value;
            }
            set
            {
                dateTimeRef = value;
                dateTime = value.ToString();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="state">State</param>
        /// <param name="dateTime">Date and time</param>
        public SaveStateDataContract(string name, string state, DateTime dateTime)
        {
            Name = name;
            State = state;
            DateTime = dateTime;
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return ("[ " + DateTime.ToString() + " ] : " + Name);
        }
    }
}
