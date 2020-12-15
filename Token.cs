using System;
using System.Collections.Generic;
using System.Text;

namespace Natrix_Digital
{

    public enum TokenType
    {
        Identifier,
        Separator,
        Literal,
        Operator,
        Keyword
    };

    class Token 
    {
        public string tokenString;
        public string tokenValue;

        public TokenType tokenType;

    }

    class Identifier : Token
    {
        public Identifier(string value)
        {
            tokenString = "[identifier('" + value + "')]";
            tokenValue = value;
            tokenType = TokenType.Identifier;
        }
    }

    class Separator : Token
    {
        public Separator(string value)
        {
            tokenString = "[separator('" + value + "')]";
            tokenValue = value;
            tokenType = TokenType.Separator;
        }
    }

    class Keyword : Token
    {
        public Keyword(string value)
        {
            tokenString = "[Keyword('" + value + "')]";
            tokenValue = value;
            tokenType = TokenType.Keyword;
        }
    }

    class Operator : Token
    {
        public Operator(string value)
        {
            tokenString = "[operator('" + value + "')]";
            tokenValue = value;
            tokenType = TokenType.Operator;
        }
    }

    class Literal : Token
    {
        public Literal(string value)
        {
            tokenString = "[literal('" + value + "')]";
            tokenValue = value;
            tokenType = TokenType.Literal;
        }
    }
}
