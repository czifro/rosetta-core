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

    [<Fact>]
    let ``Should generate simple cs method with body using default tab size`` () =
      let tabSize = Codegen.defaultIndentSize
      let tabs = (' ', [ for i in 0..tabSize -> "" ]) |> String.Join
      let bodyStr = "Console.WriteLine(\"Hello World!\");"
      let expected = sprintf "public void Test()\n{\n%s%s\n}" tabs bodyStr
      let body =
        codegen {
          appendLine "Console.WriteLine(\"Hello World!\");"
        }
      let code =
        codegen {
          appendLine "public void Test()"
          appendLine "{"
          useIndent body
          append "}"
        }
      let actual = Codegen.result code
      Assert.Equal(expected,actual)

    [<Fact>]
    let ``Should generate nested multi-line fs function`` () =
      let expected =
        [
          "public class Test"
          "{"
          "    public void Test()"
          "    {"
          "        Console.WriteLine(\"Hello World!\");"
          "    }"
          "}"
        ]
        |> String.concat "\n"
      let body =
        codegen {
          appendLine "Console.WriteLine(\"Hello World!\");"
        }
      let method =
        codegen {
          appendLine "public void Test()"
          appendLine "{"
          useIndent body
          appendLine "}"
        }
      let code =
        codegen {
          appendLine "public class Test"
          appendLine "{"
          useIndent method
          append "}"
        }
      let actual = Codegen.result code
      printfn "%s" actual
      Assert.Equal(expected,actual)