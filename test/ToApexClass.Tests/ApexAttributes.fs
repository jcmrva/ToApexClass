module ApexAttributes

open Expecto
open ToApexClass.Attributes
open DotnetAssembly

[<Tests>]
let attrs =
    testList "ApexAttributes" [
        testCase "" <| fun _ ->
            Expect.equal Attr.name "ApexName" ""
    ]
