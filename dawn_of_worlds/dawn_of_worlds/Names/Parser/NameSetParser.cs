using dawn_of_worlds.Parser.LexerClasses;
using dawn_of_worlds.Parser.TokenClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dawn_of_worlds.Names.Parser
{
    class NameSetParser
    {

        private const string NAME_SET = @"C:\Users\Jonas Waeber\Documents\Projects\dawn_of_worlds\dawn_of_worlds\dawn_of_worlds\Names\NameSets\";


        private Lexer _nameset_lexer { get; set; }
        private void initLexer()
        {
            _nameset_lexer = new Lexer();

            _nameset_lexer.AddDefinition(new TokenDefinition(new Regex(@"[a-zA-Z_\-]+"), "VARIABLE"));

            _nameset_lexer.AddDefinition(new TokenDefinition(new Regex(@"([""'])(?:\\\1|.)*?\1"), "STRING"));
            _nameset_lexer.AddDefinition(new TokenDefinition(new Regex(@"\="), "ASSIGNMENT"));

            _nameset_lexer.AddDefinition(new TokenDefinition(new Regex(@"\{"), "OPENING_CURLY_BRACES"));
            _nameset_lexer.AddDefinition(new TokenDefinition(new Regex(@"\}"), "CLOSING_CURLY_BRACES"));

            _nameset_lexer.AddDefinition(new TokenDefinition(new Regex(@"\,"), "COMMA"));
            _nameset_lexer.AddDefinition(new TokenDefinition(new Regex(@"[0-9]+"), "INTEGER"));
            _nameset_lexer.AddDefinition(new TokenDefinition(new Regex(@"\["), "OPENING_SQUARE_BRACES"));
            _nameset_lexer.AddDefinition(new TokenDefinition(new Regex(@"\]"), "CLOSING_SQUARE_BRACES"));

            _nameset_lexer.AddDefinition(new TokenDefinition(new Regex(@"[#].+[\n]"), "COMMENT", true));
            _nameset_lexer.AddDefinition(new TokenDefinition(new Regex(@"\s+"), "WHITE_SPACE", true));

        }
        private IEnumerable<Token> GetTokens(string path)
        {
            return _nameset_lexer.Tokenize(readTargetFile(path));
        }
        private string readTargetFile(string path)
        {
            string result = "";
            StreamReader reader = new StreamReader(path);

            while (!reader.EndOfStream)
            {
                result += reader.ReadLine() + "\n";
            }

            reader.Close();
            return result;
        }

        private List<List<Token>> _tokens { get; set; }
        private void generateTokens()
        {
            List<string> name_sets = new List<string>();
            StreamReader reader = new StreamReader(NAME_SET + @"00_namesetlist.txt");

            while (!reader.EndOfStream)
            {
                name_sets.Add(reader.ReadLine());
            }

            reader.Close();

            _tokens = new List<List<Token>>();

            NameSets = new LinkedList<NameSet>();
            foreach (string name_set in name_sets)
            {
                NameSets.AddLast(new NameSet(name_set));
                _tokens.Add(new List<Token>(GetTokens(NAME_SET + name_set + ".txt")));
            }
        }


        public LinkedList<NameSet> NameSets { get; set; }
        public LinkedListNode<NameSet> CurrentNameSet { get; set; }

        private NameSetFSM<State> _NameSetFSM { get; set; }
        private void initializeParser()
        {
            _NameSetFSM = new NameSetFSM<State>();
            _NameSetFSM.AddTransition(State.INITIAL_STATE, State.VARIABLE, storeNameListName);

            _NameSetFSM.AddTransition(State.VARIABLE, State.ASSIGNMENT, doNothing);
            _NameSetFSM.AddTransition(State.VARIABLE, State.VARIABLE, addName);
            _NameSetFSM.AddTransition(State.VARIABLE, State.STRING, addName);
            _NameSetFSM.AddTransition(State.VARIABLE, State.CLOSING_CURLY_BRACES, createNameList);

            _NameSetFSM.AddTransition(State.STRING, State.VARIABLE, addName);
            _NameSetFSM.AddTransition(State.STRING, State.STRING, addName);
            _NameSetFSM.AddTransition(State.STRING, State.CLOSING_CURLY_BRACES, createNameList);

            _NameSetFSM.AddTransition(State.ASSIGNMENT, State.OPENING_CURLY_BRACES, doNothing);

            _NameSetFSM.AddTransition(State.OPENING_CURLY_BRACES, State.VARIABLE, addName);
            _NameSetFSM.AddTransition(State.OPENING_CURLY_BRACES, State.STRING, addName);
            _NameSetFSM.AddTransition(State.OPENING_CURLY_BRACES, State.CLOSING_CURLY_BRACES, createNameList);

            _NameSetFSM.AddTransition(State.CLOSING_CURLY_BRACES, State.VARIABLE, storeNameListName);
            _NameSetFSM.AddTransition(State.CLOSING_CURLY_BRACES, State.OPENING_SQUARE_BRACES, createRandomizationList);
            _NameSetFSM.AddTransition(State.CLOSING_CURLY_BRACES, State.END_OF_FILE, nextNameSet);

            _NameSetFSM.AddTransition(State.OPENING_SQUARE_BRACES, State.INTEGER, addInteger);

            _NameSetFSM.AddTransition(State.INTEGER, State.COMMA, doNothing);
            _NameSetFSM.AddTransition(State.INTEGER, State.CLOSING_SQUARE_BRACES, doNothing);

            _NameSetFSM.AddTransition(State.COMMA, State.INTEGER, addInteger);

            _NameSetFSM.AddTransition(State.CLOSING_SQUARE_BRACES, State.VARIABLE, storeNameListName);

            _NameSetFSM.AddTransition(State.END_OF_FILE, State.VARIABLE, storeNameListName);
        }

        private List<int> _randomization { get; set; }
        private string _namelist_name { get; set; }
        private List<string> _names { get; set; }
        private Token _previous_token { get; set; }
        private Token _current_token { get; set; }
        private void parse()
        {
            foreach (List<Token> token_list in _tokens)
            {
                foreach (Token token in token_list)
                {
                    _previous_token = _current_token;
                    _current_token = token;
                    switch (token.Type)
                    {
                        case "VARIABLE":
                            _NameSetFSM.Advance(State.VARIABLE);
                            break;
                        case "STRING":
                            _NameSetFSM.Advance(State.STRING);
                            break;
                        case "ASSIGNMENT":
                            _NameSetFSM.Advance(State.ASSIGNMENT);
                            break;
                        case "OPENING_CURLY_BRACES":
                            _NameSetFSM.Advance(State.OPENING_CURLY_BRACES);
                            break;
                        case "CLOSING_CURLY_BRACES":
                            _NameSetFSM.Advance(State.CLOSING_CURLY_BRACES);
                            break;
                        case "OPENING_SQUARE_BRACES":
                            _NameSetFSM.Advance(State.OPENING_SQUARE_BRACES);
                            break;
                        case "CLOSING_SQUARE_BRACES":
                            _NameSetFSM.Advance(State.CLOSING_SQUARE_BRACES);
                            break;
                        case "COMMA":
                            _NameSetFSM.Advance(State.COMMA);
                            break;
                        case "INTEGER":
                            _NameSetFSM.Advance(State.INTEGER);
                            break;
                        case "(end)":
                            _NameSetFSM.Advance(State.END_OF_FILE);
                            break;
                    }
                }
            }           
        }

        private void doNothing() { }
        private void nextNameSet()
        {
            CurrentNameSet = CurrentNameSet.Next;
        }
        private void storeNameListName()
        {
            _namelist_name = _current_token.Value;
            _names = new List<string>();                    
        }

        private void createRandomizationList()
        {
            _randomization = new List<int>();
        }

        private void addName()
        {
            string value = _current_token.Value;
            if (value[0] == '"')
                value = value.Substring(1, value.Length - 2);
            _names.Add(value);
        }

        private void addInteger()
        {
            _randomization.Add(int.Parse(_current_token.Value));
        }

        private void createNameList()
        {
            CurrentNameSet.Value.Names.Add(_namelist_name, new Pair<List<int>, List<string>>(_randomization, _names));
            _names = null;
            _randomization = null;
        }


        public NameSetParser()
        {
            initLexer();
            generateTokens();
            initializeParser();
            CurrentNameSet = NameSets.First;
            _randomization = null;
            parse();
        }


    }


    enum State
    {
        INITIAL_STATE,
        END_OF_FILE,
        VARIABLE,
        STRING,
        ASSIGNMENT,
        OPENING_CURLY_BRACES,
        CLOSING_CURLY_BRACES,
        COMMA,
        INTEGER,
        OPENING_SQUARE_BRACES,
        CLOSING_SQUARE_BRACES
    }
}
