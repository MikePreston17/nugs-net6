/* 
This class only exists to reduce boilerplate 

and because I can'think of a better name

 So, It's Hi-Speed.
 Inheritance sucks.

 Get used to it.
*/


using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CodeMechanic.Extensions;
using Neo4j.Driver;

namespace CodeMechanic.RazorPages;

public abstract class HighSpeedPageModel : PageModel
{
    protected readonly IEmbeddedResourceQuery embeddedResourceQuery;
    protected readonly IDriver driver;

    public HighSpeedPageModel(
    IEmbeddedResourceQuery embeddedResourceQuery
    , IDriver driver
    )
    {
        this.embeddedResourceQuery = embeddedResourceQuery;
        this.driver = driver;
    }

    // public object NeoWrite(string query, IDictionary<string, object> neo4j_params) 
    // {
    //     await using var session = _driver.AsyncSession(configBuilder => configBuilder.WithDatabase("neo4j"));

    //     try
    //     {
    //         // Write transactions allow the driver to handle retries and transient error
    //         var writeResults = await session.ExecuteWriteAsync(async tx =>
    //         {
    //             var result = await tx.RunAsync(query, 
    //             new { 
    //                 person1Name, person2Name 
    //             });
    //             return await result.ToListAsync();
    //         });

    //         // foreach (var result in writeResults)
    //         // {
    //         //     var person1 = result["p1"].As<INode>().Properties["name"];
    //         //     var person2 = result["p2"].As<INode>().Properties["name"];
    //         //     Console.WriteLine($"Created friendship between: {person1}, {person2}");
    //         // }
    //     }

    //     // Capture any errors along with the query and data for traceability
    //     catch (Neo4jException ex)
    //     {
    //         Console.WriteLine($"{query} - {ex}");
    //         throw;
    //     }
    // }

}