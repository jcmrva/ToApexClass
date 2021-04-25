module Config

open Expecto
open Config

let cfgDefault =
    Config.Zero

[<Tests>]
let config1 =
    let cfg1 =
        [ View; Quiet; Header None; Extension " " ] |> toConfig

    testList "Config" [
        testCase "Quiet flag ignored when View is used." <| fun _ ->
            Expect.isFalse cfg1.Quiet "ignored"
        testCase "Header arg with no value uses default text." <| fun _ ->
            Expect.isSome cfg1.Header ""
        testCase "Empty extension uses default." <| fun _ ->
            Expect.equal cfg1.ApexExtn cfgDefault.ApexExtn ""
    ]

[<Tests>]
let config2 =
    let cfg2 =
        [ ] |> toConfig

    testList "Config_no_args" [
        testCase "No header arg, no header text." <| fun _ -> 
            Expect.isNone cfg2.Header ""
    ]

[<Tests>]
let config3  =
    let cfg3 =
        [ Input "Z:/test" ] |> toConfig

    testList "Config_bad_args" [
        testCase "Bad input path." <| fun _ -> 
            Expect.equal cfg3.Input PathType.InvalidPath ""
    ]