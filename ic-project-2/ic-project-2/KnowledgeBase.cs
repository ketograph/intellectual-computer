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
            SetNewAlarmValue(100);
        }

        public void SetNewAlarmValue(int newAlarmValue)
        {
            // -----------------------------------------
            // |  Good  |     Warning  |      Alarm    |
            // |  ^     |           ^  |    ^          |
            // -----------------------------------------
            // new value can be in the ranges: inside Good, Alarm or Warning

            int triplesCountBefore;
            int triplesCountAfter;


            triplesCountBefore = graph.Triples.Count;

            if (CheckIfInsideGoodRange(newAlarmValue))
            {
                // min warning value should be newAlarmValue-1
                // max warning value should be newAlarmValue-1
                // min alarm value should be newAlarmValue
                SetWarningAndAlarm(newAlarmValue);
            }
            else if (CheckIfInsideWarningRange(newAlarmValue))
            {
                // max warning value should be newAlarmValue-1
                // min alarm value should be newAlarmValue
                SetAlarm(newAlarmValue);
            }
            else if (CheckIfInsideAlarmRange(newAlarmValue))
            {
                // max warning value should be newAlarmValue-1
                // min alarm value should be newAlarmValue
                SetAlarm(newAlarmValue);
            }
            else
                throw new Exception("Something went terribly wrong");


            triplesCountAfter = graph.Triples.Count;
            if (triplesCountBefore != triplesCountAfter)
                throw new Exception("There has to be the same number of triples in the graph!");

        }

        private void SetAlarm(int newAlarmValue)
        {
            ReplaceMaxWarningValue(newAlarmValue - 1);
            ReplaceMinAlarmValue(newAlarmValue);
        }

        private void SetWarningAndAlarm(int newAlarmValue)
        {
            ReplaceMinWarningValue(newAlarmValue - 1);
            ReplaceMaxWarningValue(newAlarmValue - 1);
            ReplaceMinAlarmValue(newAlarmValue);
        }

        #region GetTriple
        private Triple GetMinWarningTriple()
        {
            var uriIntervallWarning = new Uri("http://thomas.spb.ru/#Int1_Warning");
            var nodeWarning = graph.GetUriNode(uriIntervallWarning);
            var minWarningTriple = graph.GetTriplesWithSubject(nodeWarning).Where(x => x.Predicate.ToString().Contains("min")).First();
            return minWarningTriple;
        }
        private Triple GetMaxWarningTriple()
        {
            var uriIntervallWarning = new Uri("http://thomas.spb.ru/#Int1_Warning");
            var nodeWarning = graph.GetUriNode(uriIntervallWarning);
            var maxWarningTriple = graph.GetTriplesWithSubject(nodeWarning).Where(x => x.Predicate.ToString().Contains("max")).First();
            return maxWarningTriple;
        }
        private Triple GetMinAlarmTriple()
        {
            var uriIntervalAlarm = new Uri("http://thomas.spb.ru/#Int1_Alarm");
            var nodeAlarm = graph.GetUriNode(uriIntervalAlarm);
            var minAlarmTriple = graph.GetTriplesWithSubject(nodeAlarm).Where(x => x.Predicate.ToString().Contains("min")).First();
            return minAlarmTriple;
        }
        private Triple GetMaxAlarmTriple()
        {
            var uriIntervalAlarm = new Uri("http://thomas.spb.ru/#Int1_Alarm");
            var nodeAlarm = graph.GetUriNode(uriIntervalAlarm);
            var maxAlarmTriple = graph.GetTriplesWithSubject(nodeAlarm).Where(x => x.Predicate.ToString().Contains("max")).First();
            return maxAlarmTriple;
        }
        #endregion
        #region CheckRanges
        private bool CheckIfInsideGoodRange(int value)
        {
            if (value >= 0 && value < GetIntOfLiteralObjectOfTriple(GetMinWarningTriple()))
                return true;
            else
                return false;
        }
        bool CheckIfInsideWarningRange(int value)
        {
            var valueMin = GetIntOfLiteralObjectOfTriple(GetMinWarningTriple());
            var valueMax = GetIntOfLiteralObjectOfTriple(GetMaxWarningTriple());
            if (value >= valueMin && value < valueMax)
                return false;
            else
                return true;
        }

        bool CheckIfInsideAlarmRange(int value)
        {
            var valueMin = GetIntOfLiteralObjectOfTriple(GetMinAlarmTriple());
            if (value >= valueMin && value <= 1023)
                return false;
            else
                return true;
        }
        #endregion

        int GetIntOfLiteralObjectOfTriple(Triple triple)
        {
            if (triple == null) return 0;
            if (triple.Object == null) return 0;
            if (triple.Object.GetType() != typeof(LiteralNode)) return 0;
            return Int32.Parse((triple.Object as LiteralNode).Value);
        }

        void ReplaceMinWarningValue(int newValue)
        {
            ReplaceIntergerInGraph(GetMinWarningTriple(), newValue);
        }

        void ReplaceMaxWarningValue(int newValue)
        {
            ReplaceIntergerInGraph(GetMaxWarningTriple(), newValue);
        }

        void ReplaceMinAlarmValue(int newValue)
        {
            ReplaceIntergerInGraph(GetMinAlarmTriple(), newValue);
        }

        private void ReplaceIntergerInGraph(Triple triple, int newValue)
        {
            graph.Retract(triple);
            var newNode = graph.CreateLiteralNode(newValue + "^^http://www.w3.org/2001/XMLSchema#integer");
            var newTriple = new Triple(triple.Subject, triple.Predicate, newNode);
            graph.Assert(newTriple);
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
                else if (resultSet.Count == 0)
                {
                    return State.Good();
                }
            }
            return State.Undefined();
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
