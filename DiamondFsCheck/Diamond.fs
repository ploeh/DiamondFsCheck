module Ploeh.Samples.Diamond

open System

let make letter =
    let makeLine width letter =
        match letter with
        | 'A' ->
            let padding = String(' ', (width - 1) / 2)
            sprintf "%s%c%s" padding letter padding
        | _ -> String(letter, width)

    let letters = ['A' .. letter]
    let letters = letters @ (letters |> List.rev |> List.tail)

    let width = letters.Length

    letters
    |> List.map (makeLine width)
    |> List.reduce (fun x y -> sprintf "%s%s%s" x Environment.NewLine y)