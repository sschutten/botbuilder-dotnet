// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Microsoft.Bot.Builder.LanguageGeneration.Events;

namespace Microsoft.Bot.Builder.LanguageGeneration
{
    /// <summary>
    /// LG managed code checker.
    /// </summary>
    public class EventRegister : LGTemplateParserBaseVisitor<object>
    {
        private readonly List<Template> templates;
        private Template currentTemplate;
        private readonly EventHandler onRegisterEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventRegister"/> class.
        /// </summary>
        /// <param name="templates">The templates wihch would be checked.</param>
        /// <param name="onRegisterEvent">registeSourceMap handler.</param>
        public EventRegister(List<Template> templates, EventHandler onRegisterEvent)
        {
            this.templates = templates;
            this.onRegisterEvent = onRegisterEvent;
        }

        public Dictionary<string, ExpressionRef> ExpressionRefDict { get; set; } = new Dictionary<string, ExpressionRef>();

        /// <summary>
        /// Register sourse mapping.
        /// </summary>
        public void Register()
        {
            foreach (var template in templates)
            {
                onRegisterEvent?.Invoke(template, new RegisterSourceMapArgs { SourceRange = template.SourceRange });
                currentTemplate = template;

                if (template.TemplateBodyParseTree != null)
                {
                    Visit(template.TemplateBodyParseTree);
                }
            }
        }

        public override object VisitNormalTemplateBody([NotNull] LGTemplateParser.NormalTemplateBodyContext context)
        {
            foreach (var templateStr in context.templateString())
            {
                var errorTemplateStr = templateStr.errorTemplateString();
                if (errorTemplateStr == null)
                {
                    return Visit(templateStr.normalTemplateString());
                }
            }

            return null;
        }

        public override object VisitStructuredTemplateBody([NotNull] LGTemplateParser.StructuredTemplateBodyContext context)
        {
            if (context.structuredBodyNameLine().errorStructuredName() == null
                && context.structuredBodyEndLine() != null
                && (context.errorStructureLine() == null || context.errorStructureLine().Length == 0)
                && (context.structuredBodyContentLine() != null && context.structuredBodyContentLine().Length > 0))
            {
                var bodys = context.structuredBodyContentLine();
                foreach (var body in bodys)
                {
                    if (body.objectStructureLine() != null)
                    {
                        RegisterExpression(body.objectStructureLine().GetText(), body.objectStructureLine());
                    }
                    else
                    {
                        // KeyValueStructuredLine
                        var structureValues = body.keyValueStructureLine().keyValueStructureValue();
                        foreach (var structureValue in structureValues)
                        {
                            foreach (var expression in structureValue.EXPRESSION_IN_STRUCTURE_BODY())
                            {
                                RegisterExpression(expression.GetText(), structureValue);
                            }
                        }
                    }
                }
            }

            return null;
        }

        public override object VisitIfElseBody([NotNull] LGTemplateParser.IfElseBodyContext context)
        {
            var ifRules = context.ifElseTemplateBody().ifConditionRule();
            for (var idx = 0; idx < ifRules.Length; idx++)
            {
                var conditionNode = ifRules[idx].ifCondition();
                var ifExpr = conditionNode.IF() != null;
                var elseIfExpr = conditionNode.ELSEIF() != null;
                var elseExpr = conditionNode.ELSE() != null;

                var node = ifExpr ? conditionNode.IF() :
                           elseIfExpr ? conditionNode.ELSEIF() :
                           conditionNode.ELSE();

                if (node.GetText().Count(u => u == ' ') > 1
                    || (idx == 0 && !ifExpr)
                    || (idx > 0 && ifExpr)
                    || (idx == ifRules.Length - 1 && !elseExpr)
                    || (idx > 0 && idx < ifRules.Length - 1 && !elseIfExpr))
                {
                    return null;
                }

                if (!elseExpr && (ifRules[idx].ifCondition().EXPRESSION().Length == 1))
                {
                    RegisterExpression(conditionNode.EXPRESSION(0).GetText(), conditionNode);
                }

                if (ifRules[idx].normalTemplateBody() != null)
                {
                    Visit(ifRules[idx].normalTemplateBody());
                }
            }

            return null;
        }

        public override object VisitSwitchCaseBody([NotNull] LGTemplateParser.SwitchCaseBodyContext context)
        {
            var switchCaseRules = context.switchCaseTemplateBody().switchCaseRule();
            var length = switchCaseRules.Length;
            for (var idx = 0; idx < length; idx++)
            {
                var switchCaseNode = switchCaseRules[idx].switchCaseStat();
                var switchExpr = switchCaseNode.SWITCH() != null;
                var caseExpr = switchCaseNode.CASE() != null;
                var defaultExpr = switchCaseNode.DEFAULT() != null;
                var node = switchExpr ? switchCaseNode.SWITCH() :
                           caseExpr ? switchCaseNode.CASE() :
                           switchCaseNode.DEFAULT();

                if (node.GetText().Count(u => u == ' ') > 1
                    || (idx == 0 && !switchExpr)
                    || (idx > 0 && switchExpr)
                    || (idx > 0 && idx < length - 1 && !caseExpr)
                    || (idx == length - 1 && caseExpr)
                    || (idx == length - 1 && defaultExpr && length == 2))
                {
                    return null;
                }

                if ((switchExpr || caseExpr) && switchCaseNode.EXPRESSION().Length == 1)
                {
                    RegisterExpression(switchCaseNode.EXPRESSION(0).GetText(), switchCaseNode);
                }

                if ((caseExpr || defaultExpr) && switchCaseRules[idx].normalTemplateBody() != null)
                {
                    Visit(switchCaseRules[idx].normalTemplateBody());
                }
            }

            return null;
        }

        public override object VisitNormalTemplateString([NotNull] LGTemplateParser.NormalTemplateStringContext context)
        {
            foreach (var expression in context.EXPRESSION())
            {
                RegisterExpression(expression.GetText(), context);
            }

            return null;
        }

        private object RegisterExpression(string exp, ParserRuleContext context, string prefix = "")
        {
            if (!exp.EndsWith("}"))
            {
                return null;
            }

            exp = exp.TrimExpression();

            var source = currentTemplate?.SourceRange?.Source;
            if (source != null && Path.IsPathRooted(source) && context != null && this.onRegisterEvent != null)
            {
                var expressionRef = new ExpressionRef(exp, context.Start.Line, source);
                var lineOffset = this.currentTemplate.SourceRange.Range.Start.Line;
                var sourceRange = new SourceRange(context, source, lineOffset);
                this.onRegisterEvent(expressionRef, new RegisterSourceMapArgs { SourceRange = sourceRange });

                var expressionLine = lineOffset + context.Start.Line;
                var id = exp + expressionLine + source;
                if (!ExpressionRefDict.ContainsKey(id))
                {
                    ExpressionRefDict.Add(id, expressionRef);
                }
            }

            return null;
        }
    }
}
