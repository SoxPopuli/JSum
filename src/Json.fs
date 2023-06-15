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
    and RootJson =
        | Null
        | Bool of bool
        | Number of double
        | String of string
        | Array of Json list
        | Object of Json list

open Model

type Result<'O, 'E> with

    member this.map f = Result.map f this
    member this.bind f = Result.bind f this

type Option<'T> with

    member this.map f = Option.map f this

type private TempObject() =
    [<DefaultValue>]
    val mutable key: string option

    [<DefaultValue>]
    val mutable items: List<Json>

    member this.Reset() =
        this.key <- None
        this.items.Clear()

let private parseObject (tokens: Lexer.Token array) (i: int) =
    let rec loop (j: int) =
        failwith "todo"

    failwith "todo"

let private parseTokens (tokens: Lexer.Token array): RootJson =
    let stringBuffer = Stack<string>()
    let objectStack = Stack<TempObject>()

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
            tokens



    failwith "todo"

let parse (input: string) : Result<Json, string> =
    let tokens = Lexer.getTokens input
    Result.map parseTokens tokens
