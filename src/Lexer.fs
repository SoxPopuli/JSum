module internal JSum.Lexer

type Token =
    | OpenBrace
    | CloseBrace
    | OpenBracket
    | CloseBracket
    | Comma
    | Colon
    | String of string
    | Number of double
    | Bool of bool
    | Null
    | Whitespace


let get_token (input: string) = 
    failwith "todo"
