open System
open System.IO
open System.Linq

type Model =
    { Cfg : Config
      Files : Map<string, string list>
    }


let filesAndContents (cfg:Config) =
    let filename (f:string) =
        Path.GetFileNameWithoutExtension f 
    match cfg.Input with
    | PathType.Directory d ->
        let opts = EnumerationOptions (RecurseSubdirectories = cfg.Recurse)
        
        Directory.GetFiles(d, "*.cs", opts) 
        |> Array.map (fun fn -> filename fn, File.ReadAllLines fn |> Array.toList)
        |> Map.ofArray
    | PathType.File f ->
        Map.add (filename f) (File.ReadAllLines f |> Array.toList) Map.empty
    | PathType.InvalidPath -> 
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

let convert cfg _ contents =
    let replaced =
        List.map replacements contents
    match cfg.Header with
    | Some h -> (h + "\n")::replaced
    | None -> replaced

let save cfg filename contents =
    let p = Path.Combine (cfg.OutputDir, filename)
    File.WriteAllLines (p + cfg.ApexExtn, contents |> List.toArray)
    ()

[<EntryPoint>]
let main argv =
    let cfg =
        argv |> allParseResults |> toConfig

    let model =
        { Cfg = cfg
          Files = filesAndContents cfg
        }

    let converted =
        model.Files
        |> Map.map (convert cfg)
    
    if cfg.View then
        printfn "%A\n" model.Cfg

        let toDisplay file apex =
            printfn "%s\n" (file + cfg.ApexExtn)

            List.fold (fun s l -> if l = "" then s else s + l + "\n") "" apex
            |> printfn "%s\n" 

        converted
        |> Map.iter toDisplay
        
    else
        converted
        |> Map.iter (save cfg)

    0
