using Lox.Enums;

namespace Lox.Models
{
    public class Scanner
    {
        public Scanner(string source)
        {
            Source = source;
        }

        private static readonly Dictionary<string, TokenType> _keywords = new()
        {
            ["and"] = TokenType.And,
            ["class"] = TokenType.Class,
            ["else"] = TokenType.Else,
            ["false"] = TokenType.False,
            ["for"] = TokenType.For,
            ["fun"] = TokenType.Fun,
            ["if"] = TokenType.If,
            ["nil"] = TokenType.Nil,
            ["or"] = TokenType.Or,
            ["print"] = TokenType.Print,
            ["return"] = TokenType.Return,
            ["super"] = TokenType.Super,
            ["this"] = TokenType.This,
            ["true"] = TokenType.True,
            ["var"] = TokenType.Var,
            ["while"] = TokenType.While
        };

        public string Source { get; set; }
        public List<Token> Tokens { get; set; } = new List<Token>();
        private int Start = 0;
        private int Current = 0;
        private int Line = 1;

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                Start = Current;
                ScanToken();
            }

            Tokens.Add(new Token(TokenType.EndOfFile, "", null, Line));
            return Tokens;
        }

        private void ScanToken()
        {
            char c = Advance();

            switch (c)
            {
                case '(': AddToken(TokenType.LeftParen); break;
                case ')': AddToken(TokenType.RightParen); break;
                case '{': AddToken(TokenType.LeftBracket); break;
                case '}': AddToken(TokenType.RightBracket); break;
                case ',': AddToken(TokenType.Comma); break;
                case '.': AddToken(TokenType.Dot); break;
                case '-': AddToken(TokenType.Minus); break;
                case '+': AddToken(TokenType.Plus); break;
                case ';': AddToken(TokenType.Semicolon); break;
                case '*': AddToken(TokenType.Star); break;
                case '!':
                    AddToken(Match('=') ? TokenType.BangEqual : TokenType.Bang);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EqualEqual : TokenType.Equal);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.LessEqual : TokenType.Less);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater);
                    break;
                case '/':
                    if (Match('/'))
                    {
                        while (Peek() != '\n' && !IsAtEnd())
                        {
                            Advance();
                        }
                    }
                    else
                    {
                        AddToken(TokenType.Slash); 
                    }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    break;
                case '\n':
                    Line++;
                    break;
                case '"':
                    HandleString();
                    break;
                default:
                    if (IsDigit(c))
                    {
                        HandleNumber();
                    }
                    else if (IsAlpha(c))
                    {
                        HandleIdentifier();
                    }
                    else
                    {
                        ErrorReporter.Error(Line, $"Unexpected character '{c}'.");
                    }
                    break;
            }
        }

        private bool IsAtEnd()
        {
            return Current >= Source.Length;
        }

        private char Advance()
        {
            return Source[Current++];
        }

        private bool Match(char expected)
        {
            if (IsAtEnd())
            {
                return false;
            }
            if (Source[Current] != expected)
            {
                return false;
            }

            Current++;
            return true;
        }

        private char Peek()
        {
            if (IsAtEnd())
            {
                return '\0';
            }

            return Source[Current];
        }

        private char PeekNext()
        {
            if (Current + 1 >= Source.Length)
            {
                return '\0';
            }
            return Source[Current + 1];
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   (c == '_');
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c); 
        }

        private void HandleString()
        {
            while (Peek() != '"' && !IsAtEnd()) 
            {
                if (Peek() == '\n')
                {
                    Line++;
                }
                Advance();
            }

            if (IsAtEnd()) 
            {
                ErrorReporter.Error(Line, "Unterminated string.");
                return;
            }

            Advance();
            string value = Source.Substring(Start + 1, Current - 1);
            AddToken(TokenType.String, value);
        }

        private void HandleNumber()
        {
            while (IsDigit(Peek()))
            {
                Advance();
            }

            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                Advance();

                while (IsDigit(Peek()))
                {
                    Advance();
                }
            }

            AddToken(TokenType.Number, double.Parse(Source.Substring(Start, Current)));
        }

        private void HandleIdentifier()
        {
            while (IsAlphaNumeric(Peek()))
            {
                Advance();
            }

            string text = Source.Substring(Start, Current);
            TokenType type;
            if (!_keywords.TryGetValue(text, out type))
            {
                type = TokenType.Identifier;
            }

            AddToken(type);
        }

        private void AddToken(TokenType type, object? literal = null)
        {
            string text = Source[Start..Current];
            Tokens.Add(new Token(type, text, literal, Line));
        }
    }
}