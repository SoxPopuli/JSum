module JSum.Json

open System.Collections.Generic

module Model =
    type Json =
        | Null
        | Bool of bool
        | Number of double
        | String of string
        | Array of Json list
        | Object of (string * Json) list

open Model

type private TempObject = TempObject of (string * Json) list

let private parseTokens (tokens: Lexer.Token array) =
    let objectStack = Stack<TempObject>()
    let arrayQueue = Queue<Json>()

    let rec loop (tokens: Lexer.Token array) (i: int) =
        match tokens[i] with
        | Lexer.OpenBrace ->
            failwith "todo"
        | Lexer.CloseBrace ->
            failwith "todo"
        | Lexer.OpenBracket ->
            failwith "todo"
        | Lexer.CloseBracket ->
            failwith "todo"
        | Lexer.Comma ->
            failwith "todo"
        | Lexer.Colon ->
            failwith "todo"
        | Lexer.String s ->
            failwith "todo"
        | Lexer.Number n ->
            failwith "todo"
        | Lexer.Bool b ->
            failwith "todo"
        | Lexer.Null ->
            failwith "todo"
        | Lexer.Eof ->
            failwith "todo"


        failwith "todo"



    failwith "todo"

let parse (input: string): Result<Json, string> =
    let tokens = Lexer.getTokens input
    Result.map parseTokens tokens
