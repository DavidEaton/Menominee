using MenomineePlayWASM.Shared.Entities.Scheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Telerik.Blazor;

namespace MenomineePlayWASM.Client.Pages.Schedule
{
    public partial class Scheduler
    {
        public DateTime StartDate { get; set; } = DateTime.Today;
        public SchedulerView CurrView { get; set; } = SchedulerView.Timeline;
        public DateTime DayStart { get; set; } = new DateTime(2000, 1, 1, 7, 0, 0);//the time portion is important
        public DateTime DayEnd { get; set; } = new DateTime(2000, 1, 1, 18, 0, 0);//the time portion is important

        //List<SchedulerAppointmentDetail> Appointments = new List<SchedulerAppointmentDetail>()
        //{
        //    new SchedulerAppointmentDetail
        //    {
        //        JobCode = "B",
        //        Description = "The cat needs vaccinations and her teeth checked.",
        //        Start = new DateTime(2021, 11, 26, 11, 30, 0),
        //        End = new DateTime(2021, 11, 26, 12, 0, 0)
        //    },

        //    new SchedulerAppointmentDetail
        //    {
        //        JobCode = "E",
        //        Description = "Kick off the new project.",
        //        Start = new DateTime(2021, 11, 25, 9, 30, 0),
        //        End = new DateTime(2021, 11, 25, 12, 45, 0)
        //    },

        //    new SchedulerAppointmentDetail
        //    {
        //        JobCode = "L",
        //        Description = "An unforgettable holiday!",
        //        Start = new DateTime(2021, 11, 27),
        //        End = new DateTime(2021, 12, 07)
        //    }
        //};

        //public class SchedulerAppointment
        //{
        //    public string Title { get; set; }
        //    public string Description { get; set; }
        //    public DateTime Start { get; set; }
        //    public DateTime End { get; set; }
        //    public bool IsAllDay { get; set; }
        //}

