module FelizUseElmishIssue.Client.View

open System
open Feliz
open Router
open Elmish
open Feliz.UseElmish
open Feliz.DaisyUI

type private Msg =
    | Tick of DateTime

type private State = {
    Date : DateTime
}

let private init () =
    { Date = DateTime.MinValue }, Cmd.none

let private update (msg:Msg) (state:State) : State * Cmd<Msg> =
    match msg with
    | Tick d -> { state with Date = d }, Cmd.none

[<ReactComponent>]
let AppView () =
    let state,dispatch = React.useElmish(init, update)

    React.useEffectOnce (fun _ ->
        printfn "Effect called"
        DateTime.Now |> Tick |> dispatch
    )

    Html.div [

        state.Date |> string |> Html.div

        Daisy.button.button [
            button.primary
            prop.text "Update manually - THIS WORKS"
            prop.onClick (fun _ -> DateTime.Now |> Tick |> dispatch)
        ]

    ]
