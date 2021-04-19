[<AutoOpen>]
module rec Config

open Argu
open System
open System.IO

type PathType = | File of string | Directory of string | InvalidPath

type Args =
    | [<MainCommand; Mandatory; First;>] Input of Path:string
    | [<AltCommandLine("-o")>] Output of Path:string
    | [<AltCommandLine("-r")>] Recurse
    | [<AltCommandLine("-x")>] Extension of string
    | [<AltCommandLine("-h")>] Header of string option
    | [<AltCommandLine("-v")>] View
    | [<AltCommandLine("-q")>] Quiet
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

    | Extension _ -> """
Apex file extension.
  Defaults to .cls.
"""

    | Header _ -> """
Include a comment at the top of each Apex file.
  Provide your own text or leave blank to use the default.
"""

    | Recurse -> """
Search all subdirectories.
"""

    | View -> """
Skip creating output files.
  View transformed contents in the console.
"""

    | Quiet -> """
No console output.
  Ignored if View is used.
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

let inputPath path =
    if Directory.Exists path then 
        Directory path
    else if File.Exists path then 
        File path
    else 
        InvalidPath

type Config =
    { Input : PathType
      OutputDir : string
      Recurse : bool
      ApexExtn : string
    }
