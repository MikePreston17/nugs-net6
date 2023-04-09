using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CodeMechanic.Extensions 
{

    public static class EmbedExtensions {


         public static async Task<string> GetQueryAsync<TPageMode>(
        this IEmbeddedResourceQuery embeddedResourceQuery
        , StackTrace trace
        , [CallerMemberName] string caller_method = ""

         ) {

            string resource =  trace.GetCurrentResourcePath(caller_method: caller_method);

            // trace.GetFrames().ToList()
            // .ForEach(x => Console.WriteLine(x.GetMethod()?.Name));
            caller_method.Dump("caller method");
            resource.Dump("resource path");
            
            // Reads from file system...
            await using Stream stream = embeddedResourceQuery.Read<TPageMode>(resource);
            
            if(stream == null)
            Console.WriteLine("""
            
            Psst!  The Query could not be found for this method. You can try the following to troubleshoot:
                
                1. All namespaces match that of your current project.
                2. ...
                3. ...

            """);

            // Reads the any file I tell it to as a query.
             string query = await stream.ReadAllLinesFromStreamAsync();

             return query;
        }

        /// Credit: https://gist.github.com/rflechner/fab685187f10b8eb9815c6af1f874d3d
        /// https://josef.codes/using-embedded-files-in-dotnet-core/
        public static string GetCurrentResourcePath(
            this StackTrace stackTrace
        , string extension = "cypher"
        , [CallerFilePath] string caller_path = ""
        , [CallerMemberName] string caller_method =""
        , bool exclude_crud = true)  
        {
            var frame = stackTrace.GetFrame(0); // this int controls the level by which we go up or down the stack.
            var method = frame.GetMethod();
            // var assembly = method.DeclaringType.Assembly;

            string full_namespace = method?.DeclaringType?.Namespace.Replace("_", " ");// For some reason, underscored directories are not red properly.  E.g. 'TPOT_Links' will not be understood.
            string path = $"{full_namespace}.{caller_method}.{extension}";

            if(exclude_crud)
                path = path
                    .Replace("OnGet", "")
                    .Replace("OnPatch", "")
                    .Replace("OnDelete", "")
                    .Replace("OnUpdate", "")
                    .Replace("OnPost", "");

            return path;
        }
    
        public static async Task<string> ReadAllLinesFromStreamAsync(this Stream stream)
        {
            if (stream == null)
            {
                return string.Empty;
            }
            using (StreamReader reader = new StreamReader(stream))
            {
                // Console.WriteLine("YA MADE IT!!");
                // System.Diagnostics.Debug.WriteLine("YA MADE IT!!");
                string contents = await reader.ReadToEndAsync();

                return contents;
            }

        }
    }
}