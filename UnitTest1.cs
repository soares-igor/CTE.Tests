using CTE.Controllers;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CTE.Tests
{
    public class CteControllerTests
    {
        [Fact]
        public void CalcularCte_RetornaValoresCorretos()
        {
            // Arrange
            var controller = new CteController();
            var request = new CteRequest
            {
                TarifaPorPeso = 100.00m, // Tarifa por tonelada
                PesoCarga = 2.0m,        // Peso da carga em toneladas
                DespesasAdicionais = 80.00m, // Pedágios e outras despesas
                AliquotaIcms = 12.0m,     // Alíquota do ICMS
                Quantidade = 10,         // Quantidade de itens
                Volume = 5,              // Volume total
                Origem = "Sao Paulo",    // Origem
                Destino = "Minas Gerais", // Destino
                DistanciaKm = 500,       // Distância
                InicioOperacao = DateTime.Parse("2024-12-25T23:46:52.058Z") // Data e hora de operação
            };

            // Act
            var result = controller.CalcularCte(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);  // Verificar se não é null
            var response = result?.Value as CteResponse;

            Assert.NotNull(response);
            Assert.Equal(280.00m, response.ValorFrete); // 2 toneladas x 100 + 80 despesas
            Assert.Equal(38.18m, Math.Round(response.ValorIcms, 2)); // ICMS calculado
            Assert.Equal(318.18m, Math.Round(response.ValorTotalCte, 2)); // Frete + ICMS

            // Verificar se os campos adicionais estão corretos
            Assert.Equal(10, response.Informacoes.Quantidade);
            Assert.Equal(5, response.Informacoes.Volume);
            Assert.Equal("Sao Paulo", response.Informacoes.Origem);
            Assert.Equal("Minas Gerais", response.Informacoes.Destino);
            Assert.Equal(500, response.Informacoes.DistanciaKm);
            Assert.Equal(DateTime.Parse("2024-12-25T23:46:52.058Z"), response.Informacoes.InicioOperacao);
        }
    }
}