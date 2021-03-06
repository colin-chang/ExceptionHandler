﻿using System;
using System.Linq;
using System.Threading.Tasks;
using ColinChang.ExceptionHandler;
using ColinChang.ExceptionHandler.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ColinChang.ExceptionHandler.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public Task<IOperationResult> GetAsync()
        {
            if (Request.Query.Any())
                throw new OperationException("test expected exception");

            throw new InvalidOperationException("test unexpected exception");
        }

        [HttpGet("{code}")]
        [OperationExceptionFilter]
        public Task<IOperationResult> GetAsync(int code)
        {
            if (code < 0)
                throw new OperationException("expected exception");

            if (code > 0)
                return Task.FromResult<IOperationResult>(
                    new OperationResult<string>("test exception filter attribute"));

            throw new Exception("unexpected exception");
        }

        [HttpPost]
        [OperationExceptionFilter]
        public Task PostAsync([FromBody] Person person)
        {
            throw new OperationException("test body parameter exception");
        }

        [HttpPut]
        [OperationExceptionFilter]
        public Task PutAsync([FromForm] Student student)
        {
            throw new OperationException("test body parameter exception");
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class Student
    {
        public string No { get; set; }
        public IFormFile Photo { get; set; }
    }
}