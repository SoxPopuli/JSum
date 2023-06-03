module Program

open Expecto
open Expecto.Flip

open JSum
open JSum.Json.Model

[<Tests>]
let parseTests = 
    testList "Parse Tests" [
        testCase "Simple JSON Parse" <| fun _ ->
            let actual = Json.parse Samples.simpleJson
            let expected = 
                Object [
                    "name", String "Bob"
                    "age", Number 30.0
                    "hobbies", Array [ String "swimming"; String "dancing" ]
                ]
                
            Expect.equal "" expected actual

    ]

[<EntryPoint>]
let main args = 
    runTestsInAssemblyWithCLIArgs [ No_Spinner ] args
