using System;

namespace WebCinema.Models
{
    public class InventoryHistory
    {
        public int InventoryHistoryId { get; set; }
        public int ComboId { get; set; }
        public Combo Combo { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime Date { get; set; }
    }
}
