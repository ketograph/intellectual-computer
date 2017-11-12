using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Writing;

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
        string knowledgeBaseFile = "KB_Data.txt";


        public event EventHandler<LogMessageEventArgs> LogMessageAdded;

        public KnowledgeBase()
        {
            parser = new Notation3Parser();
            queryParser = new SparqlQueryParser();
            graph = new Graph();
        }

        public void LoadTurtleFile() //Laden des Files für die Bearbeitung
        {
            parser.Load(graph, knowledgeBaseFile);
            OnNewLogMessage("Loaded Data file.");
        }

        public void SaveGraphToFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("Filename was empty or not ending with .txt");
            var writer = new Notation3Writer();
            writer.Save(graph, filename);
            OnNewLogMessage("Saved changed Knowledge Base to " + filename);
        }

        public void SetNewAlarmValueAndSaveFile(int newAlarmValue)
        {
            OnNewLogMessage("Setting alarm value for Parameter 1 to " + newAlarmValue);
            SetNewAlarmValue(newAlarmValue);
            string newFilename = System.IO.Path.GetFileNameWithoutExtension(knowledgeBaseFile) + "-" + newAlarmValue.ToString() + ".txt";
            SaveGraphToFile(newFilename);
        }

        public void SetNewAlarmValue(int newAlarmValue)
        {
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
            else if (CheckIfInsideWarningOrAlarmRange(newAlarmValue))
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

        private Triple GetMinWarningTriple()
        {
            var uriIntervallWarning = new Uri("http://computing.spb.ru/#Int1_Warning");
            var nodeWarning = graph.GetUriNode(uriIntervallWarning);
            var minWarningTriple = graph.GetTriplesWithSubject(nodeWarning).Where(x => x.Predicate.ToString().Contains("min")).First();
            return minWarningTriple;
        }
        private Triple GetMaxWarningTriple()
        {
            var uriIntervallWarning = new Uri("http://computing.spb.ru/#Int1_Warning");
            var nodeWarning = graph.GetUriNode(uriIntervallWarning);
            var maxWarningTriple = graph.GetTriplesWithSubject(nodeWarning).Where(x => x.Predicate.ToString().Contains("max")).First();
            return maxWarningTriple;
        }
        private Triple GetMinAlarmTriple()
        {
            var uriIntervalAlarm = new Uri("http://computing.spb.ru/#Int1_Alarm");
            var nodeAlarm = graph.GetUriNode(uriIntervalAlarm);
            var minAlarmTriple = graph.GetTriplesWithSubject(nodeAlarm).Where(x => x.Predicate.ToString().Contains("min")).First();
            return minAlarmTriple;
        }
        private Triple GetMaxAlarmTriple()
        {
            var uriIntervalAlarm = new Uri("http://computing.spb.ru/#Int1_Alarm");
            var nodeAlarm = graph.GetUriNode(uriIntervalAlarm);
            var maxAlarmTriple = graph.GetTriplesWithSubject(nodeAlarm).Where(x => x.Predicate.ToString().Contains("max")).First();
            return maxAlarmTriple;
        }
        private bool CheckIfInsideGoodRange(int value)
        {
            int minWarningValue = GetTripleObjectLiteralNodeValue(GetMinWarningTriple());
            if (value >= 0 && value < minWarningValue)
                return true;
            else
                return false;
        }
        bool CheckIfInsideWarningOrAlarmRange(int value)
        {
            var valueMin = GetTripleObjectLiteralNodeValue(GetMinWarningTriple());
            var valueMax = GetTripleObjectLiteralNodeValue(GetMaxAlarmTriple());
            if (value >= valueMin && value < valueMax)
                return true;
            else
                return false;
        }

        int GetTripleObjectLiteralNodeValue(Triple triple)
        {
            if (triple == null) return 0;
            if (triple.Object == null) return 0;
            if (triple.Object.GetType() != typeof(LiteralNode)) return 0;
            return Int32.Parse((triple.Object as LiteralNode).Value);
        }
        void ReplaceMinWarningValue(int newValue)
        {
            ReplaceTripleObjectLiteralNode(GetMinWarningTriple(), newValue);
        }

        void ReplaceMaxWarningValue(int newValue)
        {
            ReplaceTripleObjectLiteralNode(GetMaxWarningTriple(), newValue);
        }

        void ReplaceMinAlarmValue(int newValue)
        {
            ReplaceTripleObjectLiteralNode(GetMinAlarmTriple(), newValue);
        }

        private void ReplaceTripleObjectLiteralNode(Triple triple, int newValue)
        {
            graph.Retract(triple);
            ILiteralNode number = graph.CreateLiteralNode(newValue.ToString(), UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeInteger));

            var newTriple = new Triple(triple.Subject, triple.Predicate, number);
            graph.Assert(newTriple);
        }


        public string CreateSparqlQuery(int parameter, int value)
        {
            return @"
                PREFIX owl:     <http://www.w3.org/2002/07/owl#> 
                PREFIX rdf:     <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX : <http://computing.spb.ru/#>

                SELECT ?state
                WHERE {
                    ?state :par" + parameter + @" ?int.
                    ?int :min ?min.
                        FILTER(?min < " + value.ToString() + @").
                    ?int :max ?max.
                        FILTER(?max > " + value.ToString() + @").

                }";
        }

        public string CreateSparqlQueryPair(int ParA, int valueA, int ParB, int valueB)
        {
            // Pair1_Alarm
            // Pair1_Int1_Alarm
            string ParAname = "Par" + ParA;
            string ParBname = "Par" + ParB;

            return @"
                PREFIX owl:     <http://www.w3.org/2002/07/owl#> 
                PREFIX rdf:     <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX :        <http://computing.spb.ru/#>

                SELECT  ?state ?Pair
                WHERE {
                    ?state :Pair ?Pair.
                    ?Pair :" + ParAname + @" ?intA.
                    ?intA :min ?minA.
                        FILTER(?minA < " + valueA.ToString() + @").
                    ?intA :max ?maxA.
                        FILTER(?maxA > " + valueA.ToString() + @").
                    ?Pair :" + ParBname + @" ?intB.
                    ?intB :min ?minB.
                        FILTER(?minB < " + valueB.ToString() + @").
                    ?intB :max ?maxB.
                        FILTER(?maxB > " + valueB.ToString() + @").
                }";
        }

        public string CreateSparqlQueryTriple(int ParA, int valueA, int ParB, int valueB, int ParC, int valueC)
        {
            string ParAname = "Par" + ParA;
            string ParBname = "Par" + ParB;
            string ParCname = "Par" + ParC;

            return @"
                PREFIX owl:     <http://www.w3.org/2002/07/owl#> 
                PREFIX rdf:     <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX :        <http://computing.spb.ru/#>

                SELECT  ?state ?Pair
                WHERE {
                    ?state :Triple ?triple.
                    ?triple :" + ParAname + @" ?intA.
                    ?intA :min ?minA.
                        FILTER(?minA < " + valueA.ToString() + @").
                    ?intA :max ?maxA.
                        FILTER(?maxA > " + valueA.ToString() + @").
                    ?triple :" + ParBname + @" ?intB.
                    ?intB :min ?minB.
                        FILTER(?minB < " + valueB.ToString() + @").
                    ?intB :max ?maxB.
                        FILTER(?maxB > " + valueB.ToString() + @").
                    ?triple :" + ParCname + @" ?intC.
                    ?intC :min ?minC.
                        FILTER(?minC < " + valueC.ToString() + @").
                    ?intC :max ?maxC.
                        FILTER(?maxC > " + valueC.ToString() + @").
                }";
        }

        public string CreateSparqlQueryQuad(int ParA, int valueA, int ParB, int valueB, int ParC, int valueC, int ParD, int valueD)
        {
            string ParAname = "Par" + ParA;
            string ParBname = "Par" + ParB;
            string ParCname = "Par" + ParC;
            string ParDname = "Par" + ParD;

            return @"
                PREFIX owl:     <http://www.w3.org/2002/07/owl#> 
                PREFIX rdf:     <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX :        <http://computing.spb.ru/#>

                SELECT  ?state ?Pair
                WHERE {
                    ?state :Quad ?Pair.
                    ?Pair :" + ParAname + @" ?intA.
                    ?intA :min ?minA.
                        FILTER(?minA < " + valueA.ToString() + @").
                    ?intA :max ?maxA.
                        FILTER(?maxA > " + valueA.ToString() + @").
                    ?Pair :" + ParBname + @" ?intB.
                    ?intB :min ?minB.
                        FILTER(?minB < " + valueB.ToString() + @").
                    ?intB :max ?maxB.
                        FILTER(?maxB > " + valueB.ToString() + @").
                    ?Pair :" + ParCname + @" ?intC.
                    ?intC :min ?minC.
                        FILTER(?minC < " + valueC.ToString() + @").
                    ?intC :max ?maxC.
                        FILTER(?maxC > " + valueC.ToString() + @").
                    ?Pair :" + ParDname + @" ?intD.
                    ?intD :min ?minD.
                        FILTER(?minD < " + valueD.ToString() + @").
                    ?intD :max ?maxD.
                        FILTER(?maxD > " + valueD.ToString() + @").
                }";
        }

        public string CreateSparqlQueryFifth(int ParA, int valueA, int ParB, int valueB, int ParC, int valueC, int ParD, int valueD, int ParE, int valueE)
        {
            string ParAname = "Par" + ParA;
            string ParBname = "Par" + ParB;
            string ParCname = "Par" + ParC;
            string ParDname = "Par" + ParD;
            string ParEname = "Par" + ParE;

            return @"
                PREFIX owl:     <http://www.w3.org/2002/07/owl#> 
                PREFIX rdf:     <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX :        <http://computing.spb.ru/#>

                SELECT  ?state ?fifth
                WHERE {
                    ?state :Fifth ?fifth.
                    ?fifth :" + ParAname + @" ?intA.
                    ?intA :min ?minA.
                        FILTER(?minA < " + valueA.ToString() + @").
                    ?intA :max ?maxA.
                        FILTER(?maxA > " + valueA.ToString() + @").
                    ?fifth :" + ParBname + @" ?intB.
                    ?intB :min ?minB.
                        FILTER(?minB < " + valueB.ToString() + @").
                    ?intB :max ?maxB.
                        FILTER(?maxB > " + valueB.ToString() + @").
                    ?fifth :" + ParCname + @" ?intC.
                    ?intC :min ?minC.
                        FILTER(?minC < " + valueC.ToString() + @").
                    ?intC :max ?maxC.
                        FILTER(?maxC > " + valueC.ToString() + @").
                    ?fifth :" + ParDname + @" ?intD.
                    ?intD :min ?minD.
                        FILTER(?minD < " + valueD.ToString() + @").
                    ?intD :max ?maxD.
                        FILTER(?maxD > " + valueD.ToString() + @").
                    ?fifth :" + ParEname + @" ?intE.
                    ?intE :min ?minE.
                        FILTER(?minE < " + valueE.ToString() + @").
                    ?intE :max ?maxE.
                        FILTER(?maxE > " + valueE.ToString() + @").
                }";
        }


        public State AskOneParameter(int parameter, int value)
        {
            string sparqlQuery = CreateSparqlQuery(parameter, value);
            var q = queryParser.ParseFromString(sparqlQuery);
            SparqlResultSet resultSet = graph.ExecuteQuery(q) as SparqlResultSet;

            return ParseSparqlResultSetToState(resultSet);
        }

        public State AskPair(SensorSetValues parameters)
        {
            State[] stateArrayPair = new State[4];

            stateArrayPair[0] = AskPairParameter(1, parameters.SensorValue[0], 3, parameters.SensorValue[2]);
            stateArrayPair[1] = AskPairParameter(2, parameters.SensorValue[1], 5, parameters.SensorValue[4]);
            stateArrayPair[2] = AskPairParameter(4, parameters.SensorValue[3], 5, parameters.SensorValue[4]);
            stateArrayPair[3] = AskPairParameter(2, parameters.SensorValue[1], 3, parameters.SensorValue[2]);

            return ParseStateArrayToState(stateArrayPair);
        }

        public State AskTriple(SensorSetValues parameters)
        {
            State[] stateArrayTriple = new State[3];

            stateArrayTriple[0] = AskTripleParameter(1, parameters.SensorValue[0], 3, parameters.SensorValue[2], 4, parameters.SensorValue[3]);
            stateArrayTriple[1] = AskTripleParameter(1, parameters.SensorValue[0], 3, parameters.SensorValue[2], 5, parameters.SensorValue[4]);
            stateArrayTriple[2] = AskTripleParameter(1, parameters.SensorValue[0], 4, parameters.SensorValue[3], 5, parameters.SensorValue[4]);

            return ParseStateArrayToState(stateArrayTriple);
        }

        public State AskQuad(SensorSetValues parameters)
        {
            State[] stateArrayQuad = new State[2];

            stateArrayQuad[0] = AskQuadParameter(1, parameters.SensorValue[0], 2, parameters.SensorValue[1], 4, parameters.SensorValue[3], 5, parameters.SensorValue[4]);
            stateArrayQuad[1] = AskQuadParameter(2, parameters.SensorValue[1], 3, parameters.SensorValue[2], 3, parameters.SensorValue[2], 5, parameters.SensorValue[4]);

            return ParseStateArrayToState(stateArrayQuad);
        }

        private State ParseStateArrayToState(State[] stateArray)
        {
            if (stateArray.Any(x => x.Status == State.InternalStatus.Alarm))
            {
                return State.Alarm();
            }
            else if (stateArray.Any(x => x.Status == State.InternalStatus.Warning))
            {
                return State.Warning();
            }
            else
            {
                return State.Good();
            }
        }

        public State AskFifth(SensorSetValues parameters)
        {
            return AskFifthParameter(1, parameters.SensorValue[0], 2, parameters.SensorValue[1], 3, parameters.SensorValue[2], 4, parameters.SensorValue[3], 5, parameters.SensorValue[4]);
        }

        public State AskPairParameter(int parameterA, int valueA, int parameterB, int valueB)
        {
            string sparqlQuery = CreateSparqlQueryPair(parameterA, valueA, parameterB, valueB);
            var q = queryParser.ParseFromString(sparqlQuery);
            SparqlResultSet resultSet = graph.ExecuteQuery(q) as SparqlResultSet;

            return ParseSparqlResultSetToState(resultSet);
        }

        public State AskTripleParameter(int parameterA, int valueA, int parameterB, int valueB, int parameterC, int valueC)
        {
            string sparqlQuery = CreateSparqlQueryTriple(parameterA, valueA, parameterB, valueB, parameterC, valueC);
            var q = queryParser.ParseFromString(sparqlQuery);
            SparqlResultSet resultSet = graph.ExecuteQuery(q) as SparqlResultSet;

            return ParseSparqlResultSetToState(resultSet);
        }

        public State AskQuadParameter(int parameterA, int valueA, int parameterB, int valueB, int parameterC, int valueC, int parameterD, int valueD)
        {
            string sparqlQuery = CreateSparqlQueryQuad(parameterA, valueA, parameterB, valueB, parameterC, valueC, parameterD, valueD);
            var q = queryParser.ParseFromString(sparqlQuery);
            SparqlResultSet resultSet = graph.ExecuteQuery(q) as SparqlResultSet;

            return ParseSparqlResultSetToState(resultSet);
        }

        public State AskFifthParameter(int parameterA, int valueA, int parameterB, int valueB, int parameterC, int valueC, int parameterD, int valueD, int parameterE, int valueE)
        {
            string sparqlQuery = CreateSparqlQueryFifth(parameterA, valueA, parameterB, valueB, parameterC, valueC, parameterD, valueD, parameterE, valueE);
            var q = queryParser.ParseFromString(sparqlQuery);
            SparqlResultSet resultSet = graph.ExecuteQuery(q) as SparqlResultSet;

            return ParseSparqlResultSetToState(resultSet);
        }

        public State ParseSparqlResultSetToState(SparqlResultSet resultSet)
        {
            if (resultSet != null)
            {
                if (resultSet.Count > 1)
                {
                    return State.Alarm();
                }
                else if (resultSet.Count == 1)
                {
                    SparqlResult result = resultSet[0];
                    var resultString = result["state"].ToString();
                    OnNewLogMessage(result.ToString().Substring(29));
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
