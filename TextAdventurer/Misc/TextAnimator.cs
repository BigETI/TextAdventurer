using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Text adventurer namespace
/// </summary>
namespace TextAdventurer
{
    /// <summary>
    /// Text animator class
    /// </summary>
    public class TextAnimator
    {
        /// <summary>
        /// Colors regular expression
        /// </summary>
        private static Regex colorsRegex;

        /// <summary>
        /// Text
        /// </summary>
        private string text = string.Empty;

        /// <summary>
        /// Is colored
        /// </summary>
        private bool isColored;

        /// <summary>
        /// Letter delay
        /// </summary>
        private double letterDelay;

        /// <summary>
        /// Number delay
        /// </summary>
        private double numberDelay;

        /// <summary>
        /// Symbol delay
        /// </summary>
        private double symbolDelay;

        /// <summary>
        /// Sequence
        /// </summary>
        private List<Tuple<string, double>> sequence = new List<Tuple<string, double>>();

        /// <summary>
        /// Sequence index
        /// </summary>
        private uint sequenceIndex = 0U;

        /// <summary>
        /// Text builder
        /// </summary>
        private StringBuilder textBuilder = new StringBuilder();

        /// <summary>
        /// Elapsed time
        /// </summary>
        private double elapsedTime;

        /// <summary>
        /// Last date and time
        /// </summary>
        private DateTime lastDateTime;

        /// <summary>
        /// Colors regular expression
        /// </summary>
        private static Regex ColorsRegex
        {
            get
            {
                if (colorsRegex == null)
                {
                    StringBuilder colors_regex_builder = new StringBuilder();
                    string[] console_colors = Enum.GetNames(typeof(ConsoleColor));
                    colors_regex_builder.Append(@"(\[(Default");
                    foreach (string console_color in console_colors)
                    {
                        colors_regex_builder.Append("|");
                        colors_regex_builder.Append(console_color);
                    }
                    colors_regex_builder.Append(@")\]|<(Default");
                    foreach (string console_color in console_colors)
                    {
                        colors_regex_builder.Append("|");
                        colors_regex_builder.Append(console_color);
                    }
                    colors_regex_builder.Append(")>)");
                    colorsRegex = new Regex(colors_regex_builder.ToString());
                    colors_regex_builder.Clear();
                }
                return colorsRegex;
            }
        }

        /// <summary>
        /// Text
        /// </summary>
        public string Text
        {
            get
            {
                if (sequenceIndex < sequence.Count)
                {
                    DateTime now = DateTime.Now;
                    Tuple<string, double> sequence_item = sequence[(int)sequenceIndex];
                    elapsedTime += (now - lastDateTime).TotalSeconds;
                    lastDateTime = now;
                    while ((sequenceIndex < sequence.Count) && (elapsedTime >= sequence_item.Item2))
                    {
                        elapsedTime -= sequence_item.Item2;
                        textBuilder.Append(sequence_item.Item1);
                        ++sequenceIndex;
                        if (sequenceIndex < sequence.Count)
                        {
                            sequence_item = sequence[(int)sequenceIndex];
                        }
                    }
                }
                return textBuilder.ToString();
            }
            set
            {
                if ((value != null) && (value != text))
                {
                    Dictionary<int, int> colors = new Dictionary<int, int>();
                    text = value;
                    sequence.Clear();
                    Reset();
                    if (isColored)
                    {
                        MatchCollection matches = ColorsRegex.Matches(text);
                        if (matches != null)
                        {
                            foreach (Match match in matches)
                            {
                                if (match != null)
                                {
                                    if (colors.ContainsKey(match.Index))
                                    {
                                        colors[match.Index] = match.Length;
                                    }
                                    else
                                    {
                                        colors.Add(match.Index, match.Length);
                                    }
                                }
                            }
                        }
                    }
                    StringBuilder sequence_item_builder = new StringBuilder();
                    for (int i = 0; i < text.Length; i++)
                    {
                        if (colors.ContainsKey(i))
                        {
                            int len = colors[i];
                            sequence_item_builder.Append(text.Substring(i, len));
                            i += len - 1;
                        }
                        else
                        {
                            char character = text[i];
                            sequence_item_builder.Append(character.ToString());
                            sequence.Add(new Tuple<string, double>(sequence_item_builder.ToString(), GetSequenceDelay(character)));
                            sequence_item_builder.Clear();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="isColored">Is colored</param>
        /// <param name="letterDelay">Letter delay</param>
        /// <param name="numberDelay">Number delay</param>
        /// <param name="symbolDelay">Symbol delay</param>
        public TextAnimator(string text, bool isColored, double letterDelay, double numberDelay, double symbolDelay)
        {
            this.isColored = isColored;
            this.letterDelay = letterDelay;
            this.numberDelay = numberDelay;
            this.symbolDelay = symbolDelay;
            lastDateTime = DateTime.Now;
            Text = text;
        }

        /// <summary>
        /// Get sequence delay
        /// </summary>
        /// <param name="character">Character</param>
        /// <returns>Sequence delay</returns>
        private double GetSequenceDelay(char character) => (char.IsDigit(character) ? numberDelay : ((char.IsWhiteSpace(character) || char.IsPunctuation(character) || char.IsSymbol(character)) ? symbolDelay : letterDelay));

        /// <summary>
        /// Reset text animator
        /// </summary>
        public void Reset()
        {
            textBuilder.Clear();
            sequenceIndex = 0U;
            elapsedTime = 0.0;
            lastDateTime = DateTime.Now;
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return Text;
        }
    }
}
