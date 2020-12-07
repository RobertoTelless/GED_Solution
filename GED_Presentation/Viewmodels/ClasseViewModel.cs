using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EntitiesServices.Model;

namespace Ged.ViewModels
{
    public class ClasseViewModel
    {
        [Key]
        public int CLAS_CD_ID { get; set; }
        public int ASSI_CD_ID { get; set; }
        public int SUBG_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo NOME obrigatorio")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "O NOME deve ter no minimo 1 e no máximo 150 caracteres.")]
        public string CLAS_NM_NOME { get; set; }
        [Required(ErrorMessage = "Campo SIGLA obrigatorio")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "A SIGLA deve ter no minimo 1 e no máximo 10 caracteres.")]
        public string CLAS_SG_SIGLA { get; set; }
        public Nullable<int> CLAS_IN_NIVEL_SEGURANCA { get; set; }
        [Required(ErrorMessage = "Campo DESCRIÇÃO obrigatorio")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "A DESCRIÇÃO deve ter no minimo 1 e no máximo 500 caracteres.")]
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