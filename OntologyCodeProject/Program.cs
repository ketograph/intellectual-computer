using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;

namespace OntologyCodeProject
{

    /*
     PREFIX my: <http://www.codeproject.com/KB/recipes/n3_notation#>
SELECT ?name
WHERE {
    [ a my:person;
        my:suffers my:insomnia;
        my:name ?name].
}"; 
     */
    static class Program
    {
        private const string getInsomnia = @"
PREFIX my: <http://www.codeproject.com/KB/recipes/n3_notation#>
PREFIX ns0: <http://our-place.spb.ru/today#>
SELECT ?x ?name
WHERE {
     ?name ns0:needs ns0:mult.
}";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var parser = new Notation3Parser();            
            var graph = new Graph();

            Console.WriteLine("Loading Notation-3 file.");
            //parser.Load(graph, @"n3\ontology.n3");

            parser.Load(graph, @"n3\converter.n3");
            Console.WriteLine("Loaded Notation-3 file.");
            Console.WriteLine("Nodes:");
            foreach (Triple triple in graph.Triples)
            {
                Console.WriteLine("{0} {1} {2}", GetNodeString(triple.Subject), GetNodeString(triple.Predicate), GetNodeString(triple.Object));
            }
            Console.WriteLine();
            Console.WriteLine("Results of 'insomnia' query:");
            Console.WriteLine();
            SparqlResultSet resultSet = graph.ExecuteQuery(getInsomnia) as SparqlResultSet;
            if (resultSet != null)
            {
                Console.WriteLine("Results for variable 'name':");
                for (int i = 0; i < resultSet.Count; i++)
                {
                    SparqlResult result = resultSet[i];
                    Console.WriteLine("{0}. {1}", i+1, result["name"]);
                }
            }

            Console.ReadLine();
        }

        static string GetNodeString(INode node)
        {
            string s = node.ToString();
            switch (node.NodeType)
            {
                case NodeType.Uri:
                    int lio = s.LastIndexOf('#');
                    if (lio == -1)
                        return s;
                    else
                        return s.Substring(lio + 1);
                case NodeType.Literal:
                    return string.Format("\"{0}\"", s);
                default:
                    return s;
            }
        }
    }
}
