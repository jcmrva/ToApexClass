module Config

open Expecto
open Config

[<Tests>]
let config =
    let cfg1 =
        List.fold updateConfig Config.Zero 
            [ View; Quiet; Header None;  ]

    testList "Config" [
        testCase "Quiet flag ignored when View is used" <| fun _ ->
            Expect.isFalse cfg1.Quiet "ignored"
        testCase "Blank header uses default" <| fun _ ->
            Expect.equal cfg1.Header defaultHeader "default"
    ]