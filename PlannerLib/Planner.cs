using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlannerLib.DetroitDataSetTableAdapters;
using System.Data;

namespace PlannerLib
{
    public enum PlanMode
    {
        Day = 1,
        Month = 2
    }


    public class Planner
    {
        int lineId;
        private int lineTaktLengthInMinutes;
        //private SimLine simLine;
        private string databaseConnectionString;


        public Planner(int lineId, int lineTaktLengthInMinutes)
        {
            this.lineId = lineId;
            this.lineTaktLengthInMinutes = lineTaktLengthInMinutes;
            if (this.lineTaktLengthInMinutes <= 0) 
            {
                throw new Exception("Line takt length must be greater 0!");
            }
        }

        public Planner(int lineId, string databaseConnectionString)
            : this(lineId, 40)
        {
            this.databaseConnectionString = databaseConnectionString;
        }


        public void RunSimulation()
        {
            //this.simLine = new SimLine(this.databaseConnectionString);
            //this.simLine.Init(this.lineId);
            //this.simLine.Execute();
            //while (!this.simLine.isFinished)
            //{
            //    this.simLine.Move();
            //}
        }

        public void MakeProductPlan(PlanMode planMode) 
        {
        }


        public void MakeProductPlanInTakts(PlanMode planMode)
        {
        }

        public void FillScheduleFramesByDefault(DateTime enDay) 
        {
            DetroitDataSetTableAdapters.SchedulerFrameDefaultTableAdapter schedulerFrameDefaultTableAdapter = new SchedulerFrameDefaultTableAdapter();
            
            
            int curMonth = enDay.Month;
            int i = 0;
            while (curMonth == enDay.Month) 
            {
                schedulerFrameDefaultTableAdapter.schedulerFramesPasteDefault(enDay.AddDays(i), 0);
                i++;
                curMonth = enDay.AddDays(i).Month;
            }
            
        }

    }
}
