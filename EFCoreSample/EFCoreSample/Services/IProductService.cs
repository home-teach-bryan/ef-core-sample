﻿using EFCoreSample.DbContext;
using EFCoreSample.Models;
using EFCoreSample.Models.Request;
using EFCoreSample.Models.Response;

namespace EFCoreSample.Services;

public interface IProductService
{
    public bool AddProduct(AddProductRequest product);

    public bool UpdateProduct(Guid id, UpdateProductRequest product);

    public bool RemoveProduct(Guid id);
    
    public List<GetProductResponse> GetAllProducts();

    public GetProductResponse GetProduct(Guid id);
}