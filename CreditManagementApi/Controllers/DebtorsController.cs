using AutoMapper;
using CreditManagementApi.Dtos;
using CreditManagementApi.Entities;
using CreditManagementApi.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CreditManagementApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DebtorsController : ControllerBase
    {
        private readonly IDebtorService _service;
        private readonly IMapper _mapper;

        public DebtorsController(IDebtorService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(int page = 1,int pageSize = 10,string? fincode = null,string? firstName = null)
        {
            var result = await _service
                .GetAll(
                page,
                pageSize,
                fincode,
                firstName);

            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<ActionResult> GetDebtorAsync(Guid id)
        {
            var debtor = await _service.GetDebtorAsync(id);
            var result = _mapper.Map<DebtorDto>(debtor);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddDebtorAsync([FromBody] CreateDebtorDto dto)
        {
            var debtor = _mapper.Map<Debtor>(dto);
            var result = await _service.AddAsync(debtor);
            var response = _mapper.Map<DebtorDto>(result);

            return Ok(response);
        }

        [HttpPut("id:{guid}")]
        public async Task<ActionResult> UpdateAsync([FromBody] UpdateDebtorDto dto)
        {
            var debtor = _mapper.Map<Debtor>(dto);
            var result = await _service.UpdateAsync(debtor);
            var response = _mapper.Map<DebtorDto>(result);
            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeletedAsync(Guid id)
        {
            await _service.DeleteDebtorAsync(id);
            return NoContent();
        }
    }
}
