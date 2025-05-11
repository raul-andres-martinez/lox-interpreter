using Lox.Enums;
using Lox.Models;
using Xunit;

namespace Lox.Tests.Models
{
    public class ScannerTests
    {
        [Fact]
        public void Scan_SimpleTokens_ReturnsExpectedTokens()
        {
            var scanner = new Scanner("()");
            var tokens = scanner.ScanTokens();
            Assert.Collection(tokens,
                t => Assert.Equal(TokenType.LeftParen, t.Type),
                t => Assert.Equal(TokenType.RightParen, t.Type),
                t => Assert.Equal(TokenType.EndOfFile, t.Type)
            );
        }

        [Fact]
        public void Scan_IdentifierAndKeyword()
        {
            var scanner = new Scanner("print foo");
            var tokens = scanner.ScanTokens();

            Assert.Collection(tokens,
                t => Assert.Equal(TokenType.Print, t.Type),
                t => {
                    Assert.Equal(TokenType.Identifier, t.Type);
                    Assert.Equal("foo", t.Lexeme);
                },
                t => Assert.Equal(TokenType.EndOfFile, t.Type)
            );
        }

        [Fact]
        public void Scan_StringLiteral()
        {
            var scanner = new Scanner("\"hello\"");
            var tokens = scanner.ScanTokens();

            Assert.Collection(tokens,
            t =>
            {
                Assert.Equal(TokenType.String, t.Type);
                Assert.Equal("hello", t.Literal);
            },
                t => Assert.Equal(TokenType.EndOfFile, t.Type)
            );
        }

        [Fact]
        public void Scan_NumberLiteral()
        {
            var scanner = new Scanner("123.45");
            var tokens = scanner.ScanTokens();

            Assert.Collection(tokens,
                t =>
                {
                    Assert.Equal(TokenType.Number, t.Type);
                    Assert.Equal(123.45, (double)t.Literal!);
                },
                t => Assert.Equal(TokenType.EndOfFile, t.Type)
            );
        }

        [Fact]
        public void Scan_BlockCommentWithNesting()
        {
            var scanner = new Scanner("/* outer /* inner */ still outer */ var x = 1;");
            var tokens = scanner.ScanTokens();

            Assert.Contains(tokens, t => t.Type == TokenType.Var);
            Assert.Contains(tokens, t => t.Type == TokenType.Identifier && t.Lexeme == "x");
            Assert.Contains(tokens, t => t.Type == TokenType.Equal);
            Assert.Contains(tokens, t => t.Type == TokenType.Number);
        }
    }
}