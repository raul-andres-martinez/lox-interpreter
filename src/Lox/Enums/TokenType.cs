﻿namespace Lox.Enums
{
    public enum TokenType
    {
        // Single-char tokens
        LeftParen,
        RightParen, 
        LeftBracket, 
        RightBracket,
        Comma,
        Dot,
        Minus,
        Plus,
        Semicolon,
        Slash,
        Star,

        // One or two char tokens
        Bang,
        BangEqual,
        Equal,
        EqualEqual,
        Greater,
        GreaterEqual,
        Less,
        LessEqual,

        // Literals
        Identifier,
        String,
        Number,

        // Keywords
        And,
        Class,
        Else,
        False,
        Fun,
        For,
        If,
        Nil,
        Or,
        Print,
        Return,
        Super,
        This,
        True,
        Var,
        While,

        EndOfFile
    }
}