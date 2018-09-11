using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.BLL.Dto;
using VRP.DAL.Database.Models.Ticket;
using VRP.DAL.UnitOfWork;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class TicketController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TicketController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("account/{id}")]
        public IActionResult GetTicketsByAccountIdAsync(int id)
        {
            
            IEnumerable<TicketModel> tickets = _unitOfWork.TicketsRepository
                .JoinAndGetAll(ticket => ticket.InvolvedAccounts.Any(acc => acc.User.Id == id));

            if (!tickets.Any())
            {
                return NotFound(id);
            }

            IEnumerable<TicketDto> ticketDtos = _mapper.Map<TicketDto[]>(tickets);
            return Json(ticketDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetAsync(int id)
        {
            TicketModel ticket = _unitOfWork.TicketsRepository.JoinAndGet(id);
            if (ticket == null)
            {
                return NotFound(id);
            }

            TicketDto ticketDto = _mapper.Map<TicketDto>(ticket);   
            return Json(ticketDto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}