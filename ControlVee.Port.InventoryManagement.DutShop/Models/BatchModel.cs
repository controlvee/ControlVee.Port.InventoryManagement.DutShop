using System;
using System.Collections.Generic;
using System.Text;

namespace ControlVee.Port.InventoryManagement.DutShop.Models
{
    public class BatchModel 
    {
        public string ID { get; set; }
        public string NameOf { get; set; }
        public int Total { get; set; }
        public DateTime Completion { get; set; }
        public DateTime Expire { get; set; }
        public int TotalSold { get; set; }
        public int HasExpired { get; set; }
    }
}
