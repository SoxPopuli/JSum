module Program

open Expecto
open Expecto.Flip

open JSum
open JSum.Json.Model

[<Tests>]
let readerTests =
    testList
        "Reader Tests"
        [ testCase "Remaining Test" ReaderTests.remainingTest
          testCase "Peek Test" ReaderTests.peekTest
          testCase "Take Test" ReaderTests.takeTest

          ]

[<Tests>]
let lexerTests =
    testList
        "Lexer Tests"
        [ testCase "Simple Lexer Test"
          <| fun _ ->
              let input = """{ "a": "b", [2], true, false, null }"""
              let tokens = 
                Lexer.getTokens input 
                |> Seq.takeWhile (fun token -> 
                  match token with
                  | Lexer.Error _ -> false
                  | _ -> true)
                |> Seq.toList

              printf $"%A{tokens}"

              Expect.equal
                  "Lexer output should be:"
                  [ Lexer.OpenBrace
                    Lexer.Whitespace
                    Lexer.String "a"
                    Lexer.Colon
                    Lexer.Whitespace
                    Lexer.String "b"
                    Lexer.Comma
                    Lexer.Whitespace
                    Lexer.OpenBracket
                    Lexer.Number 2.0
                    Lexer.CloseBracket
                    Lexer.Comma
                    Lexer.Whitespace
                    Lexer.Bool true
                    Lexer.Comma
                    Lexer.Whitespace
                    Lexer.Bool false
                    Lexer.Comma
                    Lexer.Whitespace
                    Lexer.Null
                    Lexer.Whitespace
                    Lexer.CloseBrace 
                  ]
                  tokens

          ]

[<Tests>]
let parseTests =
    testList
        "Parse Tests"
        [ testCase "Simple JSON Parse"
          <| fun _ ->
              let actual = Json.parse Samples.simpleJson

              let expected =
                  Object
                      [ "name", String "Bob"
                        "age", Number 30.0
                        "hobbies", Array [ String "swimming"; String "dancing" ] ]

              Expect.equal "" expected actual

          ]

[<EntryPoint>]
let main args =
    runTestsInAssemblyWithCLIArgs [ No_Spinner ] args
