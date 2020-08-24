using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using RI.Utilities.Collections;
using RI.Utilities.ObjectModel;




namespace RI.Utilities.Text
{
    /// <summary>
    ///     Used to parse and/or build command line strings including executable, parameters, and/or literals.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A command line consists of the following: <c> [executable] [parameter 1 [parameter 2] ... [parameter n]] [literal 1 [literal 2] ... [literal n]]. </c>
    ///     </para>
    ///     <para>
    ///         Example 1: <c> myprogram.exe -name1=value1 literal1 literal2 </c>
    ///     </para>
    ///     <para>
    ///         Example 2: <c> &quot;d:\my folder\my program.exe&quot; -name1=&quot;Some value with whitespace and \&quot;quotes\&quot;&quot; SomeLiteral &quot;Another literal&quot; </c>
    ///     </para>
    ///     <para>
    ///         A command line can have:
    ///         Zero or one executable.
    ///         Zero, one, or multiple parameters.
    ///         Zero, one, or multiple literals.
    ///     </para>
    ///     <para>
    ///         If an executable is used, it is always at the beginning of the command line.
    ///         An executable supports whitespaces when wrapped in quotes.
    ///     </para>
    ///     <para>
    ///         Parameters are always name/value pairs which start with a minus sign and which separates the name and the value with an equal sign.
    ///         A parameter name and value supports whitespaces and quotes when wrapped in quotes, where the quotes need to be escaped.
    ///         Note that parameters can also consist of only the name, e.g. <c> -name </c>, without the equal sign or any value.
    ///     </para>
    ///     <para>
    ///         Literals are just a list of strings (e.g. a list of files).
    ///         A literal supports whitespaces and quotes when wrapped in quotes, where the quotes need to be escaped.
    ///     </para>
    ///     <para>
    ///         When parsing command lines, the order of parameters and literals does not matter, they can be mixed as needed.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    [Serializable,]
    public sealed class CommandLine : ICloneable<CommandLine>, ICloneable, ICopyable<CommandLine>
    {
        #region Constants

        /// <summary>
        ///     The default string comparer used to distinguish parameter names.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The default comparer is <see cref="StringComparer.InvariantCultureIgnoreCase" />.
        ///     </para>
        /// </remarks>
        public static readonly IEqualityComparer<string> DefaultParameterNameComparer = StringComparer.InvariantCultureIgnoreCase;

        #endregion




        #region Static Methods

        /// <summary>
        ///     Parses the command line of the current process.
        /// </summary>
        /// <returns>
        ///     The <see cref="CommandLine" /> created by parsing the command line of the current process.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="DefaultParameterNameComparer" /> is used to distinguish parameter names.
        ///     </para>
        /// </remarks>
        public static CommandLine FromCurrentProcess ()
        {
            return CommandLine.FromCurrentProcess(null);
        }

        /// <summary>
        ///     Parses the command line of the current process.
        /// </summary>
        /// <param name="parameterNameComparer"> A string comparer used to distinguish parameter names (can be null to use <see cref="DefaultParameterNameComparer" />). </param>
        /// <returns>
        ///     The <see cref="CommandLine" /> created by parsing the command line of the current process.
        /// </returns>
        public static CommandLine FromCurrentProcess (IEqualityComparer<string> parameterNameComparer)
        {
            bool containsExecutable = !Environment.GetCommandLineArgs()[0]
                                                  .IsNullOrEmptyOrWhitespace();

            return CommandLine.Parse(Environment.CommandLine, containsExecutable, parameterNameComparer);
        }

        /// <summary>
        ///     Parses a command line string and returns a <see cref="CommandLine" /> object for it.
        /// </summary>
        /// <param name="commandLine"> The command line string to parse. </param>
        /// <param name="startsWithExecutable"> Indicates whether the command line string starts with an executable. </param>
        /// <returns>
        ///     The <see cref="CommandLine" /> created by parsing the command line string.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         No deep error checking is performed on the parsed command line string so any string can be parsed and is considered a valid command line.
        ///         Invalid command lines will result in strange content of the <see cref="CommandLine" />.
        ///     </para>
        ///     <para>
        ///         Because an executable cannot be distinguished from a literal, it must be specified whether the command line string starts with an executable.
        ///         For example, on Windows, a full process command line (e.g. as retrieved using <see cref="Environment.CommandLine" />) usually starts with the executable of the process.
        ///     </para>
        ///     <para>
        ///         <see cref="DefaultParameterNameComparer" /> is used to distinguish parameter names.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="commandLine" /> is null. </exception>
        public static CommandLine Parse (string commandLine, bool startsWithExecutable)
        {
            return CommandLine.Parse(commandLine, startsWithExecutable, null);
        }

