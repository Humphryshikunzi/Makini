using _.Application.Features.SpeedGovenor.Queries.GetOneSpeedGovernor;
using _.Application.Interfaces.Repositories;
using _.Application.Interfaces.Serialization.Serializers;
using _.Domain.Entities.Catalog;
using _.Infrastructure.Contexts;
using _.Server.Hubs;
using _.Shared.Constants.Application;
using _.Shared.Wrapper;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.SignalR; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Location = _.Domain.Entities.Catalog.Location;

namespace _.Server.Background
{
  public class UdpServerBackground : BackgroundService
  {

    private readonly char[] chartoTrim = { 'b', '\'' };
    private UdpClient listner;
    private IPEndPoint groupEp;
    private readonly IConfiguration _config;
    private readonly AppDbContext context;
    private readonly ILogger<UdpServerBackground> logger;
    private readonly IJsonSerializer serializer;
    private readonly IUnitOfWork<int> unitofWork;
    protected IMediator _mediator;
    private readonly IHubContext<SignalRHub> _hubConnnection;

    public UdpServerBackground(ILogger<UdpServerBackground> logger, IServiceScopeFactory factory, IHubContext<SignalRHub> hub)
    {
      this.context = factory.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
      this.logger = logger;
      this.serializer = factory.CreateScope().ServiceProvider.GetRequiredService<IJsonSerializer>();
      this._config = factory.CreateScope().ServiceProvider.GetRequiredService<IConfiguration>();
      this.unitofWork = factory.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork<int>>();
      _mediator = factory.CreateScope().ServiceProvider.GetService<IMediator>();
      _hubConnnection = hub;
    }

    public async Task<Result<SpeedGovernor>> GetSpeedGovernor(string searchString)
    {
      var result = await _mediator.Send(new GetSpeedGovenorQuery(searchString));
      return result;
    }

    public async Task<SpeedGovernor> GetSpeedGov(string searchString)
    {
      var result = await GetSpeedGovernor(searchString);
      return await Task.FromResult<SpeedGovernor>(result.Data);
    }

    public async Task SaveLocation(Location location, SpeedGovernor speedgov)
    {
      location.SpeedGovId = speedgov.Id;
      context.Locations.Add(location);
      await context.SaveChangesAsync();
    }

    public async Task SendLocation(Location location , string speedGovId)
{
      await _hubConnnection.Clients.All.SendAsync(ApplicationConstants.SignalR.ReceiveNewLocation, location, speedGovId);
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      var port = int.Parse(_config["UdpPort"]);
      groupEp = new IPEndPoint(IPAddress.Any, port);
      listner = new UdpClient(groupEp);
      logger.LogInformation($"Waiting for BloadCasts on port {port}");
      try
      {
        while (true)
      {
        var bytes = await listner.ReceiveAsync();
        var data = Encoding.ASCII.GetString(bytes.Buffer, 0, bytes.Buffer.Length);
        var result = data.TrimStart(chartoTrim).Trim().Split(",");
        logger.LogInformation(serializer.Serialize(result));

        string searchstring = result[1];

        //find speed governor
        try
        {
        var location = new Location()
        {
          Time = result[8],
          Date = result[7],
          GpsCourse = result[3],
          Speed = result[11].Remove(result[11].Length - 1),
          EngineON = result[5],
          Latitude = Double.Parse(result[6]),
          Long = Double.Parse(result[9])
        };
        
        var speedgov = (await GetSpeedGovernor(searchstring)).Data;
        await SendLocation(location, speedgov.Id.ToString());
        
       BackgroundJob.Enqueue(() => SaveLocation(location, speedgov));
        }
        catch (System.Exception ex)
        {
           logger.LogInformation(ex.Message);
        }
      }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
    }   
  }
}
