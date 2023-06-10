module internal JSum.Lexer

open System.Collections.Generic

type internal Token =
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
    | Eof

let private tryParseDouble (input: string) : double option =
    try
        Some <| System.Double.Parse input
    with _ ->
        None

let private digitChars =
    HashSet [ '0'; '1'; '2'; '3'; '4'; '5'; '6'; '7'; '8'; '9'; '.'; 'e'; 'E'; '+' ]

let private getString (chars: char array) (i: int) =
    let rec loop (chars: char array) (i: int) (s: System.Text.StringBuilder) =
        match chars[i] with
        | '"' -> Some(i + 1, s.ToString())
        | '\000' -> None
        | ch ->
            let struct (ch, increment) =
                if ch = '\\' then //Escape next character
                    struct (chars[i + 1], 2)
                else
                    struct (ch, 1)

            s.Append ch |> ignore
            loop chars (i + increment) s

    loop chars i (System.Text.StringBuilder())

let private getNumber (chars: char array) (i: int) =
    let rec loop (chars: char array) (i: int) (s: System.Text.StringBuilder) =
        match chars[i] with
        | c when digitChars.Contains c ->
            s.Append c |> ignore
            loop chars (i + 1) s
        | _ -> (i, s.ToString())

    loop chars i (System.Text.StringBuilder())

let private peekNext (chars: char array) (i: int) (expected: char array) ifTrue ifFalse =
    let slice = chars[i .. i + (expected.Length - 1)]

    if slice = expected then
        ifTrue slice
    else
        let rest = Array.fold (fun acc s -> acc + s.ToString()) "" slice
        ifFalse rest

let internal getTokens (input: string) =
    //Null terminate char array to check if array bounds exceeded
    let chars = (input + '\000'.ToString()).ToCharArray()

    // returns new index + token
    let rec get (chars: char array) (i: int) : Result<(int * Token), string> =
        match chars[i] with
        | ' '
        | '\n'
        | '\r'
        | '\t' ->
            //Skip whitespace
            get chars (i + 1)

        | '\000' -> Ok(-1, Eof)
        | '{' -> Ok(i + 1, OpenBrace)
        | '}' -> Ok(i + 1, CloseBrace)
        | '[' -> Ok(i + 1, OpenBracket)
        | ']' -> Ok(i + 1, CloseBracket)
        | ',' -> Ok(i + 1, Comma)
        | ':' -> Ok(i + 1, Colon)
        | 't' ->
            peekNext chars (i + 1) ("rue".ToCharArray())
            <| fun slice -> Ok(i + slice.Length + 1, Bool true)
            <| fun slice -> Error $"Unexpected literal t{slice} at {i}"
        | 'f' ->
            peekNext chars (i + 1) ("alse".ToCharArray())
            <| fun slice -> Ok(i + slice.Length + 1, Bool false)
            <| fun slice -> Error $"Unexpected literal f{slice} at {i}"
        | 'n' ->
            peekNext chars (i + 1) ("ull".ToCharArray())
            <| fun slice -> Ok(i + slice.Length + 1, Null)
            <| fun slice -> Error $"Unexpected literal n{slice} at {i}"
        | '"' ->
            match getString chars (i + 1) with
            | Some(j, s) -> Ok(j, String s)
            | None -> Error $"Unclosed string literal starting at {i}"
        | c when digitChars.Contains c ->
            let j, s = getNumber chars i

            match tryParseDouble s with
            | Some d -> Ok(j, Number d)
            | None -> Error $"Could not convert {s} to number"
        | c -> Error $"Unexpected character {c} at position {i}"


    let rec loop (chars: char array) (i: int) (tokens: Token list) =
        if i = chars.Length then
            Ok tokens
        else
            let token = get chars i

            match token with
            | Error e -> Error e
            | Ok(j, t) ->
                match t with
                | Eof -> Ok tokens
                | _ -> loop chars j (t :: tokens)


    loop chars 0 [] |> Result.map (fun l -> List.rev l |> List.toArray)
