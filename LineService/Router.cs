using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace LineService
{
    public class Router
    {
        protected string startState = "START_STATE";
        protected string finishState = "FINISH_STATE";
        protected string state = "";
        protected string backupState = "";

        protected Hashtable transTable = new Hashtable();

        public Router() : this(0) { }

        public Router(int transTableCode) 
        {
            this.state = startState;
            switch (transTableCode)
            {
                case 1:
                    this.fillTransTable_1();
                    break;
                default:
                    this.fillTransTable_0();
                    break;
            }        
        }

        public void Run() 
        {
            //state = Convert.ToString(transTable[state + "-" + startState]);
        }

        public string FindNextState(string symblol) 
        {
            string result = "";
            object nextValue = transTable[state + "-" + symblol];
            if(nextValue != null) 
            {
                result = nextValue.ToString();
            }
            return result;
        }

        public string State { get { return state; } }

        public virtual string Go(string symbol) 
        {
            string result = "";
            string newState = FindNextState(symbol);
            if (newState != "") 
            {
                state = newState;
                result = state;
            }

            return result;
        }

        public virtual void Backup() 
        {
            backupState = state;
        }

        public virtual void Restore() 
        {
            if (backupState != "")
                state = backupState;
        }

        public virtual void Restore(string state) 
        {
            if (state != null && state != "") 
            {
                if (transTable.ContainsValue(state))
                {
                    this.state = state;
                }
                else 
                {
                    throw new Exception("Attempting to set incorrect product router state! (State = " + state);
                }
            }
        }

        public bool IsFinalState { 
            get 
            { 
                return state == finishState; 
            } 
        }

        public bool IsNextStateFinal
        {
            get
            {
                bool result = false;
                string nextState = this.FindNextState(this.state);
                result = nextState == finishState;
                return result;
            }
        }


        protected virtual void fillTransTable_0() 
        {
            transTable.Add(startState + "-" + startState, "A");
            transTable.Add("A-A", "B1B2");
            transTable.Add("B1B2-B1", "B2C");
            transTable.Add("B1B2-B2", "B1C");
            transTable.Add("B1C-B1", "C");
            transTable.Add("B2C-B2", "C");
            transTable.Add("C-C", "D1D2D3");
            transTable.Add("D1D2D3-D1", "D2D3E");
            transTable.Add("D1D2D3-D2", "D1D3E");
            transTable.Add("D1D2D3-D3", "D1D2E");

            transTable.Add("D2D3E-D2", "D3E");
            transTable.Add("D2D3E-D3", "D2E");

            transTable.Add("D1D3E-D1", "D3E");
            transTable.Add("D1D3E-D3", "D1E");

            transTable.Add("D1D2E-D1", "D2E");
            transTable.Add("D1D2E-D2", "D1E");


            transTable.Add("D1E-D1", "E");
            transTable.Add("D2E-D2", "E");
            transTable.Add("D3E-D3", "E");

            transTable.Add("E-E", "F");
            transTable.Add("F-F", "FINISH_STATE");        
        }
        protected virtual void fillTransTable_1() 
        {
            transTable.Add(startState + "-" + startState, "FA0.1");
            transTable.Add("FA0.1-FA0.1", "FA0.2");
            transTable.Add("FA0.2-FA0.2", "FA1.1");
            transTable.Add("FA1.1-FA1.1", "FA1.2+PFA2");
            transTable.Add("FA1.2+PFA2-FA1.2", "FA2.1+PFA2");
            transTable.Add("FA1.2+PFA2-PFA2", "FA2.1+FA1.2");
            transTable.Add("FA2.1+PFA2-PFA2", "FA2.1");
            transTable.Add("FA2.1+FA1.2-FA1.2", "FA2.1");
            transTable.Add("FA2.1-FA2.1", "FA2.2");
            transTable.Add("FA2.2-FA2.2", "FINISH_STATE");    
        }

    }



    public class ProductRouter : Router 
    {
        protected Hashtable stateStationsTable = new Hashtable();
        protected Hashtable symbolStationsTable = new Hashtable();
        protected int nextLineId;
        protected int nextLineAuto;
        protected int taktDuration;
     
        public ProductRouter() : this(0) { }

        public ProductRouter(int transTableCode)
            : base(transTableCode)
        {
            switch (transTableCode)
            {
                case 1:
                    this.fillStationTable_1();
                    break;
                default:
                    this.fillStationTable_0();
                    break;
            }
        }

        public List<string> FindStateStations() 
        {
            List<string> stations = (List<string>)(stateStationsTable[this.state]);
            if (stations != null)
            {
                return stations;
            }
            else
            {
                return new List<string>();
            }
        }
        public override string Go(string stationName) 
        {
            string result = "";

            string symbol = Convert.ToString(symbolStationsTable[stationName]);
            string newState = base.Go(symbol);

            if (newState != "")
            {
                List<string> newStations = FindStateStations();
                if (newStations != null && newStations.Count > 0)
                {
                    result = newStations.Aggregate((stations, next) => stations + ", " + next);
                }
            }
            return result;
        }
        public int NextLineId { get { return this.nextLineId; } set { this.nextLineId = value; } }
        public int NextLineAuto { get { return this.nextLineAuto; } set { this.nextLineAuto = value; } }
        public List<string> NextStationNames(string stationName) 
        {
            List<string> result;
            this.Backup();
            this.Go(stationName);
            result = this.FindStateStations();
            this.Restore();
            return result;
        }
        public int TaktDuration { get { return this.taktDuration; } }

        protected virtual void fillStationTable_0() 
        {
            stateStationsTable.Add("START_STATE", new List<string> { "START_STATE" });
            stateStationsTable.Add("A", new List<string> { "Station A" });
            stateStationsTable.Add("B1B2", new List<string> { "Station B1", "Station B2" });
            stateStationsTable.Add("B1C", new List<string> { "Station B1", "Station C" });
            stateStationsTable.Add("B2C", new List<string> { "Station B2", "Station C" });
            stateStationsTable.Add("C", new List<string> { "Station C" });
            stateStationsTable.Add("D1D2D3", new List<string> { "Station D1", "Station D2", "Station D3" });
            stateStationsTable.Add("D1D2E", new List<string> { "Station D1", "Station D2", "Station E" });
            stateStationsTable.Add("D1D3E", new List<string> { "Station D1", "Station D3", "Station E" });
            stateStationsTable.Add("D2D3E", new List<string> { "Station D2", "Station D3", "Station E" });
            stateStationsTable.Add("D1E", new List<string> { "Station D1", "Station E" });
            stateStationsTable.Add("D2E", new List<string> { "Station D2", "Station E" });
            stateStationsTable.Add("D3E", new List<string> { "Station D3", "Station E" });
            stateStationsTable.Add("E", new List<string> { "Station E" });
            stateStationsTable.Add("F", new List<string> { "Station F" });

            symbolStationsTable.Add("START_STATE", "START_STATE");
            symbolStationsTable.Add("Station A", "A");
            symbolStationsTable.Add("Station B1", "B1");
            symbolStationsTable.Add("Station B2", "B2");
            symbolStationsTable.Add("Station C", "C");
            symbolStationsTable.Add("Station D", "D");
            symbolStationsTable.Add("Station D1", "D1");
            symbolStationsTable.Add("Station D2", "D2");
            symbolStationsTable.Add("Station D3", "D3");
            symbolStationsTable.Add("Station E", "E");
            symbolStationsTable.Add("Station F", "F");        
        }
        protected virtual void fillStationTable_1()
        {
            stateStationsTable.Add("START_STATE", new List<string> { "START_STATE" });
            stateStationsTable.Add("FA0.1", new List<string> { "FA0.1" });
            stateStationsTable.Add("FA0.2", new List<string> { "FA0.2" });
            stateStationsTable.Add("FA1.1", new List<string> { "FA1.1" });
            stateStationsTable.Add("FA1.2+PFA2", new List<string> { "FA1.2", "PFA2" });
            stateStationsTable.Add("FA2.1+PFA2", new List<string> { "FA2.1", "PFA2" });
            stateStationsTable.Add("FA2.1+FA1.2", new List<string> { "FA2.1", "FA1.2" });
            stateStationsTable.Add("FA2.1", new List<string> { "FA2.1" });
            stateStationsTable.Add("FA2.2", new List<string> { "FA2.2" });

            symbolStationsTable.Add("START_STATE", "START_STATE");
            symbolStationsTable.Add("FA0.1", "FA0.1");
            symbolStationsTable.Add("FA0.2", "FA0.2");
            symbolStationsTable.Add("FA1.1", "FA1.1");
            symbolStationsTable.Add("PFA2", "PFA2");
            symbolStationsTable.Add("FA1.2", "FA1.2");
            symbolStationsTable.Add("FA2.1", "FA2.1");
            symbolStationsTable.Add("FA2.2", "FA2.2");

            nextLineId = 2;
            nextLineAuto = 1;
            taktDuration = 2;
         }

    }

    public class DbProductRouter : ProductRouter
    {
        DetroitDataSet detroitDataSet;
        DetroitDataSetTableAdapters.BatchMapPointTableAdapter batchMapPointTableAdapter;
        DetroitDataSetTableAdapters.BatchTypeMapTableAdapter batchTypeMapTableAdapter;
        int typeId;
        int lineId;


        public DbProductRouter(int transTableCode, DetroitDataSet detroit, int typeId)
            : base(0)
        {
            this.detroitDataSet = detroit;
            this.typeId = typeId;
            this.lineId = detroit.LineId;

            this.transTable.Clear();
            this.stateStationsTable.Clear();
            this.symbolStationsTable.Clear();

            this.fillTransTable_1();
            this.fillStationTable_1();

        }

        protected override void fillStationTable_1()
        {
            //string state = trans.Substring(0, trans.IndexOf("+"));


            stateStationsTable.Add("START_STATE", new List<string> { "START_STATE" });
            symbolStationsTable.Add("START_STATE", "START_STATE");

            foreach (string trans in this.transTable.Values) 
            {
                List<string> states = trans.Split('+').ToList<string>();
                if( !stateStationsTable.ContainsKey(trans))
                    stateStationsTable.Add(trans, states);

                foreach (string station in states) 
                {
                    if (!symbolStationsTable.ContainsKey(station)) 
                    {
                        symbolStationsTable.Add(station, station);    
                    }
                } 
            }
            //stateStationsTable.Add("START_STATE", new List<string> { "START_STATE" });
            //stateStationsTable.Add("FA0.1", new List<string> { "FA0.1" });
            //stateStationsTable.Add("FA0.2", new List<string> { "FA0.2" });
            //stateStationsTable.Add("FA1.1", new List<string> { "FA1.1" });
            //stateStationsTable.Add("FA1.2+PFA2", new List<string> { "FA1.2", "PFA2" });
            //stateStationsTable.Add("FA2.1+PFA2", new List<string> { "FA2.1", "PFA2" });
            //stateStationsTable.Add("FA2.1+FA1.2", new List<string> { "FA2.1", "FA1.2" });
            //stateStationsTable.Add("FA2.1", new List<string> { "FA2.1" });
            //stateStationsTable.Add("FA2.2", new List<string> { "FA2.2" });



            //symbolStationsTable.Add("START_STATE", "START_STATE");
            //symbolStationsTable.Add("FA0.1", "FA0.1");
            //symbolStationsTable.Add("FA0.2", "FA0.2");
            //symbolStationsTable.Add("FA1.1", "FA1.1");
            //symbolStationsTable.Add("PFA2", "PFA2");
            //symbolStationsTable.Add("FA1.2", "FA1.2");
            //symbolStationsTable.Add("FA2.1", "FA2.1");
            //symbolStationsTable.Add("FA2.2", "FA2.2");

            //this.nextLineId = 2;
            //this.nextLineAuto = 1;
            //this.taktDuration = 2;
        }

        protected override void fillTransTable_1()
        {
            //transTable.Add(this.startState + "-" + this.startState, "FA0.1");
            //transTable.Add("FA0.1-FA0.1", "FA0.2");
            //transTable.Add("FA2.2-FA2.2", "FINISH_STATE");

            //this.owner = owner;
            //this.line = line;
            //this.myLog = logProvider;
            //int line_capacity = line.GetStations().Count();


            // connect to database
            this.detroitDataSet = new DetroitDataSet();
            this.batchMapPointTableAdapter = new DetroitDataSetTableAdapters.BatchMapPointTableAdapter();
            this.batchMapPointTableAdapter.FillByBatchTypeId(this.detroitDataSet.BatchMapPoint, this.typeId, this.lineId);

            this.batchTypeMapTableAdapter = new DetroitDataSetTableAdapters.BatchTypeMapTableAdapter();
            this.batchTypeMapTableAdapter.FillBy(this.detroitDataSet.BatchTypeMap, this.typeId, this.lineId);

            if (this.detroitDataSet.BatchTypeMap.Rows.Count > 0)
            {
                object objNextLineId = this.detroitDataSet.BatchTypeMap.Rows[0]["NextLineId"];
                object objNextLineAuto = this.detroitDataSet.BatchTypeMap.Rows[0]["NextLineAuto"];
                object objTakt = this.detroitDataSet.BatchTypeMap.Rows[0]["Takt"];
                if (objNextLineId != null && objNextLineId != DBNull.Value)
                {
                    this.nextLineId = Convert.ToInt32(objNextLineId);
                    if (objNextLineAuto != null && objNextLineAuto != DBNull.Value)
                    {
                        this.nextLineAuto = Convert.ToInt32(objNextLineAuto);
                    }
                }
                else
                {
                    this.nextLineId = 0;
                    this.nextLineAuto = 0;
                }

                if (objTakt != DBNull.Value)
                {
                    this.taktDuration = Convert.ToInt32(objTakt);
                }
                else
                {
                    this.taktDuration = 1;
                }

            }

            //transTable.Add(this.startState + "-" + this.startState, "FA0.1");

            // create station structure
            //----------------------------------------------------------------------
            List<StationGrafNode> stationGraf = new List<StationGrafNode>();
            for (int i = 0; i <= this.detroitDataSet.BatchMapPoint.Rows.Count - 1; i++)
            {
                // read key data from databse
                DataRow mapPointRow = this.detroitDataSet.BatchMapPoint.Rows[i];
                int step = (int)mapPointRow["Step"];
                int stationId = (int)mapPointRow["StationId"];
                int isMain = (int)mapPointRow["IsMain"];
                string stationName = mapPointRow["StationName"].ToString();

                // TODO: construct trans table
                if (isMain == 1)
                {
                    stationGraf.Add(new StationGrafNode() { StationName = stationName, Step = step });
                }
                else
                {
                    StationGrafNode mainStepStation = stationGraf.FirstOrDefault(p => p.Step.Equals(step));
                    if (mainStepStation != null)
                    {
                        if (mainStepStation.AssistSationNames == null)
                        {
                            mainStepStation.AssistSationNames = new List<string>();
                        }
                        mainStepStation.AssistSationNames.Add(stationName);
                    }
                }

            }

            // process station structure
            //-------------------------------------------------------------------------
            string state = this.startState;
            string symbol = this.startState;
            string newState = "";
            for (int i = 0; i < stationGraf.Count; i++)
            {
                StationGrafNode stationNode = stationGraf[i];
                if (stationNode.AssistSationNames == null || stationNode.AssistSationNames.Count == 0)
                {
                    newState = stationNode.StationName;
                    this.transTable.Add(state + "-" + symbol, newState);

                    state = newState;
                    symbol = newState;
                }
                else
                {
                    newState = stationNode.StationName;
                    List<string> stationNames = new List<string>();
                    stationNames.Add(stationNode.StationName);

                    for (int j = 0; j < stationNode.AssistSationNames.Count; j++)
                    {
                        newState = newState + "+" + stationNode.AssistSationNames[j].ToString();
                        stationNames.Add(stationNode.AssistSationNames[j].ToString());
                    }
                    this.transTable.Add(state + "-" + symbol, newState);

                    List<Transition> transitions = this.getTransitions(stationNames, stationGraf[i + 1].StationName);
                    foreach(Transition transition in transitions) 
                    {
                        this.transTable.Add(transition.State + "-" + transition.Symbol, transition.NewState);
                    }

                    state = stationGraf[i+1].StationName;
                    symbol = state;
                    i++;

                }
            }

            this.transTable.Add(state + "-" + symbol, this.finishState);


        }


        public void RefreshDataFromDb() 
        {
            this.batchTypeMapTableAdapter.FillBy(this.detroitDataSet.BatchTypeMap, this.typeId, this.lineId);
            object objTakt = this.detroitDataSet.BatchTypeMap.Rows[0]["Takt"];
            if (objTakt != DBNull.Value)
            {
                this.taktDuration = Convert.ToInt32(objTakt);
            }        
        }



        private List<Transition> getTransitions(List<string> states, string nextState)
        {
            List<Transition> result = new List<Transition>();
            string state = states.Aggregate((workingSentence, next) => workingSentence + "+" + next);
            string newState = "";

            states.Remove(nextState);
            for (int i = 0; i < states.Count; i++)
            {
                    List<string> tmpA = new List<string>(states);
                    tmpA.Remove(states[i]);
                    if (tmpA.Count > 0)
                    {
                        newState = tmpA.Aggregate((workingSentence, next) => workingSentence + "+" + next);
                        newState = newState + "+" + nextState;
                    }
                    else
                    {
                        newState = nextState;
                    }

                    result.Add(new Transition() { State = state, Symbol = states[i], NewState = newState });
                    if (tmpA.Count > 0)
                    {
                        tmpA.Add(nextState);
                        result.AddRange(getTransitions(tmpA, nextState));
                    }
            }



            return result;
        }

    }

    class StationGrafNode 
    {
        public string StationName;
        public int Step;
        public List<string> AssistSationNames;
    }

    class Transition 
    {
        public string State;
        public string Symbol;
        public string NewState;
    }





}