        /// <summary>
        ///     Parses a command line string and returns a <see cref="CommandLine" /> object for it.
        /// </summary>
        /// <param name="commandLine"> The command line string to parse. </param>
        /// <param name="startsWithExecutable"> Indicates whether the command line string starts with an executable. </param>
        /// <param name="parameterNameComparer"> A string comparer used to distinguish parameter names (can be null to use <see cref="DefaultParameterNameComparer" />). </param>
        /// <returns>
        ///     The <see cref="CommandLine" /> created by parsing the command line string.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         No deep error checking is performed on the parsed command line string so any string can be parsed and is considered a valid command line.
        ///         Invalid command lines will result in strange content of the <see cref="CommandLine" />.
        ///     </para>
        ///     <para>
        ///         Because an executable cannot be distinguished from a literal, it must be specified whether the command line string starts with an executable.
        ///         For example, on Windows, a full process command line (e.g. as retrieved using <see cref="Environment.CommandLine" />) usually starts with the executable of the process.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="commandLine" /> is null. </exception>
        public static CommandLine Parse (string commandLine, bool startsWithExecutable, IEqualityComparer<string> parameterNameComparer)
        {
            if (commandLine == null)
            {
                throw new ArgumentNullException(nameof(commandLine));
            }

            parameterNameComparer ??= CommandLine.DefaultParameterNameComparer;

            Dictionary<string, List<string>> parameterDictionary = new Dictionary<string, List<string>>(parameterNameComparer);
            List<string> literalList = new List<string>();
            bool firstIsLiteral = false;

            bool eos = false;

            for (int i1 = 0; i1 < commandLine.Length; i1++)
            {
                if (!char.IsWhiteSpace(commandLine[i1]))
                {
                    if (commandLine[i1] == '-')
                    {
                        i1++;

                        if (i1 < commandLine.Length)
                        {
                            bool hasValue = false;
                            string key = null;

                            if (commandLine[i1] == '\"')
                            {
                                i1++;

                                if (i1 < commandLine.Length)
                                {
                                    key = CommandLine.ReadUntil(commandLine, ref i1, out eos, (v, p) => ((v[p] == '=') && (v[p - 1] == '\"') && (v[p - 2] != '\\')) || (char.IsWhiteSpace(v[p]) && (v[p - 1] == '\"') && (v[p - 2] != '\\')));

                                    if (!eos)
                                    {
                                        key = key.Substring(0, key.Length - 1);
                                    }
                                }
                            }
                            else
                            {
                                key = CommandLine.ReadUntil(commandLine, ref i1, out eos, (v, p) => (v[p] == '=') || char.IsWhiteSpace(v[p]));
                                key = key.Trim();
                            }

                            if (i1 < commandLine.Length)
                            {
                                if (commandLine[i1] == '=')
                                {
                                    i1++;
                                    hasValue = true;
                                }
                            }

                            string parameterKeyToAdd = key;
                            string parameterValueToAdd = null;

                            if ((hasValue && !eos) || (key == null))
                            {
                                if (i1 < commandLine.Length)
                                {
                                    string value = null;

                                    if (commandLine[i1] == '\"')
                                    {
                                        i1++;

                                        if (i1 < commandLine.Length)
                                        {
                                            value = CommandLine.ReadUntil(commandLine, ref i1, out eos, (v, p) => (v[p] == '\"') && (v[p - 1] != '\\'));
                                        }
                                    }
                                    else
                                    {
                                        value = CommandLine.ReadUntil(commandLine, ref i1, out eos, (v, p) => char.IsWhiteSpace(v[p]));
                                        value = value.Trim();
                                    }

                                    parameterValueToAdd = value;
                                }
                            }

                            if (parameterKeyToAdd != null)
                            {
                                parameterKeyToAdd = parameterKeyToAdd.Unescape(StringEscapeOptions.DoubleQuote);

                                if (!parameterDictionary.ContainsKey(parameterKeyToAdd))
                                {
                                    parameterDictionary.Add(parameterKeyToAdd, new List<string>());
                                }

                                parameterValueToAdd = parameterValueToAdd == null ? string.Empty : parameterValueToAdd.Unescape(StringEscapeOptions.DoubleQuote);

                                parameterDictionary[parameterKeyToAdd]
                                    .Add(parameterValueToAdd);
                            }
                        }
                    }
                    else if (commandLine[i1] == '\"')
                    {
                        i1++;

                        if (i1 < commandLine.Length)
                        {
                            string literal = CommandLine.ReadUntil(commandLine, ref i1, out eos, (v, p) => (v[p] == '\"') && (v[p - 1] != '\\'));
                            literalList.Add(literal.Unescape(StringEscapeOptions.DoubleQuote));

                            if (parameterDictionary.Count == 0)
                            {
                                firstIsLiteral = true;
                            }
                        }
                    }
                    else
                    {
                        string literal = CommandLine.ReadUntil(commandLine, ref i1, out eos, (v, p) => char.IsWhiteSpace(v[p]));

                        if (!literal.IsEmptyOrWhitespace())
                        {
                            literalList.Add(literal.Unescape(StringEscapeOptions.DoubleQuote)
                                                   .Trim());

                            if (parameterDictionary.Count == 0)
                            {
                                firstIsLiteral = true;
                            }
                        }
                    }
                }
            }

            string executable = null;

            if (startsWithExecutable && firstIsLiteral && (literalList.Count > 0))
            {
                executable = literalList[0];
                literalList.RemoveAt(0);
            }

            CommandLine result = new CommandLine(parameterNameComparer);
            result.Executable = executable;

            foreach (KeyValuePair<string, List<string>> parameter in parameterDictionary)
            {
                result.Parameters.Add(parameter.Key, parameter.Value);
            }

            result.Literals.AddRange(literalList);
            return result;
        }

