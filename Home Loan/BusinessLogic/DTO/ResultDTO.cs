using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;

namespace BusinessLogic.DTO
{
    public class ResultDTO
    {
        public ResultDTO(bool success, int statusCode,string message)
        {
            this.Success = success;
            this.StatusCode = statusCode;
            this.Message = message;
        }
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
       
    }
    public class ResultDTO<T> : ResultDTO
    {
        public ResultDTO(bool success, int statusCode, string message,T data)
            :base(success,statusCode,message)
        {
            this.Data = data;
        }
        public T Data { get; set; }
    }
}
