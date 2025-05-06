using Lox.Enums;

namespace Lox.Models
{
    public class Token
    {
        public Token(TokenType type, string lexeme, object? literal, int line)
        {
            Type = type;
            Lexeme = lexeme;
            Literal = literal;
            Line = line;
        }

        public TokenType Type { get; set; }
        public string Lexeme { get; set; }
        public object? Literal { get; set; }
        public int Line { get; set; }
    }
}