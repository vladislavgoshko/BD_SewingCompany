﻿using SewingCompany.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SewingCompany;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SewingCompany
{
    public class DbUserInitMiddleware
    {
        private readonly RequestDelegate _next;
        public DbUserInitMiddleware(RequestDelegate next)
        {
            // инициализация базы данных 
            _next = next;
        }
        public Task Invoke(HttpContext context)
        {
            //if (!(context.Session.Keys.Contains("starting")))
            //{
                DbUserInitializer.Initialize(context).Wait();
            //    context.Session.SetString("starting", "Yes");
            //}

            // Вызов следующего делегата / компонента middleware в конвейере
            return _next.Invoke(context);
        }
    }

}