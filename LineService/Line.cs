using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization; 

namespace LineService
{
    [Serializable]
    public abstract class Line
    {
        private int id;
        private string name;

        [NonSerialized]
        private BatchesOnLine batchesOnLine;

        public Line() { }

        public virtual int Id { get { return this.id;} }
        public virtual string Name { get { return this.name; } }
        public virtual BatchesOnLine BatchesOnLine { get { return this.batchesOnLine;} }

        public abstract LineStationBase GetStation(int stationId);
        public abstract List<LineStationBase> GetStations();
        
    }
}
