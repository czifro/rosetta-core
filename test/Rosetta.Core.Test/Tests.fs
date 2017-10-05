namespace Rosetta.Core.Test

  module Tests =

    open System
    open Xunit
    open Rosetta.Core

    [<Fact>]
    let ``Should generate simple fs binding`` () =
      let expected = "let hello = sprintfn \"Hello World!\""
      let code =
        codegen {
          append "let hello = sprintfn \"Hello World!\""
        }
      let actual = Codegen.result code
      Assert.Equal(expected,actual)

    [<Fact>]
    let ``Should generate simple cs var`` () =
      let expected = "var time = System.DateTime.Now"
      let code =
        codegen {
          append "var time = System.DateTime.Now"
        }
      let actual = Codegen.result code
      Assert.Equal(expected,actual)

    [<Fact>]
    let ``Should generate simple cs empty method with braces on new line`` () =
      let expected = "public void Test()\n{}"
      let code =
        codegen {
          appendLine "public void Test()"
          append "{}"
        }
      let actual = Codegen.result code
      Assert.Equal(expected,actual)