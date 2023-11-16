using System.Text.RegularExpressions;
using CodeMechanic.Diagnostics;
using CodeMechanic.FileSystem;
using NSpecifications;
using nugsnet6.Experimental;

namespace CodeMechanic.RazorHAT.Services;

public interface IRazorRoutesService
{
    string[] GetAllRoutes();
    string[] GetBreadcrumbsForPage(string builder);
}

public class RazorRoutesService : IRazorRoutesService
{
    private readonly bool dev_mode;
    private readonly IEnumerable<string> razor_page_routes;

    public RazorRoutesService(bool dev_mode = false)
    {
        razor_page_routes = GetAllRoutes();
        this.dev_mode = dev_mode;
    }

    public string[] GetAllRoutes()
    {
        string current_directory = Environment.CurrentDirectory;

        if (dev_mode) Console.WriteLine("cwd :>> " + current_directory);

        var grepper = new Grepper()
        {
            RootPath = current_directory,
            FileSearchMask = "**.cs",
            Recursive = true,
        };

        var is_blacklisted = new Spec<string>(
            filepath => filepath.Contains("node_modules/")
                        || filepath.Contains("wwwroot/")
                        || filepath.Contains("bin/")
                        || filepath.Contains("obj/")
        );

        RegexOptions options = RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace |
                               RegexOptions.IgnoreCase;

        var regex = new Regex(@"(?<subdirectory>\/?(\w+\/)*)(?<file_name>.*\.(?<extension>cshtml|cs))",
            options);

        var routes = grepper.GetFileNames()
                .Where(!is_blacklisted)
                .Select(p => p.Replace(current_directory, ""))
                .Select(p => p.Extract<RazorRoute>(regex)?.FirstOrDefault()?.subdirectory)
                .Distinct()
            // .Dump("routes")
            ;

        return routes.ToArray();
    }

    public string[] GetBreadcrumbsForPage(string builder)
    {
        var current_breadcrumbs = this.GetAllRoutes()
            .Where(path => path.Contains("Builder"))
            .Dump("Current breadcrumbs");

        return current_breadcrumbs.ToArray();
    }
}

public class RazorRoute
{
    public string subdirectory { get; set; }
    public string file_name { get; set; }
    public string extension { get; set; }
}