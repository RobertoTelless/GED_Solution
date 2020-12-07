using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EntitiesServices.Model;

namespace Ged.ViewModels
{
    public class MetadadoClasseViewModel
    {
        [Key]
        public int MECL_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo CLASSE obrigatorio")]
        public int CLAS_CD_ID { get; set; }
        [Required(ErrorMessage = "Campo NOME obrigatorio")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "O NOME deve ter no minimo 1 e no máximo 100 caracteres.")]
        public string MECL_NM_NOME { get; set; }
        [Required(ErrorMessage = "Campo SIGLA obrigatorio")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "A SIGLA deve ter no minimo 1 e no máximo 10 caracteres.")]
        public string MECL_SG_SIGLA { get; set; }
        [Required(ErrorMessage = "Campo DESCRIÇÃO obrigatorio")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "A DESCRIÇÃO deve ter no minimo 1 e no máximo 500 caracteres.")]
        public string MECL_DS_DESCRICAO { get; set; }
        public Nullable<int> MECL_IN_MULTIVALORADO { get; set; }
        [Required(ErrorMessage = "Campo TIPO obrigatorio")]
        public Nullable<int> TIME_CD_ID { get; set; }
        public Nullable<int> MECL_NR_TAMANHO { get; set; }
        public Nullable<int> MECL_IN_OBRIGATORIO { get; set; }
        public Nullable<int> MECL_IN_INDEXADO { get; set; }
        public int MECK_IN_ATIVO { get; set; }

        public virtual CLASSE CLASSE { get; set; }
        public virtual TIPO_METADADO TIPO_METADADO { get; set; }

    }
}