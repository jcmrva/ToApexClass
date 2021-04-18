open System
open System.IO
open System.Linq

type Model =
    { Cfg : Config
      Files : Map<string, string []>
    }


let files (cfg:PathType) =
    match cfg with
    | Directory d ->
        Map.empty
    | File f ->
        Map.add f (File.ReadAllLines f) Map.empty


[<EntryPoint>]
let main argv =
    let args = argParser.Parse argv

    let cfg = Config.Default <| args.PostProcessResult(<@Input@>, pathCheck)

    let model =
        { Cfg = cfg
          Files = files cfg.Input
        }

    printfn "%A" model

    0
    