using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.DAL.Database.Models.Ticket;
using VRP.vAPI.Dto;
using VRP.vAPI.UnitOfWork;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
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
        public IActionResult GetTicketsByAccountId(int id)
        {
            List<TicketModel> tickets = new List<TicketModel>();
            IEnumerable<TicketMessageModel> ticketsMessages =
                _unitOfWork.AccountsRepository.JoinAndGet(id).TicketsMessages;
            foreach (var ticketMessage in ticketsMessages)
            {
                tickets.Add(ticketMessage.Ticket);
            }
            if (!tickets.Any())
            {
                return NotFound(id);
            }


           // IEnumerable<TicketDto> ticketDtos = _mapper.Map<TicketDto>(tickets);
            return Json(tickets);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            TicketModel ticket = _unitOfWork.TicketsRepository.JoinAndGet(id);
            if (ticket == null)
            {
                return NotFound(id);
            }

            return Json(ticket);
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