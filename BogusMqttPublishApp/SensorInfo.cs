using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SensorInfo
    {
        public string Dev_Id { get; set; }
        public string Curr_Time { get; set; }
        public float Temp { get; set; }
        public float Humid { get; set; }
        public float Press { get; set; }
    }
}
