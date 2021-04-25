module DotnetAssembly

open Mono.Cecil


let getTypes (md:ModuleDefinition) =
    md.Types |> Seq.filter (fun t -> t.IsPublic) |> Seq.toList

let getProperties (td:TypeDefinition) =
    td.Properties |> Seq.toList

let getClassAttributes (td:TypeDefinition) =
    td.CustomAttributes |> Seq.toList

let getPropertyAttributes (pd:PropertyDefinition) =
    pd.CustomAttributes |> Seq.toList


[<RequireQualifiedAccess>]
module Attr =
    open ToApexClass.Attributes

    let placeholder = nameof ApexPlaceholder
    let exclude = nameof ApexExclude
    let defaultval = nameof ApexDefaultValue
    let name = nameof ApexName
