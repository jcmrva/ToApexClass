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

let replacements (line:string) =
    let replaceIf (orig:string) (replace:string) (txt:string) =
        if txt.Contains($" {orig} ") then txt.Replace(orig, replace) else txt

    let clearStartsWith (orig:string) (txt:string) =
        if txt.StartsWith(orig) then "" else txt

    let clearAttribute (txt:string) =
        if txt.Contains("[") && txt.Contains("]") then "" else txt

    let clearInitializer (txt:string) =
        if txt.Contains("} =") then txt.Substring(0, txt.IndexOf("} =") + 1) else txt

    line
    |> clearStartsWith "using"
    |> clearStartsWith "namespace"
    |> clearStartsWith "{"
    |> clearStartsWith "}"
    |> clearAttribute
    |> clearInitializer
    |> replaceIf "bool" "Boolean"
    |> replaceIf "bool?" "Boolean"
    |> replaceIf "Nullable<bool>" "Boolean"
    |> replaceIf "int" "Integer"
    |> replaceIf "int?" "Integer"
    |> replaceIf "Nullable<int>" "Integer"
    |> replaceIf "DateTime?" "DateTime"
    |> replaceIf "Nullable<DateTime>" "DateTime"

let convert cfg filename contents =
    let replaced =
        Array.map replacements contents

    replaced

let save cfg filename contents =
    let p = Path.Combine (cfg.OutputDir, filename)
    File.WriteAllLines (p + cfg.ApexExtn, contents)
    ()

[<EntryPoint>]
let main argv =
    let args = argParser.Parse argv

    let cfg =        
        args //argParser.Parse argv
        |> fun a -> a.GetAllResults ()
        |> List.fold updateConfig Config.Zero

    let model =
        { Cfg = cfg
          Files = filesAndContents cfg
        }

    let converted =
        model.Files
        |> Map.map (convert cfg)

    if args.Contains View then
        printfn "%A\n" model.Cfg

        let toDisplay file apex =
            printfn "%s\n" (file + cfg.ApexExtn)

            Array.fold (fun s l -> if l = "" then s else s + l + "\n") "" apex
            |> printfn "%s\n" 

        converted
        |> Map.iter toDisplay
        
    else
        converted
        |> Map.iter (save cfg)

    0
