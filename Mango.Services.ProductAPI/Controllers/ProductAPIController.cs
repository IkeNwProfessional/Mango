using Mango.Services.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly ResponseDto _response;

        public ProductAPIController(IProductService productService)
        {
            this.productService = productService;
            _response = new ResponseDto();
        }
        [HttpPost("CreateProduct")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> CreateProduct([FromBody] ProductDto product)
        {
            try
            {
                var result = await productService.CreateProduct(product);
                if (result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Error creating product";
                    return _response;
                }

                _response.IsSuccess = true;
                _response.Result = result;
            }
            catch (Exception ex)
            {
               _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpGet, Route("GetProduct/{id:int}")]
        public async Task<ResponseDto> GetProduct([FromRoute] int id)
        {
            try
            {
                var result = await productService.GetProductById(id);
                if (result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Product not found";
                    return _response;
                }

                _response.IsSuccess = true;
                _response.Result = result;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPut, Route("UpdateProduct/{id:int}")]
        [Authorize(Roles ="ADMIN")]
        public async Task<ResponseDto> UpdateProduct([FromRoute] int id, [FromBody] ProductDto product)
        {
            try
            {
                var result = await productService.UpdateProduct(id, product);
                if (result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Error updating product";
                    return _response;
                }

                _response.IsSuccess = true;
                _response.Result = result;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpDelete, Route("DeleteProduct/{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> DeleteProduct([FromRoute] int id)
        {
            try
            {
                var result = await productService.DeleteProduct(id);
                if (!result)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Product not found";
                    return _response;
                }

                _response.IsSuccess = true;
                _response.Message = "Product deleted successfully";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpGet, Route("GetAllProducts")]
        public async Task<ResponseDto> GetAllProducts()
        {
            try
            {
                var result = await productService.GetAllProducts();
                if (result == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "No products found";
                    return _response;
                }

                _response.IsSuccess = true;
                _response.Result = result;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
    }
}
