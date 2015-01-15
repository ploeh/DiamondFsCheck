module Ploeh.Samples.DiamondProperties

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

[<DiamondProperty>]
let ``All rows except top and bottom have two identical letters``
    (letter : char) =

    let actual = Diamond.make letter

    let isTwoIdenticalLetters x =
        let hasIdenticalLetters = x |> Seq.distinct |> Seq.length = 1
        let hasTwoLetters = x |> Seq.length = 2
        hasIdenticalLetters && hasTwoLetters
    let rows = split actual
    rows
    |> Array.filter (fun x -> not (x.Contains("A")))
    |> Array.map (fun x -> x.Replace(" ", ""))
    |> Array.forall isTwoIdenticalLetters

[<DiamondProperty>]
let ``Lower left space is a triangle`` (letter : char) =
    let actual = Diamond.make letter

    let rows = split actual
    let lowerLeftSpace =
        rows
        |> Seq.skipWhile (fun x -> not (x.Contains(string letter)))
        |> Seq.map leadingSpaces
    let spaceCounts = lowerLeftSpace |> Seq.map (fun x -> x.Length)
    let expected = Seq.initInfinite id
    spaceCounts
    |> Seq.zip expected
    |> Seq.forall (fun (x, y) -> x = y)

[<DiamondProperty>]
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
    topRows = bottomRows