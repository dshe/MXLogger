﻿using Microsoft.Extensions.Logging;
using System;
using Xunit.Abstractions;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using MXLogger;

namespace MXLoggerTest
{ 
    public class InjectionTest
    {
        private readonly Action<string> WriteLine;
        public InjectionTest(ITestOutputHelper output) => WriteLine = output.WriteLine;

        [Fact]
        public void Test()
        {
            var provider = new MXLoggerProvider(WriteLine);
            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddProvider(provider));
            services.AddSingleton<MyComponent>();
            var serviceProvider = services.BuildServiceProvider();
            MyComponent myComponent = serviceProvider.GetRequiredService<MyComponent>();

            myComponent.Log("test");
            Assert.Equal("Info\t  MXLoggerTest.MyComponent\t  test\t  ", provider.Format(provider.LogEntries.Last()));
        }
    }

    public class MyComponent
    {
        private readonly ILogger Logger;
        public MyComponent(ILogger<MyComponent> logger) => Logger = logger;
        public void Log(string s) => Logger.LogInformation(s);
     }

}