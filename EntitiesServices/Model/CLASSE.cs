//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntitiesServices.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CLASSE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CLASSE()
        {
            this.DOCUMENTO = new HashSet<DOCUMENTO>();
            this.METADADO_CLASSE = new HashSet<METADADO_CLASSE>();
        }
    
        public int CLAS_CD_ID { get; set; }
        public int ASSI_CD_ID { get; set; }
        public int SUBG_CD_ID { get; set; }
        public string CLAS_NM_NOME { get; set; }
        public string CLAS_SG_SIGLA { get; set; }
        public Nullable<int> CLAS_IN_NIVEL_SEGURANCA { get; set; }
        public string CLAS_DS_DESCRICAO { get; set; }
        public int CLAS_IN_ATIVO { get; set; }
        public string CLAS_TX_OBSERVACOES { get; set; }
    
        public virtual ASSINANTE ASSINANTE { get; set; }
        public virtual SUBGRUPO SUBGRUPO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTO> DOCUMENTO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<METADADO_CLASSE> METADADO_CLASSE { get; set; }
    }
}
