module Ploeh.Samples.DiamondProperties

open System
open Ploeh.Samples
open Xunit.Extensions
open Swensen.Unquote

type Letters () =    
    let letters = seq {'A' .. 'Z'} |> Seq.cast<obj> |> Seq.map (fun x -> [|x|])
    interface seq<obj[]> with
        member this.GetEnumerator () = letters.GetEnumerator()
        member this.GetEnumerator () =
            letters.GetEnumerator() :> Collections.IEnumerator

[<Theory; ClassData(typeof<Letters>)>]
let ``Diamond is non-empty`` (letter : char) =
    let actual = Diamond.make letter
    test <@ not (String.IsNullOrWhiteSpace actual) @>

let split (x : string) =
    x.Split([| Environment.NewLine |], StringSplitOptions.None)

let trim (x : string) = x.Trim()

[<Theory; ClassData(typeof<Letters>)>]
let ``First row contains A`` (letter : char) =
    let actual = Diamond.make letter

    let rows = split actual
    test <@ rows |> Seq.head |> trim = "A" @>

[<Theory; ClassData(typeof<Letters>)>]
let ``Last row contains A`` (letter : char) =
    let actual = Diamond.make letter

    let rows = split actual
    test <@ rows |> Seq.last |> trim = "A" @>

let leadingSpaces (x : string) =
    let indexOfNonSpace = x.IndexOfAny [| 'A' .. 'Z' |]
    x.Substring(0, indexOfNonSpace)

let trailingSpaces (x : string) =
    let lastIndexOfNonSpace = x.LastIndexOfAny [| 'A' .. 'Z' |]
    x.Substring(lastIndexOfNonSpace + 1)

[<Theory; ClassData(typeof<Letters>)>]
let ``All rows must have a symmetric contour`` (letter : char) =
    let actual = Diamond.make letter
    
    let rows = split actual
    test <@ rows
            |> Array.forall (fun r -> (leadingSpaces r) = (trailingSpaces r))
            @>

[<Theory; ClassData(typeof<Letters>)>]
let ``Rows must contain the correct letters, in the correct order``
    (letter : char) =
    
    let actual = Diamond.make letter

    let letters = ['A' .. letter]
    let expectedLetters =
        letters @ (letters |> List.rev |> List.tail) |> List.toArray
    let rows = split actual
    expectedLetters =? (rows |> Array.map trim |> Array.map Seq.head)

[<Theory; ClassData(typeof<Letters>)>]
let ``Diamond is as wide as it's high`` (letter : char) =
    let actual = Diamond.make letter

    let rows = split actual
    let expected = rows.Length
    test <@ rows |> Array.forall (fun x -> x.Length = expected) @>

let isTwoIdenticalElements x =
    let hasIdenticalLetters = x |> Seq.distinct |> Seq.length = 1
    let hasTwoLetters = x |> Seq.length = 2
    hasIdenticalLetters && hasTwoLetters

[<Theory; ClassData(typeof<Letters>)>]
let ``All rows except top and bottom have two identical letters``
    (letter : char) =

    let actual = Diamond.make letter

    let rows = split actual
    test <@ rows
            |> Array.filter (fun x -> not (x.Contains("A")))
            |> Array.map (fun x -> x.Replace(" ", ""))
            |> Array.forall isTwoIdenticalElements @>

[<Theory; ClassData(typeof<Letters>)>]
let ``Lower left space is a triangle`` (letter : char) =
    let actual = Diamond.make letter

    let rows = split actual
    let lowerLeftSpace =
        rows
        |> Seq.skipWhile (fun x -> not (x.Contains(string letter)))
        |> Seq.map leadingSpaces
    let spaceCounts = lowerLeftSpace |> Seq.map (fun x -> x.Length)
    let expected = Seq.initInfinite id
    test <@ spaceCounts
            |> Seq.zip expected
            |> Seq.forall (fun (x, y) -> x = y) @>

[<Theory; ClassData(typeof<Letters>)>]
let ``Figure is symmetric around the horizontal axis`` (letter : char) =
    let actual = Diamond.make letter

    let rows = split actual
    let topRows =
        rows
        |> Seq.takeWhile (fun x -> not (x.Contains(string letter))) 
        |> Seq.toList
    let bottomRows =
        rows
        |> Seq.skipWhile (fun x -> not (x.Contains(string letter)))
        |> Seq.skip 1
        |> Seq.toList
        |> List.rev
    topRows =? bottomRows