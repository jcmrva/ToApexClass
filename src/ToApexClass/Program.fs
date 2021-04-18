open System
open System.IO
open System.Linq

type Model =
    { Cfg : Config
      Files : Map<string, string []>
    }


let filesAndContents (cfg:Config) =
    let filename (f:string) =
        Path.GetFileNameWithoutExtension f 
    match cfg.Input with
    | Directory d ->
        let opts = EnumerationOptions(RecurseSubdirectories = cfg.Recurse)
        
        Directory.GetFiles(d, "*.cs", opts) 
        |> Array.map (fun fn -> filename fn, File.ReadAllLines fn)
        |> Map.ofArray        
    | File f ->
        Map.add (filename f) (File.ReadAllLines f) Map.empty
    | _ -> 
        Map.empty

let convert files =
    files

let save path extn filename contents =
    let p = Path.Combine (path, filename)
    File.WriteAllLines (p + extn, contents)
    ()

[<EntryPoint>]
let main argv =
    let args = argParser.Parse argv

    let cfg = 
        { Recurse = args.Contains Recurse;
          Input = args.GetResult Input |> inputPath;
          OutputDir = args.GetResult (Output, defaultValue = Directory.GetCurrentDirectory ()); 
          ApexExtn = args.GetResult (Extension, defaultValue = ".cls"); 
        }

    if not <| Directory.Exists cfg.OutputDir then do
        Directory.CreateDirectory cfg.OutputDir |> ignore

    let model =
        { Cfg = cfg
          Files = filesAndContents cfg
        }

    if args.Contains View then
        printfn "%A" model
    else
        model.Files
        |> Map.iter (save cfg.OutputDir cfg.ApexExtn)

    0
