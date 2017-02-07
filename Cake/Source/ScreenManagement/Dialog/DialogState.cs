using System;
using System.Collections.Generic;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// A screen view state that stores widget values for a dialog.
    /// </summary>
    public class DialogState
    {
        private readonly Dictionary<string, float> floatStates;

        private readonly Dictionary<string, int> intStates;

        private readonly Dictionary<string, bool> boolStates;

        private readonly Dictionary<string, string> stringStates;


        public DialogState()
        {
            floatStates = new Dictionary<string, float>();
            intStates = new Dictionary<string, int>();
            boolStates = new Dictionary<string, bool>();
            stringStates = new Dictionary<string, string>();
        }

        /// <summary>
        /// Adds a float state to the ScreenViewState.
        /// </summary>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        /// illegal values. </exception>
        /// <param name="identifier">   The identifier. </param>
        /// <param name="value">    The value. </param>
        public DialogState AddFloatState(string identifier, float value)
        {
            if (!floatStates.ContainsKey(identifier))
            {
                floatStates.Add(identifier, value);
            }
            else
            {
                throw new ArgumentException("A state for this element has already been added. (" + identifier + ")", "identifier");
            }
            return this;
        }

        /// <summary>
        /// Adds an int state to the ScreenViewState.
        /// </summary>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        /// illegal values. </exception>
        /// <param name="identifier">   The identifier. </param>
        /// <param name="value">    The value. </param>
        public DialogState AddIntState(string identifier, int value)
        {
            if (!intStates.ContainsKey(identifier))
            {
                intStates.Add(identifier, value);
            }
            else
            {
                throw new ArgumentException("A state for this element has already been added. (" + identifier + ")", "identifier");
            }
            return this;
        }

        /// <summary>
        /// Adds a bool state to the ScreenViewState..
        /// </summary>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        /// illegal values. </exception>
        /// <param name="identifier">   The identifier. </param>
        /// <param name="value">    The value. </param>
        public DialogState AddBoolState(string identifier, bool value)
        {
            if (!boolStates.ContainsKey(identifier))
            {
                boolStates.Add(identifier, value);
            }
            else
            {
                throw new ArgumentException("A state for this element has already been added. (" + identifier + ")", "identifier");
            }
            return this;
        }

        /// <summary>
        /// Adds a string state to the ScreenViewState.
        /// </summary>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        /// illegal values. </exception>
        /// <param name="identifier">   The identifier. </param>
        /// <param name="value">    The value. </param>
        public DialogState AddStringState(string identifier, string value)
        {
            if (!stringStates.ContainsKey(identifier))
            {
                stringStates.Add(identifier, value);
            }
            else
            {
                throw new ArgumentException("A state for this element has already been added. (" + identifier + ")", "identifier");
            }
            return this;
        }

        /// <summary>
        /// Adds an int state to the ScreenViewState.
        /// </summary>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        /// illegal values. </exception>
        /// <param name="identifier"> The identifier. </param>
        /// <param name="value"> The value. </param>
        public DialogState AddIntState(string identifier, bool value)
        {
            if (!intStates.ContainsKey(identifier))
            {
                boolStates.Add(identifier, value);
            }
            else
            {
                throw new ArgumentException("A state for this element has already been added. (" + identifier + ")", "identifier");
            }
            return this;
        }

        /// <summary>
        /// Reads a float state of the ScreenViewState.
        /// </summary>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        /// illegal values. </exception>
        /// <param name="identifier">   The identifier. </param>
        /// <returns> The float state. </returns>
        public float ReadFloatState(string identifier)
        {
            float value;

            if (!floatStates.TryGetValue(identifier, out value))
            {
                throw new ArgumentException("A state for this element has not been added. (" + identifier + ")", "identifier");
            }

            return value;
        }

        /// <summary>
        /// Reads an int of the ScreenViewState.
        /// </summary>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        /// illegal values. </exception>
        /// <param name="identifier">   The identifier. </param>
        /// <returns> The int state. </returns>
        public int ReadIntState(string identifier)
        {
            int value;

            if (!intStates.TryGetValue(identifier, out value))
            {
                throw new ArgumentException("A state for this element has not been added. (" + identifier + ")", "identifier");
            }

            return value;
        }

        /// <summary>
        /// Reads a bool of the ScreenViewState.
        /// </summary>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        /// illegal values. </exception>
        /// <param name="identifier">   The identifier. </param>
        /// <returns>  true if it succeeds, false if it fails. </returns>
        public bool ReadBoolState(string identifier)
        {
            bool value;

            if (!boolStates.TryGetValue(identifier, out value))
            {
                throw new ArgumentException("A state for this element has not been added. (" + identifier + ")", "identifier");
            }

            return value;
        }

        /// <summary>
        /// Reads a string of the ScreenViewState.
        /// </summary>
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        /// illegal values. </exception>
        /// <param name="identifier"> The identifier. </param>
        /// <returns>  The string state.  </returns>
        public string ReadStringState(string identifier)
        {
            string value;

            if (!stringStates.TryGetValue(identifier, out value))
            {
                throw new ArgumentException("A state for this element has not been added. (" + identifier + ")", "identifier");
            }

            return value;
        }
        
    }
}