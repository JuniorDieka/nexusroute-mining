import { SignalRClient } from './signalr-client.js';
import { DashboardManager } from './dashboard.js';
import { AlertsManager } from './alerts.js';

class App {
    constructor() {
        this.signalR = new SignalRClient();
        this.dashboard = new DashboardManager();
        this.alerts = new AlertsManager();
        this.init();
    }

    async init() {
        this.setupNavigation();
        await this.loadInitialData();
        await this.signalR.connect();
        this.setupSignalRHandlers();
    }

    setupNavigation() {
        const navButtons = document.querySelectorAll('.nav-btn');
        navButtons.forEach(btn => {
            btn.addEventListener('click', () => {
                const viewName = btn.dataset.view;
                this.switchView(viewName);
                
                navButtons.forEach(b => b.classList.remove('active'));
                btn.classList.add('active');
            });
        });
    }

    switchView(viewName) {
        const views = document.querySelectorAll('.view');
        views.forEach(view => view.classList.remove('active'));
        
        const targetView = document.getElementById(`${viewName}View`);
        if (targetView) {
            targetView.classList.add('active');
        }
    }

    async loadInitialData() {
        try {
            await Promise.all([
                this.dashboard.loadAssets(),
                this.dashboard.loadConvoys(),
                this.dashboard.loadProduction(),
                this.alerts.loadAlerts()
            ]);
        } catch (error) {
            console.error('Error loading initial data:', error);
        }
    }

    setupSignalRHandlers() {
        this.signalR.onTelemetryUpdate((telemetry) => {
            this.dashboard.updateTelemetry(telemetry);
        });

        this.signalR.onAssetStatusUpdate((asset) => {
            this.dashboard.updateAssetStatus(asset);
        });

        this.signalR.onAlert((alert) => {
            this.alerts.addAlert(alert);
        });

        this.signalR.onConvoyUpdate((convoy) => {
            this.dashboard.updateConvoy(convoy);
        });

        this.signalR.onProductionUpdate((production) => {
            this.dashboard.updateProduction(production);
        });

        this.signalR.onConnectionChange((connected) => {
            this.updateConnectionStatus(connected);
        });
    }

    updateConnectionStatus(connected) {
        const statusDot = document.querySelector('.status-dot');
        const statusText = document.getElementById('statusText');
        
        if (connected) {
            statusDot.classList.remove('disconnected');
            statusDot.classList.add('connected');
            statusText.textContent = 'Connected';
        } else {
            statusDot.classList.remove('connected');
            statusDot.classList.add('disconnected');
            statusText.textContent = 'Disconnected';
        }
    }
}

document.addEventListener('DOMContentLoaded', () => {
    new App();
});
