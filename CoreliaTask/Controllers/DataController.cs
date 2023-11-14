using AutoMapper;
using BL;
using BL.DTOs;
using BL.IRepository;
using CoreliaTask.Data;
using CoreliaTask.Model;
using CoreliaTask.SignalR;
using DAL;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CoteliaTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotifyHub> _hubContext;
        public DataController(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<NotifyHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
        }
        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.Employees.GetById(id));
        }
        [HttpGet("GetAll")] 
        public IActionResult GetAll()
        {
            /*RecurringJob.AddOrUpdate(() => SendMessage("Recurring Job Triggered"), Cron.Minutely);*/

            /*BackgroundJob.Enqueue(() => SendMessage("Sent now"));*/
            BackgroundJob.Schedule(() => SendMessage("Email@"), TimeSpan.FromMinutes(1));
            return Ok(_unitOfWork.Employees.GetAll());
        }
        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            return Ok(_unitOfWork.Employees.Find(p=>p.Name==name));
        }
        [HttpPost("Add")]
        public Employee Add(EmployeeDTO employeeDTO)
        {
            var map = _mapper.Map<Employee>(employeeDTO);
            _unitOfWork.Employees.Add(map);
            _unitOfWork.Save();

            _hubContext.Clients.All.SendAsync("ReceiveNotification", "Employee added.");
            return map;
        }
        [HttpDelete("Delete")]
        public IActionResult Delete(Employee emp)
        {
            _unitOfWork.Employees.Delete(emp);
            _unitOfWork.Save();
            _hubContext.Clients.All.SendAsync("ReceiveNotification", "Employee Deleted.");
            return Ok();
        }
        [HttpPost("Update")]
        public Employee Update(EmployeeDTO employeeDTO)
        {
            var emp = _unitOfWork.Employees.Find(p => p.ID == employeeDTO.Id);
            _mapper.Map(employeeDTO, emp);
            _unitOfWork.Employees.Update(emp);
            _unitOfWork.Save();
            _hubContext.Clients.All.SendAsync("ReceiveNotification", "Employee Updated.");

            return emp;
        }
        /*[ApiExplorerSettings(IgnoreApi = true)]
        public void SendMessage(string message)
        {
            Console.WriteLine($"Message Sent {DateTime.Now}");
        }*/
    }
}
