﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoxezSpZOOContext.Models
{
    public partial class Dokument
    {
        [Key]
        public int Id { get; set; }
        public int Identyfikator { get; set; }
        [Required]
        public string Typ { get; set; }
        [Required]
        public string Tytul { get; set; }
        [Required]
        public string NazwaPliku { get; set; }
        [Required]
        public byte[] Tresc { get; set; }
        [Column(TypeName = "date")]
        public DateTime DataUtworzenia { get; set; }
    }
}