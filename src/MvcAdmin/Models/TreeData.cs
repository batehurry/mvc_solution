using System.Collections.Generic;

namespace MvcAdmin.Models
{
    public class TreeData
    {
        public decimal id { get; set; }
        public string text { get; set; }
        public string state { get; set; }
        //类型
        public decimal type { get; set; }
        //public bool Checked { get; set; }
        public List<TreeData> children { get; set; }
    }
}