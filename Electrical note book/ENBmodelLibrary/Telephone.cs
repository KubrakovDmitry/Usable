//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ENBmodelLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class Telephone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string telephone { get; set; }
        public int ContactId { get; set; }
    
        public virtual Contact Contact { get; set; }
    }
}
