module DotnetAssembly

open Mono.Cecil


let getTypes (md:ModuleDefinition) =
    md.Types 
    |> Seq.filter (fun t -> t.IsPublic) 
    |> Seq.toList

let getProperties (td:TypeDefinition) =
    td.Properties
    |> Seq.toList

[<RequireQualifiedAccess>]
module Attr =
    open ToApexClass.Attributes

    let placeholder = nameof ApexPlaceholder
    let exclude = nameof ApexExclude
    let defaultval = nameof ApexDefaultValue
    let name = nameof ApexName
