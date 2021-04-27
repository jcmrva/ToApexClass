module Apex

open Expecto
open Apex

[<Tests>]
let regex =
    testList "Regex" [
        testCase "Replace on-alphanumeric character with underscores" <| fun _ ->
            Expect.equal (ensureValidName "C# Example") "C__Example" ""
        testCase "Name must start with a letter" <| fun _ ->
            Expect.equal (ensureValidName "-123_%+Test**(-)") "Test_____" ""
    ]
