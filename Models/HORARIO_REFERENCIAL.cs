﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HorariosIPBejaMVC.Models;

[Table("HORARIO_REFERENCIAL")]
public partial class HORARIO_REFERENCIAL 
{
    [Key]
    public int id { get; set; }

    public int uc_id { get; set; }

    public int turma_id { get; set; }

    public int docente_id { get; set; }

    public int sala_id { get; set; }

    public int periodo_horario_id { get; set; }

    public int ano_letivo_id { get; set; }

    public int? solucaoHorarioId { get; set; }

    [ForeignKey("solucaoHorarioId")]
    public virtual SolucaoHorario SolucaoHorario { get; set; }

    [ForeignKey("ano_letivo_id")]
    [InverseProperty("HORARIO_REFERENCIALs")]
    public virtual ANO_LETIVO ano_letivo { get; set; } = null!;

    [ForeignKey("docente_id")]
    [InverseProperty("HORARIO_REFERENCIALs")]
    public virtual DOCENTE docente { get; set; } = null!;

    [ForeignKey("periodo_horario_id")]
    [InverseProperty("HORARIO_REFERENCIALs")]
    public virtual PERIODO_HORARIO periodo_horario { get; set; } = null!;

    [ForeignKey("sala_id")]
    [InverseProperty("HORARIO_REFERENCIALs")]
    public virtual SALA sala { get; set; } = null!;

    [ForeignKey("turma_id")]
    [InverseProperty("HORARIO_REFERENCIALs")]
    public virtual TURMA turma { get; set; } = null!;

    [ForeignKey("uc_id")]
    [InverseProperty("HORARIO_REFERENCIALs")]
    public virtual UNIDADE_CURRICULAR uc { get; set; } = null!;

    // Implementação da propriedade DescricaoPeriodo
    public string DescricaoPeriodo => periodo_horario.descricao;
}
