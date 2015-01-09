module Ploeh.Samples.Diamond

let make letter =
    let letters = ['A' .. letter]
    let letters = letters @ (letters |> List.rev |> List.tail)
    letters
    |> List.map string 
    |> List.reduce (fun x y -> sprintf "%s%s%s" x System.Environment.NewLine y)