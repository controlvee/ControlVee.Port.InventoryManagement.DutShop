﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlVee.Port.InventoryManagement.DutShop.Models
{
    [Serializable]
    public class CreateBatchModel
    {
        public string ID;
        public string nameOf;
        public int total;
    }
}
