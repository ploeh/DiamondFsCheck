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

[<DiamondProperty>]
let ``First row contains A`` (letter : char) =
    let actual = Diamond.make letter

    let rows = split actual
    rows |> Seq.head |> Seq.exists ((=) 'A')

[<DiamondProperty>]
let ``Last row contains A`` (letter : char) =
    let actual = Diamond.make letter

    let rows = split actual
    rows |> Seq.last |> Seq.exists ((=) 'A')