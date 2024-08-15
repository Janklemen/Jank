using System;
using System.Collections.Generic;
using System.Linq;
using JankGen.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JankGen
{
    [Generator]
    public class JankStateGenerator : AJankGenerator
    {
        const string cAttributeName = "JankState";

        public override void ExecuteClassGenerator(GeneratorExecutionContext context,
            Compilation compilation, SemanticModel model,
            ClassDeclarationSyntax classDeclarationSyntax)
        {
            if (!classDeclarationSyntax.IsWithAttribute(cAttributeName))
                return;

            NamespaceDeclarationSyntax namespa = classDeclarationSyntax.Parent as NamespaceDeclarationSyntax;

            string namespaceName = namespa.Name.ToString();
            string className = classDeclarationSyntax.Identifier.ToString();

            List<FieldDeclarationSyntax> validFields = classDeclarationSyntax.GetAllFields(context).GetValidFields(
                _ => true
            );

            List<string> usings = new();
            usings.Add("System");
            usings.Add("System.Threading");
            usings.Add("System.Collections.Generic");
            usings.Add("UnityEngine");
            usings.Add("Jank.Utilities");
            usings.Add("Jank.Observables");
            usings.Add("Jank.Observables.Observables");
            usings.Add("Jank.Observables.Subject");
            usings.Add("Cysharp.Threading.Tasks");
            usings.Add("System.Linq");
            
            List<string> propertyInitializers =
                validFields.Select(p => CreateProperty(p, className, usings, model))
                    .ToList();

            string classGeneratedSourceCode = $@"
{UTOutput.AggregateUsings(usings)}

namespace {namespaceName} 
{{
    public partial class {className}
    {{
        {UTOutput.AggregateString(propertyInitializers)}
    }}
}}
";
            context.AddSource($"{classDeclarationSyntax.Identifier}.g",
                UTOutput.ArrangeUsingRoslyn(classGeneratedSourceCode));
        }

        public override void ExecuteEnumGenerator(GeneratorExecutionContext generatorExecutionContext,
            Compilation compilation,
            SemanticModel model, EnumDeclarationSyntax enumDeclarationSyntax)
        {
        }

        public override void ExecutePostGenerator(GeneratorExecutionContext generatorExecutionContext,
            Compilation compilation,
            SemanticModel model)
        {
        }
        
        string CreateProperty(FieldDeclarationSyntax syntax, string serviceName,
            List<string> additionalUsings, SemanticModel model)
        {
            SyntaxToken identifier = syntax.GetFieldDeclarationIdentifier();
            model.GetTypeInfo(syntax.Declaration.Type).Type.GetTypeSymbolUsings(additionalUsings);

            string type = syntax.Declaration.Type.GetText().ToString().Trim();
            string propertyName = identifier.ToString();

            string arrayFunctions = GetArrayFunctions(syntax, propertyName, identifier);
            string listFunctions = GetListFunctions(syntax, propertyName, identifier);

            return @$"
public IObservableAsync<Change<{type}>> Observe{propertyName} => Subject{propertyName};
SubjectAsync<Change<{type}>> Subject{propertyName} = new();

public async UniTask Signal{propertyName}({type} last = default) 
    => await Subject{propertyName}.OnNext(new(last == default ? {identifier} : last, {identifier}));

public async UniTask<IDisposable> Bind{propertyName}(ActionAsync<Change<{type}>> action) {{
    IDisposable sub = Subject{propertyName}.Subscribe(onNext:action);
    await Signal{propertyName}();
    return sub;
}}

public {type} Get{propertyName}() => {identifier};

public async UniTask Set{propertyName}({type} value)
{{
    {type} last = {identifier};
    {identifier} = value;
    await Signal{propertyName}(last);
}}

{arrayFunctions}{listFunctions}
";
        }

        static string GetListFunctions(FieldDeclarationSyntax syntax, string propertyName, SyntaxToken identifier)
{
    bool isList = syntax.Declaration.Type.ToString().StartsWith("List<");
    string listType = syntax.Declaration.Type is GenericNameSyntax gns ? gns.TypeArgumentList.Arguments[0].ToString() : "";
    string listFunctions = !isList
        ? ""
        : $@"
public IObservableAsync<IndexedChange<{listType}>> ObserveElement{propertyName} => ElementSubject{propertyName};
SubjectAsync<IndexedChange<{listType}>> ElementSubject{propertyName} = new();

public IObservableAsync<List<{listType}>> ObserveClear{propertyName} => ClearSubject{propertyName};
SubjectAsync<List<{listType}>> ClearSubject{propertyName} = new();

public IObservableAsync<IndexedValue<{listType}>> ObserveInsert{propertyName} => InsertSubject{propertyName};
SubjectAsync<IndexedValue<{listType}>> InsertSubject{propertyName} = new();

public IObservableAsync<IndexedValue<{listType}>> ObserveRemoveAt{propertyName} => RemoveAtSubject{propertyName};
SubjectAsync<IndexedValue<{listType}>> RemoveAtSubject{propertyName} = new();

public async UniTask ElementSignal{propertyName}(int index, {listType} lastValue = default) 
    => await ElementSubject{propertyName}.OnNext(new (lastValue == default ? {propertyName}[index] : lastValue, {propertyName}[index], index));

public async UniTask<IDisposable> ListBehaviorBind{propertyName}(
    ActionAsync<IndexedChange<{listType}>> setAction,
    ActionAsync<List<{listType}>> clearAction,
    ActionAsync<IndexedValue<{listType}>> insertAction,
    ActionAsync<IndexedValue<{listType}>> removeAtAction
    ) {{

    CompositeDisposable sub = new(new[] {{
        ElementSubject{propertyName}.Subscribe(onNext: setAction),
        ClearSubject{propertyName}.Subscribe(onNext: clearAction),
        InsertSubject{propertyName}.Subscribe(onNext: insertAction),
        RemoveAtSubject{propertyName}.Subscribe(onNext: removeAtAction)
    }});

    for(int i = 0; i < GetCount{propertyName}; i++)
        await ElementSubject{propertyName}.OnNext(new({propertyName}[i], {propertyName}[i], i));
    return sub;
}}

public async UniTask ElementSet{propertyName}({listType} value, int index)
{{
    {listType} lastValue = {identifier}[index];
    {identifier}[index] = value;
    await ElementSignal{propertyName}(index, lastValue);
}}

public async UniTask Clear{propertyName}()
{{
    List<{listType}> copy = {identifier}.ToList();
    {identifier}.Clear();
    await ClearSubject{propertyName}.OnNext(copy);
}}

public async UniTask Insert{propertyName}({listType} value, int index)
{{
    {identifier}.Insert(index, value);
    await InsertSubject{propertyName}.OnNext(new(value, index));
}}

public async UniTask RemoveAt{propertyName}(int index)
{{
    {listType} removing = {identifier}[index];
    {identifier}.RemoveAt(index);
    await RemoveAtSubject{propertyName}.OnNext(new(removing, index));
}}

public int GetCount{propertyName} => {identifier}.Count;
";
    return listFunctions;
}

        static string GetArrayFunctions(FieldDeclarationSyntax syntax, string propertyName, SyntaxToken identifier)
        {
            bool isArray = syntax.Declaration.Type.IsKind(SyntaxKind.ArrayType);
            string arrayType = syntax.Declaration.Type is ArrayTypeSyntax ats ? ats.ElementType.ToString() : "";

            string arrayFunctions = !isArray
                ? ""
                : $@"
public IObservableAsync<IndexedChange<{arrayType}>> ObserveElement{propertyName} => ElementSubject{propertyName};
SubjectAsync<IndexedChange<{arrayType}>> ElementSubject{propertyName} = new();

public async UniTask ElementSignal{propertyName}(int index, {arrayType} lastValue = default) => await ElementSubject{propertyName}.OnNext(new(lastValue == default ? {propertyName}[index] : lastValue, {propertyName}[index], index));

public async UniTask<IDisposable> ElementBind{propertyName}(ActionAsync<IndexedChange<{arrayType}>> action) {{
    IDisposable sub = ElementSubject{propertyName}.Subscribe(onNext: action);
    for(int i = 0; i < GetCount{propertyName}; i++)
        await ElementSubject{propertyName}.OnNext(new({propertyName}[i], {propertyName}[i], i));
    return sub;
}}

public async UniTask ElementSet{propertyName}({arrayType} value, int index)
{{
    {arrayType} lastValue = {identifier}[index];
    {identifier}[index] = value;
    await ElementSignal{propertyName}(index, lastValue);
}}

public int GetCount{propertyName} => {identifier}.Length;
";
            return arrayFunctions;
        }
    }
}