        private static string ReadUntil (string value, ref int position, out bool eos, Func<string, int, bool> condition)
        {
            StringBuilder result = new StringBuilder();

            while (true)
            {
                if (position >= value.Length)
                {
                    eos = true;
                    return result.ToString();
                }

                if (condition(value, position))
                {
                    eos = false;
                    return result.ToString();
                }

                result.Append(value[position]);

                position++;
            }
        }

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="CommandLine" />.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <see cref="StringComparer" />.<see cref="StringComparer.InvariantCultureIgnoreCase" /> is used to distinguish parameter names.
        ///     </para>
        /// </remarks>
        public CommandLine ()
            : this(null) { }

        /// <summary>
        ///     Creates a new instance of <see cref="CommandLine" />.
        /// </summary>
        /// <param name="parameterNameComparer"> A string comparer used to distinguish parameter names or null if the default string comparer should be used (see <see cref="CommandLine()" />). </param>
        public CommandLine (IEqualityComparer<string> parameterNameComparer)
        {
            this.Executable = null;
            this.ParameterNameComparer = parameterNameComparer ?? CommandLine.DefaultParameterNameComparer;
            this.Parameters = new Dictionary<string, List<string>>(this.ParameterNameComparer);
            this.Literals = new List<string>();
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets or sets the executable of the command line.
        /// </summary>
        /// <value>
        ///     The executable of the command line.
        /// </value>
        public string Executable { get; set; }

        /// <summary>
        ///     Gets the list of literals of the command line.
        /// </summary>
        /// <value>
        ///     The list of literals of the command line.
        /// </value>
        public List<string> Literals { get; }

        /// <summary>
        ///     Gets the used string comparer used to distinguish parameter names.
        /// </summary>
        /// <value>
        ///     The used string comparer used to distinguish parameter names.
        /// </value>
        public IEqualityComparer<string> ParameterNameComparer { get; private set; }

        /// <summary>
        ///     Gets the dictionary of parameters of the command line.
        /// </summary>
        /// <value>
        ///     The dictionary of parameters of the command line.
        /// </value>
        /// <remarks>
        ///     The dictionary keys are the parameter names.
        ///     The dictionary values are lists of parameter values for the associated parameter name.
        ///     Therefore, a parameter name can have multiple parameter values.
        /// </remarks>
        public Dictionary<string, List<string>> Parameters { get; }

        #endregion




        #region Instance Methods

        /// <summary>
        ///     Builds the command line string corresponding to the data of this command line.
        /// </summary>
        /// <returns>
        ///     The built command line string.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If this command line is &quot;empty&quot; (means: contains no executable, parameters, or literals), the resulting command line string is an empty string of zero length.
        ///     </para>
        ///     <para>
        ///         If an executable is used, it is placed at the beginning of the command line string.
        ///         The executable is wrapped in quotes if it contains any whitespaces.
        ///         If the executable is null, a string of zero length, or contains only whitespaces, the executable is omited.
        ///     </para>
        ///     <para>
        ///         Parameters are always added after the executable, before the literals.
        ///         Parameter names and values are wrapped in quotes if they are of zero length, contain any whitespaces, or contain quotes.
        ///         Parameter values which are null are omited.
        ///         If a parameter has no associated values (means: the corresponding list of values in the dictionary contains no values or only null), a single parameter of the corresponding name but without a value is created.
        ///     </para>
        ///     <para>
        ///         Literals are always added after the parameters.
        ///         Literals are wrapped in quotes if they are of zero length, contain any whitespaces, or contain quotes.
        ///         Literals which are null are omited.
        ///     </para>
        /// </remarks>
        public string Build ()
        {
            string executable;

            if (this.Executable == null)
            {
                executable = null;
            }
            else if (this.Executable.IsEmptyOrWhitespace())
            {
                executable = null;
            }
            else
            {
                executable = this.Executable;
            }

            Dictionary<string, List<string>> parameters;

            if (this.Parameters.Count == 0)
            {
                parameters = null;
            }
            else
            {
                parameters = this.Parameters;
            }

            List<string> literals;

            if (this.Literals.Count == 0)
            {
                literals = null;
            }
            else
            {
                literals = this.Literals;
            }

            StringBuilder commandLine = new StringBuilder();

            if (executable != null)
            {
                commandLine.Append(this.BuildValueOrKey(executable));
            }

            if (parameters != null)
            {
                foreach (KeyValuePair<string, List<string>> parameter in parameters)
                {
                    List<string> values = parameter.Value?.Where(x => x != null)
                                                   .ToList();

                    if ((values == null) || (values.Count == 0))
                    {
                        if (commandLine.Length > 0)
                        {
                            commandLine.Append(" ");
                        }

                        commandLine.Append("-");

                        commandLine.Append(this.BuildValueOrKey(parameter.Key));
                    }
                    else
                    {
                        foreach (string value in values)
                        {
                            if (commandLine.Length > 0)
                            {
                                commandLine.Append(" ");
                            }

                            commandLine.Append("-");

                            commandLine.Append(this.BuildValueOrKey(parameter.Key));
                            commandLine.Append("=");
                            commandLine.Append(this.BuildValueOrKey(value));
                        }
                    }
                }
            }

            if (literals != null)
            {
                foreach (string literal in literals)
                {
                    if (literal == null)
                    {
                        continue;
                    }

                    if (commandLine.Length > 0)
                    {
                        commandLine.Append(" ");
                    }

                    commandLine.Append(this.BuildValueOrKey(literal));
                }
            }

            return commandLine.ToString();
        }

        private string BuildValueOrKey (string valueOrKey)
        {
            bool encapsulate = valueOrKey.Contains('\"') || valueOrKey.ContainsWhitespace() || valueOrKey.IsEmptyOrWhitespace();
            valueOrKey = valueOrKey.Replace("\"", "\\\"");

            if (encapsulate)
            {
                valueOrKey = "\"" + valueOrKey + "\"";
            }

            return valueOrKey;
        }

        #endregion




        #region Overrides

        /// <summary>
        ///     Converts this instance of <see cref="CommandLine" /> into a string.
        /// </summary>
        /// <returns>
        ///     The string representation of this <see cref="CommandLine" /> (same as returned by <see cref="Build" />).
        /// </returns>
        public override string ToString ()
        {
            return this.Build();
        }

        #endregion




        #region Interface: ICloneable<CommandLine>

        /// <summary>
        ///     Creates a clone of this command line.
        /// </summary>
        /// <returns>
        ///     The clone of this command line.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The clone will be a full clone of the executable (if available) and all parameters and literals.
        ///         The resulting clones parameter dictionary and literal list are new instances.
        ///     </para>
        ///     <para>
        ///         The <see cref="ParameterNameComparer" /> of the cloned command line is also cloned if it implements <see cref="ICloneable{T}" /> or <see cref="ICloneable" />.
        ///     </para>
        /// </remarks>
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global"),]
        public CommandLine Clone ()
        {
            CommandLine clone = new CommandLine();
            clone.Executable = this.Executable;
            clone.ParameterNameComparer = (this.ParameterNameComparer as ICloneable)?.Clone() as IEqualityComparer<string> ?? this.ParameterNameComparer;

            foreach (KeyValuePair<string, List<string>> parameter in this.Parameters)
            {
                clone.Parameters.Add(parameter.Key, new List<string>(parameter.Value));
            }

            clone.Literals.AddRange(this.Literals);
            return clone;
        }

        /// <inheritdoc cref="CommandLine.Clone" />
        object ICloneable.Clone ()
        {
            return this.Clone();
        }

        #endregion




        #region Interface: ICopyable<CommandLine>

        /// <inheritdoc />
        public void CopyTo (CommandLine other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            other.Executable = this.Executable;

            other.Literals.Clear();
            other.Literals.AddRange(this.Literals);

            other.Parameters.Clear();
            other.Parameters.AddRange(this.Parameters.Select(x => new KeyValuePair<string, List<string>>(x.Key, new List<string>(x.Value))));
        }

        #endregion
    }
}
