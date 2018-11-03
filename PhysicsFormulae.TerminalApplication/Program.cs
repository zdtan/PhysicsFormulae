﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PhysicsFormulae.Compiler.Formulae;
using PhysicsFormulae.Compiler.Constants;
using PhysicsFormulae.Compiler.References;

namespace PhysicsFormulae.TerminalApplication
{
    public class Model
    {
        public IEnumerable<Formula> Formulae { get; set; }
        public IEnumerable<Constant> Constants { get; set; }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var formulaCompiler = new FormulaCompiler();
            var constantCompiler = new ConstantCompiler();
            var referenceCompiler = new ReferenceCompiler();

            var formulae = new List<Formula>();
            var constants = new List<Constant>();
            var references = new List<Reference>();

            var directoryInfo = new DirectoryInfo(@"..\..\..\PhysicsFormulae.Formulae");
            var formulaFiles = directoryInfo.GetFiles("*.formula");
            var constantFiles = directoryInfo.GetFiles("*.constant");
            var referenceFiles = directoryInfo.GetFiles("*.reference");

            foreach (var file in referenceFiles)
            {
                var lines = File.ReadAllLines(file.FullName);
                var reference = referenceCompiler.CompileReference(lines);
                references.Add(reference);

                Console.WriteLine(reference.CitationKey);
            }

            foreach (var file in formulaFiles)
            {
                var lines = File.ReadAllLines(file.FullName);
                var formula = formulaCompiler.CompileFormula(lines, references);
                formulae.Add(formula);

                Console.WriteLine(formula.Reference);
            }

            foreach (var file in constantFiles)
            {
                var lines = File.ReadAllLines(file.FullName);
                var constant = constantCompiler.CompileConstant(lines);
                constants.Add(constant);

                Console.WriteLine(constant.Reference);
            }

            foreach(var constant in constants)
            {
                foreach(var formula in formulae)
                {
                    if (formula.Identifiers.Any(i => i.Reference == constant.Reference))
                    {
                        constant.UsedInFormulae.Add(formula.Reference);
                    }
                }
            }

            var model = new Model();

            model.Formulae = formulae;
            model.Constants = constants;

            var outputLocations = new List<string>() { @"..\..\..\PhysicsFormulae.Formulae\Compiled.json", @"..\..\..\PhysicsFormulae.WebApplication\formulae.json" };

            var serializer = new JsonSerializer();

            foreach (var outputLocation in outputLocations)
            {
                using (var streamWriter = new StreamWriter(outputLocation))
                using (var jsonTextWriter = new JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(jsonTextWriter, model);
                }
            }
        }
    }
}
