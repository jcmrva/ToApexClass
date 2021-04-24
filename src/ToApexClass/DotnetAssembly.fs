module DotnetAssembly

open Mono.Cecil


let getTypes (file:string) =
    let md = ModuleDefinition.ReadModule file
    
    seq { for td in md.Types do if td.IsPublic then yield td }
    |> Seq.toList