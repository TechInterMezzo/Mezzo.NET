using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Mezzo.Generators
{
    [Generator]
    public class ScribanSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        { }

        public void Execute(GeneratorExecutionContext context)
        {
            foreach (var file in context.AdditionalFiles.Where(x => x.Path.EndsWith(".sbncs")))
            {
                Template template = Template.Parse(file.GetText()!.ToString());
                SourceText source = SourceText.From(template.Render(), Encoding.UTF8);
                context.AddSource(Path.GetFileNameWithoutExtension(file.Path) + ".cs", source);
            }
        }
    }
}
