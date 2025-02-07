using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DesafioBackend.Model.Enums;

namespace DesafioBackend.Model
{
    public class HistoricoPesquisa
    {
        public int Id { get; set; }
        public TipoDeBusca Tipo { get; set; }
        public DateTime DataConsulta { get; set; }
        public string TermoPesquisa { get; set; }
        public string ResultadoJson { get; set; }

        public HistoricoPesquisa() { }

        public HistoricoPesquisa(TipoDeBusca tipo, string termoPesquisa, object resultado)
        {
            Tipo = tipo;
            TermoPesquisa = termoPesquisa;
            DataConsulta = DateTime.Now;
            ResultadoJson = JsonSerializer.Serialize(resultado, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
