using System;
using System.Collections.Generic;
using System.Text;

namespace ControlVee.Port.InventoryManagement.DutShop.Models
{
    // TODO: How does this attri work?
    [Serializable]
    public class BatchModel 
    {
        public string ID { get; set; }
        public string NameOf { get; set; }
        public DateTime Started { get; set; }
        public int Total { get; set; }
        public BatchModel()
        {
        }
    }
}
