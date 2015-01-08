module Ploeh.Samples.DiamonProperties

open System
open FsCheck
open FsCheck.Xunit
open Ploeh.Samples

type DiamondPropertyAttribute() =
    inherit PropertyAttribute(QuietOnSuccess = true)

[<DiamondProperty>]
let ``Diamond is non-empty`` (letter : char) =
    let actual = Diamond.make letter
    not (String.IsNullOrWhiteSpace actual)