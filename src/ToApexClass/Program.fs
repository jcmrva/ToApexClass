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
        |> Array.map (fun fn -> Path.GetFileName fn, File.ReadAllLines fn)
        |> Map.ofArray        
    | File f ->
        Map.add (Path.GetFileName f) (File.ReadAllLines f) Map.empty
    | _ -> 
        Map.empty

let convert files =
    files

let save path filename contents =
    let p = Path.Combine (path, filename)
    File.WriteAllLines (p, contents)
    ()

[<EntryPoint>]
let main argv =
    let args = argParser.Parse argv

    let cfg = 
        { Recurse = args.Contains Recurse;
          Input = args.GetResult Input |> inputPath;
          OutputDir = args.GetResult (Output, defaultValue = Directory.GetCurrentDirectory ()); 
        }

    if not <| Directory.Exists cfg.OutputDir then 
        Directory.CreateDirectory cfg.OutputDir |> ignore

    let model =
        { Cfg = cfg
          Files = filesAndContents cfg
        }

    //printfn "%A" model

    model.Files
    |> Map.iter (save cfg.OutputDir)

    0
