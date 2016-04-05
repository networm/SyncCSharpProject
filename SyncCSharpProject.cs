using UnityEditor;
using System.Xml;
using System.IO;

public class SyncCSharpProject : AssetPostprocessor
{
    [MenuItem("Tools/SyncCSharpProject")]
    public static void Test()
    {
        OnGeneratedCSProjectFiles();
    }

    public static void OnGeneratedCSProjectFiles()
    {
        string currentDir = Directory.GetCurrentDirectory();
        foreach (var file in Directory.GetFiles(currentDir, "*.csproj"))
        {
            SetPolicies(file);
        }
    }

    private static void SetPolicies(string file)
    {
        string content = @"<MonoDevelop> <Properties> <Policies> <TextStylePolicy inheritsSet='VisualStudio' inheritsScope='text/plain' scope='text/x-csharp' /> <CSharpFormattingPolicy IndentSwitchBody='False' IndentBlocksInsideExpressions='False' AnonymousMethodBraceStyle='NextLine' PropertyBraceStyle='NextLine' PropertyGetBraceStyle='NextLine' PropertySetBraceStyle='NextLine' EventBraceStyle='NextLine' EventAddBraceStyle='NextLine' EventRemoveBraceStyle='NextLine' StatementBraceStyle='NextLine' ElseNewLinePlacement='NewLine' CatchNewLinePlacement='NewLine' FinallyNewLinePlacement='NewLine' WhileNewLinePlacement='DoNotCare' ArrayInitializerWrapping='DoNotChange' ArrayInitializerBraceStyle='NextLine' BeforeMethodDeclarationParentheses='False' BeforeMethodCallParentheses='False' BeforeConstructorDeclarationParentheses='False' NewLineBeforeConstructorInitializerColon='NewLine' NewLineAfterConstructorInitializerColon='SameLine' BeforeDelegateDeclarationParentheses='False' NewParentheses='False' SpacesBeforeBrackets='False' inheritsSet='Mono' inheritsScope='text/x-csharp' scope='text/x-csharp' /> </Policies> </Properties> </MonoDevelop>";

        var doc = new XmlDocument();

        doc.Load(file);

        var node = doc.DocumentElement["ProjectExtensions"];
        if (node != null)
        {
            node.InnerXml = content;
        }
        else
        {
            // Use NamespaceURI to remove xmlns
            // http://stackoverflow.com/questions/135000/how-to-prevent-blank-xmlns-attributes-in-output-from-nets-xmldocument
            var element = doc.CreateElement("ProjectExtensions", doc.DocumentElement.NamespaceURI);
            element.InnerXml = content;
            doc.DocumentElement.AppendChild(element);
        }

        doc.Save(file);
    }
}
