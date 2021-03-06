﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using AutoMapper;
using Ninject;
using Schema2Code.CSharp.Inject;
using Schema2Code.Code;
using Schema2Code.Mapping;
using Schema2Code.Xml.Schema;
using Xunit;

namespace Test
{
    public class Class1
    {
        [Fact]
        public void Test()
        {
            var kernel = new StandardKernel(new CSharpModule());

            Mapper.Initialize(map =>
            {
                map.ConstructServicesUsing(t => kernel.Get(t));
                map.AddProfile<SchemaMapProfile>();
            });
            Mapper.AssertConfigurationIsValid();

            var schema = ReadAndCompileSchema("Resource/TestSchema.xsd");
            TraverseSOM(schema);
        }

        private static void TraverseSOM(XmlSchema custSchema)
        {
            foreach (XmlSchemaElement elem in
                             custSchema.Elements.Values)
            {
                ProcessType(elem);
            }
        }

        private static XmlSchema ReadAndCompileSchema(string fileName)
        {
            var tr = XmlReader.Create(fileName);
            // The Read method will throw errors encountered
            // on parsing the schema
            var schema = XmlSchema.Read(tr, ValidationCallback);
            tr.Close();

            var xset = new XmlSchemaSet();
            xset.Add(schema);

            xset.ValidationEventHandler += ValidationCallback;

            // The Compile method will throw errors
            // encountered on compiling the schema
            xset.Compile();

            return schema;
        }

        static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.Write("WARNING: ");
            else if (args.Severity == XmlSeverityType.Error)
                Console.Write("ERROR: ");

            Console.WriteLine(args.Message);
        }

        
        private static void ProcessType(XmlSchemaElement elem)
        {
            var type = AutoMapper.Mapper.Map<IClass>(elem);
                
            if(type != null)
                Console.WriteLine(type.ToString());
            
        }

        
    }
}
