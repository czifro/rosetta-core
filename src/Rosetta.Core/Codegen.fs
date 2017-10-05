namespace Rosetta.Core

  type CodegenResult = CodegenResult of IndentedStringWriter

  type Codegen = Codegen of (CodegenResult -> CodegenResult)

  [<RequireQualifiedAccessAttribute>]
  [<CompilationRepresentationAttribute(CompilationRepresentationFlags.ModuleSuffix)>]
  module Codegen =

    let defaultIndentSize = 4

    let init _ = Codegen (id) // val id : x:'a -> 'a

    let bind c f =
      Codegen(fun c' ->
        let (Codegen c) = c
        let (Codegen f) = f

        f (c c')
      )

    let newLine c =
      Codegen(fun c' ->
        let (Codegen c) = c
        let (CodegenResult c') = c c'

        c'.WriteLine()
        (CodegenResult c')
      )

    let useIndent c f =
      Codegen(fun c' ->
        let (CodegenResult c') = c'
        let (Codegen f) = f

        use indent = c'.Indent()
        f (CodegenResult c')
      ) |> bind c

    let append c (s:string) =
      Codegen(fun c' ->
        let (Codegen c) = c
        let (CodegenResult c') = c c'

        c'.Write s
        (CodegenResult c')
      )

    let appendLine c s =
      (append c s) |> newLine

    let resultUsingIndentSize c indentSize =
      let (Codegen c) = c
      let (CodegenResult c') = 
        c (CodegenResult (new IndentedStringWriter(indentSize)))
      c'.ToString()

    let result c = resultUsingIndentSize c defaultIndentSize