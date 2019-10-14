using FastConsoleUI;
using System;
using System.Runtime.Serialization;

/// <summary>
/// Text Adventurer data namespace
/// </summary>
namespace TextAdventurer.Data
{
    /// <summary>
    /// Theme data contract class
    /// </summary>
    [DataContract]
    internal class ThemeDataContract
    {
        /// <summary>
        /// Default value
        /// </summary>
        private static readonly string defaultValue = "Default";

        /// <summary>
        /// Foreground color
        /// </summary>
        [DataMember]
        private string foregroundColor;

        /// <summary>
        /// Background color
        /// </summary>
        [DataMember]
        private string backgroundColor;

        /// <summary>
        /// Hint foreground color
        /// </summary>
        [DataMember]
        private string hintForegroundColor;

        /// <summary>
        /// Hint background color
        /// </summary>
        [DataMember]
        private string hintBackgroundColor;

        /// <summary>
        /// Selection foreground color
        /// </summary>
        [DataMember]
        private string selectionForegroundColor;

        /// <summary>
        /// Selection background color
        /// </summary>
        [DataMember]
        private string selectionBackgroundColor;

        /// <summary>
        /// Foreground color reference
        /// </summary>
        private ConsoleColor? foregroundColorReference;

        /// <summary>
        /// Background color reference
        /// </summary>
        private ConsoleColor? backgroundColorReference;

        /// <summary>
        /// Hint foreground color reference
        /// </summary>
        private ConsoleColor? hintForegroundColorReference;

        /// <summary>
        /// Hint background color reference
        /// </summary>
        private ConsoleColor? hintBackgroundColorReference;

        /// <summary>
        /// Selection foreground color reference
        /// </summary>
        private ConsoleColor? selectionForegroundColorReference;

        /// <summary>
        /// Selection background color reference
        /// </summary>
        private ConsoleColor? selectionBackgroundColorReference;

        /// <summary>
        /// Foreground color
        /// </summary>
        public ConsoleColor ForegroundColor
        {
            get
            {
                if (foregroundColor == null)
                {
                    foregroundColor = defaultValue;
                }
                if (foregroundColorReference == null)
                {
                    foregroundColorReference = ParseColor(foregroundColor, BufferCell.defaultForegroundColor);
                }
                return foregroundColorReference.Value;
            }
        }

        /// <summary>
        /// Background color
        /// </summary>
        public ConsoleColor BackgroundColor
        {
            get
            {
                if (backgroundColor == null)
                {
                    backgroundColor = defaultValue;
                }
                if (backgroundColorReference == null)
                {
                    backgroundColorReference = ParseColor(backgroundColor, BufferCell.defaultBackgroundColor);
                }
                return backgroundColorReference.Value;
            }
        }

        /// <summary>
        /// Hint foreground color
        /// </summary>
        public ConsoleColor HintForegroundColor
        {
            get
            {
                if (hintForegroundColor == null)
                {
                    hintForegroundColor = defaultValue;
                }
                if (hintForegroundColorReference == null)
                {
                    hintForegroundColorReference = ParseColor(hintForegroundColor, BufferCell.defaultHintForegroundColor);
                }
                return hintForegroundColorReference.Value;
            }
        }

        /// <summary>
        /// Hint background color
        /// </summary>
        public ConsoleColor HintBackgroundColor
        {
            get
            {
                if (hintBackgroundColor == null)
                {
                    hintBackgroundColor = defaultValue;
                }
                if (hintBackgroundColorReference == null)
                {
                    hintBackgroundColorReference = ParseColor(hintBackgroundColor, BufferCell.defaultHintBackgroundColor);
                }
                return hintBackgroundColorReference.Value;
            }
        }

        /// <summary>
        /// Selection foreground color
        /// </summary>
        public ConsoleColor SelectionForegroundColor
        {
            get
            {
                if (selectionForegroundColor == null)
                {
                    selectionForegroundColor = defaultValue;
                }
                if (selectionForegroundColorReference == null)
                {
                    selectionForegroundColorReference = ParseColor(selectionForegroundColor, BufferCell.defaultSelectionForegroundColor);
                }
                return selectionForegroundColorReference.Value;
            }
        }

        /// <summary>
        /// Selection background color
        /// </summary>
        public ConsoleColor SelectionBackgroundColor
        {
            get
            {
                if (selectionBackgroundColor == null)
                {
                    selectionBackgroundColor = defaultValue;
                }
                if (selectionBackgroundColorReference == null)
                {
                    selectionBackgroundColorReference = ParseColor(selectionBackgroundColor, BufferCell.defaultSelectionBackgroundColor);
                }
                return selectionBackgroundColorReference.Value;
            }
        }

        /// <summary>
        /// Parse color
        /// </summary>
        /// <param name="input">Input</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Parsed color</returns>
        private ConsoleColor ParseColor(string input, ConsoleColor defaultValue)
        {
            ConsoleColor ret;
            if (!(Enum.TryParse(input, out ret)))
            {
                ret = defaultValue;
            }
            return ret;
        }
    }
}
