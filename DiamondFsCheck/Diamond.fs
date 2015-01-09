module Ploeh.Samples.Diamond

open System

let make letter =
    let makeLine width (letter, letterIndex) =
        match letter with
        | 'A' ->
            let padding = String(' ', (width - 1) / 2)
            sprintf "%s%c%s" padding letter padding
        | _ -> 
            let innerSpaceWidth = letterIndex * 2 - 1
            let padding = String(' ', (width - 2 - innerSpaceWidth) / 2)
            let innerSpace = String(' ', innerSpaceWidth)
            sprintf "%s%c%s%c%s" padding letter innerSpace letter padding

    let indexedLetters =
        ['A' .. letter] |> Seq.mapi (fun i l -> l, i) |> Seq.toList
    let indexedLetters = 
        (   indexedLetters
            |> List.map (fun (l, _) -> l, 1)
            |> List.rev
            |> List.tail
            |> List.rev)
        @ (indexedLetters |> List.rev)

    let width = indexedLetters.Length

    indexedLetters
    |> List.map (makeLine width)
    |> List.reduce (fun x y -> sprintf "%s%s%s" x Environment.NewLine y)