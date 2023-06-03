module JSum.Lexer

open System.Collections.Generic

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
    | Eof
    | Error of string

type Reader =
    { input: string
      mutable position: int

    }

    static member create input = { input = input; position = 0 }

    //Characters left to read from input
    member this.remaining() = this.input.Length - this.position

    member this.getChar() =
        if this.remaining () > 0 then
            let c = this.input[this.position]
            this.position <- this.position + 1
            Some c
        else
            None

    member this.peek(n: int) =
        let rem = this.remaining ()

        if n > rem then
            None
        else
            let pos = this.position
            let s = this.input[pos .. pos + n - 1]
            Some s

    member this.take(n: int) =
        let rem = this.remaining ()

        if n > rem then
            None
        else
            let pos = this.position
            let s = this.input[pos .. pos + n - 1]

            this.position <- this.position + n
            Some s

    member this.skip(n: int) = this.position <- this.position + n

    member this.takeWhile(pred: char -> bool) =
        let rec inner (ch: char option) (pred: char -> bool) =
            seq {
                match ch with
                | Some c ->
                    if not <| pred c then
                        this.position <- this.position - 1
                    else
                        yield c
                        yield! inner (this.getChar ()) pred

                | None -> ()
            }

        inner (this.getChar ()) pred |> Seq.fold (fun acc s -> acc + s.ToString()) ""

let tryParseDouble (input: string) : double option =
    try
        Some <| System.Double.Parse input
    with _ ->
        None

let private digitChars =
    HashSet [ '0'; '1'; '2'; '3'; '4'; '5'; '6'; '7'; '8'; '9'; '.'; 'e'; 'E'; '+' ]

let getTokens (input: string) =
    let reader = Reader.create input

    let rec inner (reader: Reader) (ch: char option) : Token seq =
        seq {
            match ch with
            | None -> ()
            | Some ch ->
                yield
                    match ch with
                    | '{' -> OpenBrace
                    | '}' -> CloseBrace
                    | '[' -> OpenBracket
                    | ']' -> CloseBracket
                    | ',' -> Comma
                    | ':' -> Colon
                    | ' '
                    | '\n'
                    | '\r'
                    | '\t' -> Whitespace //Skip Whitespace

                    | 't' ->
                        let rest = reader.take (3)

                        if rest = Some "rue" then
                            Bool true
                        else
                            Error $"Unexpected literal t{rest} at {reader.position}"

                    | 'f' ->
                        let rest = reader.take (4)

                        if rest = Some "alse" then
                            Bool false
                        else
                            Error $"Unexpected literal f{rest} at {reader.position}"

                    | 'n' ->
                        let rest = reader.take (3)

                        if rest = Some "ull" then
                            Null
                        else
                            Error $"Unexpected literal n{rest} at {reader.position}"

                    | digit when digitChars.Contains ch ->
                        let rest = reader.takeWhile (fun c -> digitChars.Contains c)
                        let num = digit.ToString() + rest

                        match tryParseDouble num with
                        | Some d -> Number d
                        | None -> Error $"Invalid number: {num}"

                    | '"' ->
                        let s = reader.takeWhile (fun c -> c <> '"')
                        reader.skip 1 //Skip closing quote
                        String <| s
                    | c -> Error $"Unexpected character {c} at {reader.position}"

                yield! inner reader (reader.getChar ())
        }

    inner reader (reader.getChar ())
