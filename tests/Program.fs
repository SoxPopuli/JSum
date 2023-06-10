module Program

open Expecto
open Expecto.Flip

open JSum
open JSum.Json.Model

[<Tests>]
let lexerTests =
    testList
        "Lexer Tests"
        [ testCase "Simple Lexer Test"
          <| fun _ ->
              let input = "{ \"a\": { \"b\": \"3.0\" }, [2e+4], true, false, null }"
              let tokens = Lexer.getTokens input

              Expect.equal
                  "Lexer output should be:"
                  (Ok
                      [| Lexer.OpenBrace
                         Lexer.String "a"
                         Lexer.Colon

                         Lexer.OpenBrace
                         Lexer.String "b"
                         Lexer.Colon
                         Lexer.String "3.0"
                         Lexer.CloseBrace

                         Lexer.Comma
                         Lexer.OpenBracket
                         Lexer.Number 20_000.0
                         Lexer.CloseBracket
                         Lexer.Comma
                         Lexer.Bool true
                         Lexer.Comma
                         Lexer.Bool false
                         Lexer.Comma
                         Lexer.Null
                         Lexer.CloseBrace |])
                  tokens

          ]

// [<Tests>]
let parseTests = testList "Parse Tests" []

[<EntryPoint>]
let main args =
    runTestsInAssemblyWithCLIArgs [ No_Spinner ] args
