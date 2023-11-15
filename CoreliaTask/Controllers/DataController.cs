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
        private readonly IHubContext<NotifyHub,INotifyHub> _hubContext;
        public DataController(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<NotifyHub, INotifyHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
        }
        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var emp = _unitOfWork.Employees.GetById(id);
            try
            {             
                if (emp != null)
                    return Ok(emp);
                return NotFound();   
            }
            catch
            {
                return StatusCode(500,"Internal Error");
            }  
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var emps = _unitOfWork.Employees.GetAll();
                if (emps != null)
                    return Ok(emps);

                return NotFound();
            }
            catch
            {
                return StatusCode(500, "Internal Error");
            }
        }
        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            try
            {
                var empByName = _unitOfWork.Employees.Find(p => p.Name == name);
                if (empByName != null)
                    return Ok(empByName);
                return NotFound();
            }
            catch
            {
                return StatusCode(500, "Internal Error");
            }
        }
        [HttpPost("Add")]
        public IActionResult Add(EmployeeDTO employeeDTO)
        {
            try
            {
                var map = _mapper.Map<Employee>(employeeDTO);
                if (map != null)
                {
                    _unitOfWork.Employees.Add(map);
                    _unitOfWork.Save();

                    _hubContext.Clients.All.sendnotification("Employee Added.");
                    return Ok(map);
                }
                else
                {
                    return BadRequest();

                }

            }
            catch
            {
                return StatusCode(500, "Internal Error");

            }

        }
        [HttpDelete("Delete")]
        public IActionResult Delete(Employee employee)
        {
            try
            {
                if (employee != null)
                {
                    var emp = _unitOfWork.Employees.GetById(employee.ID);
                    if (emp != null)
                    {
                        _unitOfWork.Employees.Delete(emp);
                        _unitOfWork.Save();
                        _hubContext.Clients.All.sendnotification("Employee Deleted.");
                        return Ok(emp);
                    }
                    return NotFound();
                }
                return BadRequest();
            }
            catch
            {
                return StatusCode(500, "Internal Error");

            }

        }
        [HttpPut("Update")]
        public IActionResult Update(EmployeeDTO employeeDTO)
        {
            try
            {
                if (employeeDTO != null)
                {
                    var emp = _unitOfWork.Employees.Find(p => p.ID == employeeDTO.Id);
                    if (emp != null)
                    {
                        _mapper.Map(employeeDTO, emp);
                        _unitOfWork.Employees.Update(emp);
                        _unitOfWork.Save();
                        _hubContext.Clients.All.sendnotification("Employee Updated.");

                        return Ok(emp);
                    }
                    else
                    {
                        return NotFound();
                    }
                    
                }
                return BadRequest();
                
            }
            catch
            {
                return StatusCode(500, "Internal Error");

            }

        }

        //Scheduling ExecuteGetAllJob To Run every One Minte
        [HttpPost("ScheduleGetAllJob")]
        public IActionResult ScheduleGetAllJob()
        {
            // Schedule a recurring job to call GetAll every minute
            RecurringJob.AddOrUpdate(() => ExecuteGetAllJob(), Cron.Minutely);

            return Ok("Recurring job scheduled successfully");
        }
        //Enqueue GetAll to the Jobs
        [ApiExplorerSettings(IgnoreApi = true)]
        public void ExecuteGetAllJob()
        {
            BackgroundJob.Enqueue(() => GetAll());
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void SendMessage(string message)
        {
            Console.WriteLine($"Message Sent {DateTime.Now}");
        }
    }
}
