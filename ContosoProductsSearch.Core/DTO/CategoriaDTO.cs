﻿namespace ContosoProductsSearch.Core.DTO;

public class CategoriaDTO
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public IEnumerable<ProdottoDTO>? Prodotti { get; set; }
}
