open Fake
open Fake.Core
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.Core.TargetOperators

open BuildHelpers
open BuildTools

initializeContext()

let publishPath = Path.getFullName "publish"
let srcPath = Path.getFullName "src"
let clientSrcPath = srcPath </> "FelizUseElmishIssue.Client"
let appPublishPath = publishPath </> "app"

// Targets
let clean proj = [ proj </> "bin"; proj </> "obj" ] |> Shell.cleanDirs

Target.create "InstallClient" (fun _ ->
    printfn "Node version:"
    run Tools.node "--version" clientSrcPath
    printfn "Yarn version:"
    run Tools.yarn "--version" clientSrcPath
    run Tools.yarn "install --frozen-lockfile" clientSrcPath
)

Target.create "Run" (fun _ ->
    run Tools.dotnet "fable clean --yes" ""
    Environment.setEnvironVar "ASPNETCORE_ENVIRONMENT" "Development"
    [
        "client", Tools.yarn "start" ""
    ]
    |> runParallel
)

let dependencies = [
    "InstallClient"
        ==> "Run"
]

[<EntryPoint>]
let main args = runOrDefault "Run" args