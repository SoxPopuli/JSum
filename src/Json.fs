module JSum.Json

module Model =
    type Json =
        | Null
        | Bool of bool
        | Number of double
        | String of string
        | Array of Json list
        | Object of (string * Json) list

open Model

let parse (input: string): Json =
    failwith "todo"
