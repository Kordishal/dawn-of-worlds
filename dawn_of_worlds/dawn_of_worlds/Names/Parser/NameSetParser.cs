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

            foreach (string name_set in name_sets)
            {
                _tokens.Add(new List<Token>(GetTokens(NAME_SET + name_set + ".txt")));
            }           
        }


        public List<NameSet> NameSets { get; set; }

        private NameSetFSM<State> _NameSetFSM { get; set; }
        private void initializeParser()
        {
            _NameSetFSM = new NameSetFSM<State>();
            _NameSetFSM.AddTransition(State.INITIAL_STATE, State.VARIABLE, establishNameSetName);

            _NameSetFSM.AddTransition(State.VARIABLE, State.ASSIGNMENT, doNothing);
            _NameSetFSM.AddTransition(State.VARIABLE, State.CLOSING_CURLY_BRACES, doNothing);
            _NameSetFSM.AddTransition(State.VARIABLE, State.VARIABLE, addName);
            _NameSetFSM.AddTransition(State.VARIABLE, State.STRING, addStringName);

            _NameSetFSM.AddTransition(State.STRING, State.VARIABLE, addName);
            _NameSetFSM.AddTransition(State.STRING, State.STRING, addTemplate);
            _NameSetFSM.AddTransition(State.STRING, State.CLOSING_CURLY_BRACES, doNothing);

            _NameSetFSM.AddTransition(State.ASSIGNMENT, State.OPENING_CURLY_BRACES, doNothing);

            _NameSetFSM.AddTransition(State.OPENING_CURLY_BRACES, State.VARIABLE, addName);
            _NameSetFSM.AddTransition(State.OPENING_CURLY_BRACES, State.STRING, addTemplate);
            _NameSetFSM.AddTransition(State.OPENING_CURLY_BRACES, State.CLOSING_CURLY_BRACES, doNothing);

            _NameSetFSM.AddTransition(State.CLOSING_CURLY_BRACES, State.END_OF_FILE, doNothing);
            _NameSetFSM.AddTransition(State.CLOSING_CURLY_BRACES, State.CLOSING_CURLY_BRACES, doNothing);
            _NameSetFSM.AddTransition(State.CLOSING_CURLY_BRACES, State.VARIABLE, establishNameSetListName);

            _NameSetFSM.AddTransition(State.END_OF_FILE, State.VARIABLE, establishNameSetName);
        }

        private Token _previous_token { get; set; }
        private Token _current_token { get; set; }
        private void parse()
        { 
            foreach (List<Token> token_list in _tokens)
            {
                foreach (Token token in token_list)
                {
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
                        case "(end)":
                            _NameSetFSM.Advance(State.END_OF_FILE);
                            break;
                    }

                    _previous_token = _current_token;
                }
            }

            
        }

        private void doNothing()
        {

        }

        private void establishNameSetName()
        {
            NameSets.Add(new NameSet());
            NameSets.Last().Name = _current_token.Value;
        }

        private void establishNameSetListName()
        {
            NameSets.Last().NameListDescriptions.Add(_current_token.Value);
            NameSets.Last().Names.Add(new List<string>());     
        }

        private void addName()
        {
            if (_current_token.Value != "templates")
                NameSets.Last().Names.Last().Add(_current_token.Value);
        }

        private void addStringName()
        {
            if (_current_token.Value != "templates")
                NameSets.Last().Names.Last().Add(_current_token.Value.Substring(1, _current_token.Value.Length - 2));
        }

        private void addTemplate()
        {
            if (_current_token.Value.Contains("<"))
                NameSets.Last().Templates.Add(_current_token.Value);
            else
                NameSets.Last().Names.Last().Add(_current_token.Value.Substring(1, _current_token.Value.Length - 2));
        }


        public NameSetParser()
        {
            NameSets = new List<NameSet>();
            initLexer();
            generateTokens();
            initializeParser();
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
    }
}
