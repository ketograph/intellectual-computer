using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;


// TODO: implement
// Graph g.Assert()
// Graph g.Save()
// NTriplesWriter

//INode n = rdf.GetUriNode(new Uri("http://www.example.org/destDetails#" + destID));
//        if (n != null)
//        {
//            rdf.Retract(rdf.GetTriplesWithSubject(n));
//        }
//        rdf.SaveToFile(rdfDatoteka);


namespace ic_project_2
{
    public class LogMessageEventArgs : System.EventArgs
    {
        public string Message { get; set; }
    }

    public class KnowledgeBase
    {
        Notation3Parser parser;
        SparqlQueryParser queryParser;
        Graph graph;
        string knowledgeBaseFile = "KB_Data_ThomasRossberg.n3";


        public event EventHandler<LogMessageEventArgs> LogMessageAdded;

        public KnowledgeBase()
        {
            parser = new Notation3Parser();
            queryParser = new SparqlQueryParser();
            graph = new Graph();
        }


        public void LoadTurtleFile()
        {
            parser.Load(graph, knowledgeBaseFile);
            OnNewLogMessage("Loaded Notation-3 file.");
        }



        public string CreateSparqlQuery(int parameter, int value)
        {
            return @"
                PREFIX owl:     <http://www.w3.org/2002/07/owl#> 
                PREFIX rdf:     <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX : <http://thomas.spb.ru/#>

                SELECT ?state
                WHERE {
                    ?state :par" + parameter + @" ?int.
                    ?int :min ?min.
                        FILTER(?min < " + value.ToString() + @").
                    ?int :max ?max.
                        FILTER(?max > " + value.ToString() + @").

                }";
        }


        public State AskOneParameter(int parameter, int value)
        {
            string sparqlQuery = CreateSparqlQuery(parameter, value);
            var q = queryParser.ParseFromString(sparqlQuery);
            SparqlResultSet resultSet = graph.ExecuteQuery(q) as SparqlResultSet;

            if (resultSet != null)
            {
                if (resultSet.Count > 1)
                {
                    OnNewLogMessage("ERROR: There is more than one result for one parameter! This means there are overlapping Invervalls in the Knowledge Base.");
                    throw new DataMisalignedException(@"There is more than one result for one parameter! This means there are overlapping Invervalls in the Knowledge Base." +
                                    "Check the Knowdegle Base." +
                                    "Parameter" + parameter + " Value: " + value);
                }
                else if (resultSet.Count == 1)
                {
                    SparqlResult result = resultSet[0];
                    var resultString = result["state"].ToString();
                    OnNewLogMessage(result.ToString());
                    return ParseResultStringToState(resultString);
                }
            }
            return new State() { Status = State.InternalStatus.Good };
        }

        private State ParseResultStringToState(string resultString)
        {
            if (string.IsNullOrEmpty(resultString))
                return State.Undefined();
            else if (resultString.Contains("Warning"))
                return State.Warning();
            else if (resultString.Contains("Alarm"))
                return State.Alarm();
            else
                return State.Good();
        }


        private string GetNodeString(INode node)
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

        protected virtual void OnNewLogMessage(string logMessage)
        {
            LogMessageAdded?.Invoke(this, new LogMessageEventArgs() { Message = logMessage });
        }
    }
}
