using System.ComponentModel.DataAnnotations;

namespace API.Models.UtilidadGeneral
{
    public class ItemMenuDB
    {
        [Key]
        public Int64 ID_ItemMenu { get; set; }
        public Int64 ID_ItemPadre { get; set; }
        public string Titulo { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public Int64 Prioridad { get; set; }
    }
    public class ItemMenu
    {
        public Int64 ID_ItemMenu { get; set; }
        public Int64 ID_ItemPadre { get; set; }
        public string label { get; set; }
        public string icon { get; set; }
        public string url { get; set; }
        public List<ItemMenu> children { get; set;}
        public Int64 Prioridad { get; set; }
        public bool Permiso { get; set; }
    }
    
}
