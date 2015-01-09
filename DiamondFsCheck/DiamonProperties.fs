module Ploeh.Samples.DiamonProperties

open System
open FsCheck
open FsCheck.Xunit
open Ploeh.Samples

type Letters =
    static member Char() =
        Arb.Default.Char()
        |> Arb.filter (fun c -> 'A' <= c && c <= 'Z')

type DiamondPropertyAttribute() =
    inherit PropertyAttribute(
        Arbitrary = [| typeof<Letters> |],
        QuietOnSuccess = true)

[<DiamondProperty>]
let ``Diamond is non-empty`` (letter : char) =
    let actual = Diamond.make letter
    not (String.IsNullOrWhiteSpace actual)

let split (x : string) =
    x.Split([| Environment.NewLine |], StringSplitOptions.None)

let trim (x : string) = x.Trim()

[<DiamondProperty>]
let ``First row contains A`` (letter : char) =
    let actual = Diamond.make letter

    let rows = split actual
    rows |> Seq.head |> trim = "A"

[<DiamondProperty>]
let ``Last row contains A`` (letter : char) =
    let actual = Diamond.make letter

    let rows = split actual
    rows |> Seq.last |> trim = "A"

let leadingSpaces (x : string) =
    let indexOfNonSpace = x.IndexOfAny [| 'A' .. 'Z' |]
    x.Substring(0, indexOfNonSpace)

let trailingSpaces (x : string) =
    let lastIndexOfNonSpace = x.LastIndexOfAny [| 'A' .. 'Z' |]
    x.Substring(lastIndexOfNonSpace + 1)

[<DiamondProperty>]
let ``All rows must have a symmetric contour`` (letter : char) =
    let actual = Diamond.make letter
    
    let rows = split actual
    rows |> Array.forall (fun r -> (leadingSpaces r) = (trailingSpaces r))

[<DiamondProperty>]
let ``Rows must contain the correct letters, in the correct order``
    (letter : char) =
    
    let actual = Diamond.make letter

    let letters = ['A' .. letter]
    let expectedLetters =
        letters @ (letters |> List.rev |> List.tail) |> List.toArray
    let rows = split actual
    expectedLetters = (rows |> Array.map trim |> Array.map Seq.head)

[<DiamondProperty>]
let ``Diamond is as wide as it's high`` (letter : char) =
    let actual = Diamond.make letter

    let rows = split actual
    let expected = rows.Length
    rows |> Array.forall (fun x -> x.Length = expected)