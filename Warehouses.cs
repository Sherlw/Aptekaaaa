//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Aptekaaaa
{
    using System;
    using System.Collections.Generic;
    
    public partial class Warehouses
    {
        public Warehouses()
        {
            this.Orderss = new HashSet<Orderss>();
            this.ProductsInWarehouses = new HashSet<ProductsInWarehouses>();
        }
    
        public int WarehouseID { get; set; }
        public string Address { get; set; }
        public string ContactInformation { get; set; }
    
        public virtual ICollection<Orderss> Orderss { get; set; }
        public virtual ICollection<ProductsInWarehouses> ProductsInWarehouses { get; set; }
    }
}
