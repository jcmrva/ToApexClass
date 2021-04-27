module Apex

// https://developer.salesforce.com/docs/atlas.en-us.apexcode.meta/apexcode/apex_classes.htm

type PlatformType = 
    | Blob
    | Boolean
    | DateTime
    | Decimal
    | Double
    | Enum
    | Integer
    | Long
    | String
    | List of CollectionParam
    | Map of CollectionParam
    | Set of CollectionParam
and CollectionParam =
    | Native of PlatformType
    | Custom of ClassDefinition
and AccessModifier =
    | Private
    | Protected
    | Public
    | Global
and DefinitionModifier =
    | Abstract
    | Virtual
and Sharing =
    | With
    | Without
and ClassDefinition =
    { Name : string
      AccessModifier : AccessModifier
      DefinitionModifier : DefinitionModifier
      Sharing : Sharing
      Properties : ClassProperty list
    }
and ClassProperty =
    { Name : string
      Type : PlatformType
    }

open System.Text.RegularExpressions

let apexNameOrDefault name dflt =
    if System.String.IsNullOrWhiteSpace name then dflt else name

let ensureValidName (name:string) =
    let rep = Regex.Replace (name, "[^\w]", "_")
    let rxm = Regex.Match (rep, "[a-zA-Z]")
    if rxm.Success then
        rep.Substring (rxm.Index, rep.Length - rxm.Index)
    else 
        rep

