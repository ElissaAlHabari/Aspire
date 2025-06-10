using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public enum InventoryStatus
    {
        inStock,
        lowStock,
        Ordered,
        Discontinued
    }
    public class Inventory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public InventoryStatus Status { get; set; }
       
    }
}