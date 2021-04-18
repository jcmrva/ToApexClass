open System
open System.IO
open System.Linq

type Model =
    { Cfg : Config
      Files : Map<string, string []>
    }


let filesAndContents (cfg:Config) =
    match cfg.Input with
    | Directory d ->
        let opts = EnumerationOptions(RecurseSubdirectories = cfg.Recurse)
        
        Directory.GetFiles(d, "*.cs", opts) 
        |> Array.map (fun fn -> fn, File.ReadAllLines fn)
        |> Map.ofArray
        
    | File f ->
        Map.add f (File.ReadAllLines f) Map.empty


[<EntryPoint>]
let main argv =
    let args = argParser.Parse argv

    let cfg = 
        Config.Default <| args.PostProcessResult(<@Input@>, pathCheck)
        |> fun cfg -> { cfg with Recurse = args.Contains Recurse }

    let model =
        { Cfg = cfg
          Files = filesAndContents cfg
        }

    printfn "%A" model

    0
