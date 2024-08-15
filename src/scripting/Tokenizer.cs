namespace Sandbox_Simulator_2024.Scripting;

using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class Tokenizer
{
    private static readonly Dictionary<string, Token.TokenType> keywords = new Dictionary<string, Token.TokenType>
        {
            { "define", Token.TokenType.Keyword },
            { "is", Token.TokenType.Keyword },
            { "has", Token.TokenType.Keyword },
            { "new", Token.TokenType.Keyword },
            { "if", Token.TokenType.Keyword },
            { "then", Token.TokenType.Keyword },
            { "and", Token.TokenType.Keyword },
            { "or", Token.TokenType.Keyword },
            { "not", Token.TokenType.Keyword },
            { "random", Token.TokenType.Keyword },
            { "action", Token.TokenType.Keyword },
            { "includes", Token.TokenType.Keyword },
            { "interface", Token.TokenType.Keyword },
            { "list", Token.TokenType.Keyword },
            { "router", Token.TokenType.Keyword },
            { "host", Token.TokenType.Keyword },
            { "print", Token.TokenType.Keyword },
            { "red", Token.TokenType.Keyword },
            { "green", Token.TokenType.Keyword },
            { "blue", Token.TokenType.Keyword },
            { "cyan", Token.TokenType.Keyword },
            { "yellow", Token.TokenType.Keyword },
            { "magenta", Token.TokenType.Keyword },
            { "white", Token.TokenType.Keyword },
            { "black", Token.TokenType.Keyword },
            { "OnStep", Token.TokenType.Keyword },
            { "OnReceivePacket", Token.TokenType.Keyword },
            { "packet", Token.TokenType.Keyword},
            { "create", Token.TokenType.Keyword },
            { "many", Token.TokenType.Keyword },
            { "true", Token.TokenType.Keyword },
            { "false", Token.TokenType.Keyword },
            { "from", Token.TokenType.Keyword },
            { ">", Token.TokenType.Keyword },
            { "<", Token.TokenType.Keyword },
            { "send", Token.TokenType.Keyword },
            { "hubAndSpoke", Token.TokenType.Keyword },
            { "roll", Token.TokenType.Keyword },
            { "tree", Token.TokenType.Keyword },
            { "connections", Token.TokenType.Keyword },
            { "for", Token.TokenType.Keyword },
            { "bool", Token.TokenType.Keyword },
            { "int", Token.TokenType.Keyword },
            { "Name", Token.TokenType.Keyword },
            { "Roll", Token.TokenType.Keyword },
            { "to", Token.TokenType.Ignored },
            { "of", Token.TokenType.Ignored },
            { "type", Token.TokenType.Ignored },
            { "%", Token.TokenType.Ignored },
            { "on", Token.TokenType.Ignored },            
            { "chance", Token.TokenType.Ignored },
            { "children", Token.TokenType.Ignored },
            { "that", Token.TokenType.Ignored },
            { "an", Token.TokenType.Ignored },
            { "with", Token.TokenType.Ignored },
            
            
            // Add other keywords here
        };

    private static readonly Regex identifierRegex = new Regex(@"^[a-zA-Z_][a-zA-Z0-9_]*");
    private static readonly Regex numberRegex = new Regex(@"^\d+(\.\d+)?");
    private static readonly Regex stringRegex = new Regex("^\"[^\"]*\"");
    private static readonly Regex commentRegex = new Regex(@"^#.*");
    private static readonly Regex operatorRegex = new Regex(@"^[=+\-*/]");
    private static readonly Regex specialCharRegex = new Regex(@"^[\(\)\{\},]");

    public static List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();
        int index = 0;

        while (index < input.Length)
        {
            if (char.IsWhiteSpace(input[index]))
            {
                if (input[index] == '\n')
                {
                    tokens.Add(new Token("\n", Token.TokenType.NewLine));
                }
                else
                {
                    tokens.Add(new Token(input[index].ToString(), Token.TokenType.Whitespace));
                }
                index++;
                continue;
            }

            string remainingInput = input.Substring(index);

            // Match keywords properly without splitting prematurely
            bool keywordMatched = false;
            foreach (var keyword in keywords.Keys)
            {
                if (remainingInput.StartsWith(keyword) &&
                    (remainingInput.Length == keyword.Length || !char.IsLetterOrDigit(remainingInput[keyword.Length])))
                {
                    tokens.Add(new Token(keyword, keywords[keyword]));
                    index += keyword.Length;
                    keywordMatched = true;
                    break;
                }
            }

            if (keywordMatched) continue;

            if (commentRegex.IsMatch(remainingInput))
            {
                string comment = commentRegex.Match(remainingInput).Value;
                tokens.Add(new Token(comment, Token.TokenType.Comment));
                index += comment.Length;
                continue;
            }

            if (identifierRegex.IsMatch(remainingInput))
            {
                string identifier = identifierRegex.Match(remainingInput).Value;
                tokens.Add(new Token(identifier, Token.TokenType.Identifier));
                index += identifier.Length;
                continue;
            }

            if (numberRegex.IsMatch(remainingInput))
            {
                string number = numberRegex.Match(remainingInput).Value;
                tokens.Add(new Token(number, Token.TokenType.Literal));
                index += number.Length;
                continue;
            }

            if (stringRegex.IsMatch(remainingInput))
            {
                string str = stringRegex.Match(remainingInput).Value;
                tokens.Add(new Token(str, Token.TokenType.String));
                index += str.Length;
                continue;
            }

            if (operatorRegex.IsMatch(remainingInput))
            {
                string op = operatorRegex.Match(remainingInput).Value;
                tokens.Add(new Token(op, Token.TokenType.Operator));
                index += op.Length;
                continue;
            }

            if (specialCharRegex.IsMatch(remainingInput))
            {
                string specialChar = specialCharRegex.Match(remainingInput).Value;
                tokens.Add(new Token(specialChar, Token.TokenType.SpecialCharacter));
                index += specialChar.Length;
                continue;
            }

            tokens.Add(new Token(input[index].ToString(), Token.TokenType.Unknown));
            index++;
        }

        return tokens;
    }

}