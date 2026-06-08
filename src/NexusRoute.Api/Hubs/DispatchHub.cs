using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NexusRoute.Application.DTOs;
using NexusRoute.Application.Interfaces;

namespace NexusRoute.Api.Hubs;

[Authorize]
public class DispatchHub : Hub<IDispatchClient>
{
    private readonly ILogger<DispatchHub> _logger;

    public DispatchHub(ILogger<DispatchHub> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, "Dispatchers");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Dispatchers");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        _logger.LogInformation("Client {ConnectionId} joined group {GroupName}", Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        _logger.LogInformation("Client {ConnectionId} left group {GroupName}", Context.ConnectionId, groupName);
    }
}

public interface IDispatchClient
{
    Task ReceiveTelemetryUpdate(TelemetryResponseDto telemetry);
    Task ReceiveAssetStatusUpdate(AssetStatusDto assetStatus);
    Task ReceiveAlert(AlertDto alert);
    Task ReceiveConvoyUpdate(ConvoyDto convoy);
    Task ReceiveProductionUpdate(ProductionSummaryDto production);
}
