export class SignalRClient {
    constructor() {
        this.connection = null;
        this.handlers = {
            telemetry: [],
            assetStatus: [],
            alert: [],
            convoy: [],
            production: [],
            connectionChange: []
        };
    }

    async connect() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl('/hubs/dispatch', {
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets
            })
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();

        this.connection.on('ReceiveTelemetryUpdate', (telemetry) => {
            this.handlers.telemetry.forEach(handler => handler(telemetry));
        });

        this.connection.on('ReceiveAssetStatusUpdate', (asset) => {
            this.handlers.assetStatus.forEach(handler => handler(asset));
        });

        this.connection.on('ReceiveAlert', (alert) => {
            this.handlers.alert.forEach(handler => handler(alert));
        });

        this.connection.on('ReceiveConvoyUpdate', (convoy) => {
            this.handlers.convoy.forEach(handler => handler(convoy));
        });

        this.connection.on('ReceiveProductionUpdate', (production) => {
            this.handlers.production.forEach(handler => handler(production));
        });

        this.connection.onreconnecting(() => {
            this.handlers.connectionChange.forEach(handler => handler(false));
        });

        this.connection.onreconnected(() => {
            this.handlers.connectionChange.forEach(handler => handler(true));
        });

        this.connection.onclose(() => {
            this.handlers.connectionChange.forEach(handler => handler(false));
        });

        try {
            await this.connection.start();
            console.log('SignalR Connected');
            this.handlers.connectionChange.forEach(handler => handler(true));
            
            await this.connection.invoke('JoinGroup', 'Dispatchers');
        } catch (err) {
            console.error('SignalR Connection Error:', err);
            this.handlers.connectionChange.forEach(handler => handler(false));
            setTimeout(() => this.connect(), 5000);
        }
    }

    onTelemetryUpdate(handler) {
        this.handlers.telemetry.push(handler);
    }

    onAssetStatusUpdate(handler) {
        this.handlers.assetStatus.push(handler);
    }

    onAlert(handler) {
        this.handlers.alert.push(handler);
    }

    onConvoyUpdate(handler) {
        this.handlers.convoy.push(handler);
    }

    onProductionUpdate(handler) {
        this.handlers.production.push(handler);
    }

    onConnectionChange(handler) {
        this.handlers.connectionChange.push(handler);
    }
}
