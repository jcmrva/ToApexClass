[<AutoOpen>]
module rec Config

open Argu
open System
open System.IO

type PathType = | File of string | Directory of string

type Args =
    | [<MainCommand; Mandatory; First;>] Input of Path:string
    | [<AltCommandLine("-o")>] Output of Path:string
    with interface IArgParserTemplate with member a.Usage = argHelp a

let argHelp a =
    match a with
    | Input _ -> """
Input path, absolute or relative to the current directory.
  If the path is a directory, all .cs files will be used.
  If no .cs files are found, an error is returned.
"""
    | Output _ -> """
Output directory path.
  If no value is provided, the current directory is used.
"""

let private errHandler =
    ProcessExiter(colorizer = function 
        | ErrorCode.HelpText -> None 
        | ErrorCode.PostProcess -> Some ConsoleColor.DarkYellow
        | _ -> Some ConsoleColor.Red)

let private checkStructure =
#if DEBUG
    true
#else
    false
#endif

let argParser =
    ArgumentParser.Create<Args>(
        programName = "toapexclass", 
        errorHandler = errHandler,
        checkStructure = checkStructure)

let parseResults argv =
    argParser.Parse(argv)

let pathCheck p =
    if Directory.Exists p || File.Exists p then p 
    else failwith $"Input path is not a valid directory or file: {p}"

type Config =
    { Input : PathType
      OutputDir : string
    }
    static member Default path =
        let p = 
            if Directory.Exists path then 
                Directory path
            else 
                if not <| File.Exists path then failwith $"Input path is not a valid directory or file: {path}" // improbable
                File path

        { Input = p
          OutputDir = Directory.GetCurrentDirectory()
        }
