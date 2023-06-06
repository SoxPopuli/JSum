module ReaderTests

open JSum.Lexer
open Expecto.Flip

let first (a: 'A, _: 'B) = 
    a

let second (_: 'A, b: 'B) = 
    b

let remainingTest () =
    let mutable reader = Reader.create "a"
    let mutable rem = reader.remaining()

    Expect.equal "Remaining should equal 1" 1 rem

    reader <- Reader.create "abc"
    rem <- reader.remaining()
    Expect.equal "abc rem == 3" 3 rem

    reader <-
        { reader with
            position = reader.position + 1 }

    rem <- reader.remaining()
    Expect.equal "a|bc rem == 2" 2 rem

    reader <-
        { reader with
            position = reader.position + 1 }

    rem <- reader.remaining()
    Expect.equal "ab|c rem == 1" 1 rem

    reader <-
        { reader with
            position = reader.position + 1 }

    rem <- reader.remaining()
    Expect.equal "abc| rem == 0" 0 rem

let peekTest () =
    let reader = Reader.create "abc"

    Expect.equal "peek 1" 
    <| Some "a"
    <| (reader.peek 1)

    Expect.equal "peek 2" 
    <| Some "ab"
    <| (reader.peek 2)

    Expect.equal "peek 3" 
    <| Some "abc"
    <| (reader.peek 3)

    Expect.equal "peek 4" 
    <| None
    <| (reader.peek 4)

let takeTest() =
    let reader = Reader.create "abc"

    Expect.equal "take 1" 
    <| Some "a"
    <| (reader.take 1)

    Expect.equal "take 2" 
    <| Some "b"
    <| (reader.take 1)

    Expect.equal "take 3" 
    <| Some "c"
    <| (reader.take 1)

    Expect.equal "take 4" 
    <| None
    <| (reader.take 1)

