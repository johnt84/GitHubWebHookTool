using GitHubWebHookEngine.API;
using GitHubWebHookEngine.Services.Interfaces;
using GitHubWebHookEngine.Services;
using GitHubWebHookShared.Models;

namespace GitHubWebHookAWSServerlessTool;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        //Environment Variables read from Properties/launchSettings.json

        var gitHubWebHookToolInput = new GitHubWebHookToolInput()
        {
            GitHubAPIUrl = Configuration["GitHubAPIUrl"],
            PrivateToken = Configuration["PrivateToken"],
        };

        services.AddSingleton(gitHubWebHookToolInput);

        services.AddHttpClient<HttpAPIClient>();

        services.AddScoped<IPushService, PushService>();
        services.AddScoped<ICommitService, CommitService>();
        services.AddScoped<ITopicService, TopicService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
            });
        });
    }
}