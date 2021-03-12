using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DCF.Common.Logging;
using DCF.Timelimits.Rules.Definitions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using NRules;
using NRules.Fluent;

namespace DCF.Timelimits.Rules.Scripting
{
    public class RulesEngine
    {
        private ILog _logger = LogProvider.GetLogger(typeof(RulesEngine));

        // TODO: Move to interface/service
        public static ISessionFactory CompileTimelimitRuleNetwork()
        {
            //TODO: Use rules engine & service to get rules
            var repository = new RuleRepository();
            repository.Load(x => x.From(typeof(RuleContext).Assembly));
            return repository.Compile();

        }

        public async Task<Assembly> CompileRulesIntoAssembly(IRuleScriptService scriptService)
        {
            var ruleFiles = await scriptService.GetScriptFilesAsync();
            var syntaxTrees = this.GetSytanxTrees(ruleFiles);
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var referencedAssemblies = Assembly.GetEntryAssembly().MyGetReferencedAssembliesRecursive();


            var compilation = CSharpCompilation.Create("DynamicRulesAssembly").WithOptions(compilationOptions);
            foreach (var assembly in referencedAssemblies)
            {
                compilation.AddReferences(MetadataReference.CreateFromFile(assembly.Value.Location));
            }

            // Add Nrules if not already loaded

            //compilation.
            //    .AddReferences(MetadataReference.CreateFromFile(typeof(NRules.Session).Assembly.Location)) // add NRules Assmebly
            //    .AddReferences(MetadataReference.CreateFromFile(typeof(NRules.Fluent.Dsl.Rule).Assembly.Location)) // add NRules.Fluent Assmebly
            //    .AddReferences(MetadataReference.CreateFromFile(typeof(NRules.RuleModel.RuleElement).Assembly.Location)) // add NRules.RuleModel Assembly
            
            // Add Scripts sytnax trees
            compilation.AddSyntaxTrees(syntaxTrees);

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);
                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());
                    return assembly;
                }
            }

            return null;

        }
        private IEnumerable<SyntaxTree> GetSytanxTrees(List<String> ruleScripts)
        {
            foreach (var ruleScript in ruleScripts)
            {
                yield return CSharpSyntaxTree.ParseText(ruleScript);
            }
        }

        

        public static void LogSessionEvents(ISession ruleSession, ILog logger)
        {
            ruleSession.Events.ActionFailedEvent += (sender, args) =>
            {
                logger.ErrorFormat("Rule Action Failed {@args}:");
            };
            ruleSession.Events.ConditionFailedEvent += (sender, args) =>
            {
                logger.ErrorFormat("Condition Failed Event {@args}:", args);
            };

            //Working Memory Events
            ruleSession.Events.FactInsertingEvent += (sender, args) =>
            {
                logger.InfoFormat("Fact Inserting Event {@args}:", args);
            };
            ruleSession.Events.FactInsertedEvent += (sender, args) =>
            {
                logger.InfoFormat("Fact Inserted Event {@args}:", args);
            };
            ruleSession.Events.FactUpdatingEvent += (sender, args) =>
            {
                logger.InfoFormat("Fact Updating Event {@args}:", args);
            };
            ruleSession.Events.FactUpdatedEvent += (sender, args) =>
            {
                logger.InfoFormat("Fact Updated Event {@args}:", args);
            };
            ruleSession.Events.FactRetractingEvent += (sender, args) =>
            {
                logger.InfoFormat("Fact Retracting Event {@args}:", args);
            };
            ruleSession.Events.FactRetractedEvent += (sender, args) =>
            {
                logger.InfoFormat("Fact Retracted Event {@args}:", args);
            };


            // Agenda events
            ruleSession.Events.ActivationCreatedEvent += (sender, args) =>
            {
                logger.InfoFormat("Activation Created Event {@args}:", args);
                logger.DebugFormat("Rule {Rule} activated created with facts {@Facts}",args.Rule.Name, args.Facts.Select(y=>y.Value));
            };
            ruleSession.Events.ActivationUpdatedEvent += (sender, args) =>
            {
                logger.InfoFormat("Activation Updated Event {@args}:", args);
                logger.DebugFormat("Rule {Rule} activation updated with facts {@Facts}", args.Rule.Name, args.Facts.Select(y => y.Value));

            };
            ruleSession.Events.ActivationDeletedEvent += (sender, args) =>
            {
                logger.InfoFormat("Activation Deleted Event {@args}:", args);
            };
            ruleSession.Events.RuleFiringEvent += (sender, args) =>
            {
                logger.InfoFormat("Rule Firing Event {@args}:", args);

            };
            ruleSession.Events.RuleFiredEvent += (sender, args) =>
            {
                logger.InfoFormat("Rule Fired Event {@args}:", args);
                logger.DebugFormat("Rule {Rule} activated created with facts {@Facts}", args.Rule.Name, args.Facts.Select(y => y.Value));
            };
        }
    }
}