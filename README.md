# SimpleAuditor

[![Build Status](https://orbsync.visualstudio.com/DigitalPockets/_apis/build/status/SimpleAuditor?branchName=master)](https://orbsync.visualstudio.com/DigitalPockets/_build/latest?definitionId=11&branchName=master)[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE) [![NuGet Badge](https://buildstats.info/nuget/SimpleAuditor)](https://www.nuget.org/packages/SimpleAuditor)

Simple EntityFrameWorkCore Audit Log 

## About SimpleAuditor

SimpleAuditor is a C Sharp library that enables you log changes made on your tables by simply putting an attributes on the tables. The library makes easy to log changes and also allows for override of who made the change.

## How to use

To use `SimpleAuditor` , add it by searching on Nuget manager or use the install command below

```
Install-Package SimpleAuditor
```
Run the SQL script to create the AuditTrail and AuditTrail tables in your Database

https://github.com/msdkool/auditor/tree/master/DbScript

Your DbContext should inherit from the `AuditContext`

```
public class ExampleDbContext : AuditContext
{
        [Audit]
        public DbSet<Person> Person { get; set; }

        public ExampleDbContext(DbContextOptions<ExampleDbContext> options) : base(options)
        {

        }

        public override string GetUserName()
        {
            return "TestUser";
        }
}
```
In the example above the Person entity has been marked for Audit, SimpleAuditor will automatically detect this when a change is made and save whatever change is effected.

## GetUserName method

By default the library will attempt to access the `IHttpContextAccessor` to fetch the identity of the person carrying out the action. The library allows you override that method is order to provide a custom GetUserName.