        List<AppointmentData> Appointments = new List<AppointmentData>()
        {
            new AppointmentData { Id=1, TeamId=1, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10,0,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 11,30,0), TechId=2, Vehicle="2019 Ford Focus", ServiceId=1},
            new AppointmentData { Id=2, TeamId=2, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 13,30,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 15,30,0), TechId=3, Vehicle="2018 Honda Civic", ServiceId=2},
            new AppointmentData { Id=3, TeamId=3, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 15,30,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 16,0,0), TechId=5, Vehicle="2015 Toyota Prius", ServiceId=3},
            new AppointmentData { Id=4, TeamId=1, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 8,0,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 9,0,0), TechId=1, Vehicle="2011 GMC Sierra", ServiceId=4},
            new AppointmentData { Id=5, TeamId=2, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 7,30,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10,0,0), TechId=4, Vehicle="2009 Dodge Durango", ServiceId=5},
            new AppointmentData { Id=6, TeamId=3, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 9,30,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 11,0,0), TechId=6, Vehicle="2017 Chevrolet Silverado", ServiceId=8},
            new AppointmentData { Id=7, TeamId=1, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12,30,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 14,0,0), TechId=1, Vehicle="2007 Volkswagon Jetta", ServiceId=7},
            new AppointmentData { Id=8, TeamId=2, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 16,30,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 17,0,0), TechId=3, Vehicle="2005 Kia Sportage", ServiceId=6},
            new AppointmentData { Id=9, TeamId=3, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 14,30,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 16,0,0), TechId=6, Vehicle="2001 Mazda 626", ServiceId=9},
            new AppointmentData { Id=10, TeamId=1, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 9,0,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10,0,0), TechId=2, Vehicle="2019 Ford Focus", ServiceId=8},
            new AppointmentData { Id=11, TeamId=2, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10,30,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 13,0,0), TechId=3, Vehicle="2018 Honda Civic", ServiceId=4},
            new AppointmentData { Id=12, TeamId=3, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 8,0,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 9,30,0), TechId=5, Vehicle="2015 Toyota Prius", ServiceId=1},
            new AppointmentData { Id=13, TeamId=1, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 9,0,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 11,0,0), TechId=1, Vehicle="2011 GMC Sierra", ServiceId=3},
            new AppointmentData { Id=14, TeamId=2, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 13,30,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 15,0,0), TechId=4, Vehicle="2009 Dodge Durango", ServiceId=7},
            new AppointmentData { Id=15, TeamId=3, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 7,0,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 8,30,0), TechId=6, Vehicle="2017 Chevrolet Silverado", ServiceId=2},
            new AppointmentData { Id=16, TeamId=1, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 11,30,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12,30,0), TechId=1, Vehicle="2007 Volkswagon Jetta", ServiceId=8},
            new AppointmentData { Id=17, TeamId=2, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 17,00,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 18,0,0), TechId=4, Vehicle="2007 Buick Encore", ServiceId=6},
            new AppointmentData { Id=18, TeamId=3, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 11,30,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 14,0,0), TechId=6, Vehicle="2001 Mazda 626", ServiceId=10},
            new AppointmentData { Id=19, TeamId=1, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12,0,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 18,0,0), TechId=2, Vehicle="", ServiceId=12},
            new AppointmentData { Id=20, TeamId=2, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12,0,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 13,0,0), TechId=4, Vehicle="", ServiceId=11},
            new AppointmentData { Id=21, TeamId=3, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12,0,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 13,0,0), TechId=5, Vehicle="", ServiceId=11}
        };

        //private string[] GroupData = new string[] { "Teams", "Techs" };

        //private List<ResourceData> TeamData { get; set; } = new List<ResourceData>
        //{
        //    new ResourceData {Text = "Team Ben", Id= 1, Color= "#cb6bb2"},
        //    new ResourceData {Text = "Team Josh", Id= 2, Color= "#56ca85"},
        //    new ResourceData {Text = "Team Rob", Id= 3, Color= "#df5286"}
        //};

        private List<ResourceData> TechData { get; set; } = new List<ResourceData>
        {
            new ResourceData { Text = "Phil", Id = "1", TeamId = 1, Color = "#df5286" },
            new ResourceData { Text = "Steven", Id = "2", TeamId = 1, Color = "#7fa900" },
            new ResourceData { Text = "Robert", Id = "3", TeamId = 2, Color = "#ea7a57" },
            new ResourceData { Text = "Samuel", Id = "4", TeamId = 2, Color = "#5978ee" },
            new ResourceData { Text = "Michael", Id = "5", TeamId = 3, Color = "#df5286" },
            new ResourceData { Text = "Larry", Id = "6", TeamId = 3, Color = "#00bdae" }
        };

        private List<Service> Services { get; set; } = new List<Service>
        {
            new Service { Id = 1, Text = "Air Conditioning", BackgroundColor = "blue", TextColor = "white" },
            new Service { Id = 2, Text = "Alignment", BackgroundColor = "yellow" },
            new Service { Id = 3, Text = "Battery", BackgroundColor = "orange" },
            new Service { Id = 4, Text = "Brakes", BackgroundColor = "lightblue" },
            new Service { Id = 5, Text = "Exhaust", BackgroundColor = "burlywood" },
            new Service { Id = 6, Text = "LOF", BackgroundColor = "cadetblue", TextColor = "white" },
            new Service { Id = 7, Text = "Shocks/Struts", BackgroundColor = "cornflowerblue", TextColor = "white" },
            new Service { Id = 8, Text = "Starting/Charging", BackgroundColor = "darkseagreen" },
            new Service { Id = 9, Text = "Tires", BackgroundColor = "plum" },
            new Service { Id = 10, Text = "Transmission", BackgroundColor = "green", TextColor = "white" },
            new Service { Id = 11, Text = "Lunch", BackgroundColor = "darkblue", TextColor = "white" },
            new Service { Id = 12, Text = "Off", BackgroundColor = "black", TextColor = "white" }
        };

        public class Service
        {
            public long Id { get; set; }
            public string Text { get; set; }
            public string TextColor { get; set; } = "black";
            public string BackgroundColor { get; set; }
        }

        public class ResourceData
        {
            public string Text { get; set; }
            public string Id { get; set; }
            public int TeamId { get; set; }
            public string Color { get; set; }
        }

        public class AppointmentData
        {
            public long Id { get; set; }
            public int TeamId { get; set; }
            public int TechId { get; set; }
            public string Service { get; set; }
            [Required]
            public int ServiceId { get; set; }
            public string Notes { get; set; }
            [Required]
            public DateTime? StartTime { get; set; }
            [Required]
            public DateTime? EndTime { get; set; }
            public string Vehicle { get; set; }
            public bool IsAllDay { get; set; } = false;
        }

    }
}
