using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;


namespace LineService
{

    [Serializable]
    class TCSavedData
    {
        public SerializableDictionary<string, int> Counters;
    }    
    
    
    [Serializable]
    public class TimersController
    {
        private Dictionary<string, Counter> timers = new Dictionary<string, Counter>();
        private List<Counter> pausedTimers = new List<Counter>();
        private List<string> asynchStartList = new List<string>();
        private string id;

        public TimersController(string id)
        {
            this.id = id;
        }

        public Counter Add(TimerKey name, int startValue, int direction, int step, TimerCounterType type)
        {
            Counter newTimer = new SuperCounter(direction, startValue, step, type);
            this.timers.Add(name.ToString(), newTimer);
            return newTimer;
        }

        public void Start(string name)
        {
            Counter aTimer = (Counter)this.timers[name];
            if (aTimer != null)
            {
                aTimer.Start();
            }
        }


        public int Stop(string name)
        {
            int result = 0;
            Counter aTimer = (Counter)this.timers[name];
            if (aTimer != null)
            {
                aTimer.Stop();
                result = aTimer.GetIntValue();
            }
            return result;
        }

        public void PauseNonCritical()
        {
            string[] keys = new string[this.timers.Count];
            this.timers.Keys.CopyTo(keys, 0);

            for (int i = 0; i < this.timers.Count; i++)
            {
                Counter aCounter = (Counter)this.timers[keys[i]];
                if (keys[i].Substring(0, 4) == "PART" && aCounter.Enabled)
                {
                    this.pausedTimers.Add(aCounter);
                    aCounter.Pause();
                }
            }
        }

        public void PauseAll()
        {
            string[] keys = new string[this.timers.Count];
            this.timers.Keys.CopyTo(keys, 0);

            for (int i = 0; i < this.timers.Count; i++)
            {
                Counter aCounter = (Counter)this.timers[keys[i]];
                if (aCounter.Enabled)
                {
                    this.pausedTimers.Add(aCounter);
                    aCounter.Pause();
                }
            }
        }

        public void StopAll()
        {
            string[] keys = new string[this.timers.Count];
            this.timers.Keys.CopyTo(keys, 0);

            for (int i = 0; i < this.timers.Count; i++)
            {
                Counter aCounter = (Counter)this.timers[keys[i]];
                if (aCounter.Enabled)
                {
                    this.pausedTimers.Add(aCounter);
                    aCounter.Stop();
                }
            }
        }

        public void StopForRelease()
        {
            string[] keys = new string[this.timers.Count];
            this.timers.Keys.CopyTo(keys, 0);

            List<string> excludeKeys = new List<string>() { "HELP", "PART", "STOP" };

            for (int i = 0; i < this.timers.Count; i++)
            {
                Counter aCounter = (Counter)this.timers[keys[i]];
                if (aCounter.Enabled && !excludeKeys.Contains(keys[i].Substring(0, 4)))
                {
                    this.pausedTimers.Add(aCounter);
                    aCounter.Stop();
                }
            }
        }

        public void ContinueAll()
        {
            int i = 0;
            while (i < this.pausedTimers.Count)
            {
                this.pausedTimers[i].Start();
                i++;
            }
            this.pausedTimers.Clear();
        }

        public int Value(string name)
        {
            int result = 0;
            Counter aTimer = (Counter)this.timers[name];
            if (aTimer != null)
            {
                result = aTimer.GetIntValue();
            }
            return result;
        }

        public void Reset(string name)
        {
            Counter aTimer = (Counter)this.timers[name];
            if (aTimer != null)
            {
                aTimer.Reset();
            }
        }

        public void Reset(string name, int offset)
        {
            Counter aTimer = (Counter)this.timers[name];
            if (aTimer != null)
            {
                aTimer.Reset(offset);
            }
        }

        public bool ContainsKey(string key)
        {
            return timers.ContainsKey(key);
        }

        public string[] KeyList()
        {
            string[] result = new string[this.timers.Keys.Count];
            this.timers.Keys.CopyTo(result, 0);
            return result;
        }

        public void Backup() 
        {
            TCSavedData dataToSave = new TCSavedData();
            dataToSave.Counters = new SerializableDictionary<string, int>();

            try
            {
                string prefix = "on-";
                foreach (KeyValuePair<string, Counter> item in this.timers)
                {
                    string left4 = item.Key.Substring(0, 4);
                    if (left4 != "PART")
                    {
                        dataToSave.Counters.Add(item.Key, (item.Value as Counter).GetIntValue());

                        if ((item.Value as Counter).Enabled)
                        {
                            dataToSave.Counters.Add(prefix + item.Key, 1);
                        }
                    }

                }

                Serialization sAgent = new Serialization("serialization_tc" + this.id + ".dat");
                sAgent.Backup(dataToSave);
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now + " " + ex.TargetSite.ToString(), ex.Source, ex.ToString());
            }
        }

        public void Restore() 
        {
            Serialization sAgent = new Serialization("serialization_tc" + this.id + ".dat");
            TCSavedData restoredData = new TCSavedData();

            try
            {
                restoredData = sAgent.Restore(restoredData);

                if (restoredData.Counters != null)
                {
                    foreach(KeyValuePair<string, int> restoredItem in restoredData.Counters) 
                    {
                        if(this.timers.ContainsKey(restoredItem.Key)) 
                        {
                            this.timers[restoredItem.Key].SetValue(restoredItem.Value);
                        }

                        string prefix = "on-";
                        string onKey;
                        if (restoredItem.Key.Length > prefix.Length)
                        {
                            onKey = restoredItem.Key.Substring(prefix.Length);
                            if (this.timers.ContainsKey(onKey))
                            {
                                this.timers[onKey].Start();
                            }
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now + " " + ex.TargetSite.ToString(), ex.Source, ex.ToString());
            }

        }

    }


}
