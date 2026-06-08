export class AlertsManager {
    constructor() {
        this.alerts = new Map();
    }

    async loadAlerts() {
        try {
            const response = await fetch('/api/alerts/active');
            const alerts = await response.json();
            
            alerts.forEach(alert => this.alerts.set(alert.id, alert));
            this.renderAlerts();
            this.updateAlertCounts();
        } catch (error) {
            console.error('Error loading alerts:', error);
        }
    }

    addAlert(alert) {
        this.alerts.set(alert.id, alert);
        this.renderAlerts();
        this.updateAlertCounts();
        
        if (alert.severity === 'Critical') {
            this.showNotification(alert);
        }
    }

    renderAlerts() {
        const container = document.getElementById('alertsList');
        
        if (this.alerts.size === 0) {
            container.innerHTML = '<p class="loading">No active alerts</p>';
            return;
        }

        const sortedAlerts = Array.from(this.alerts.values())
            .sort((a, b) => {
                const severityOrder = { Critical: 0, Warning: 1, Info: 2 };
                return severityOrder[a.severity] - severityOrder[b.severity];
            });

        container.innerHTML = sortedAlerts
            .map(alert => `
                <div class="alert-item ${alert.severity.toLowerCase()}">
                    <div class="alert-header">
                        <div class="alert-title">${alert.title}</div>
                        <div class="alert-time">${new Date(alert.createdAt).toLocaleTimeString()}</div>
                    </div>
                    <div class="alert-message">${alert.message}</div>
                    ${alert.details ? `<div class="alert-details">${alert.details}</div>` : ''}
                </div>
            `).join('');
    }

    updateAlertCounts() {
        const critical = Array.from(this.alerts.values()).filter(a => a.severity === 'Critical').length;
        const warning = Array.from(this.alerts.values()).filter(a => a.severity === 'Warning').length;

        document.getElementById('criticalCount').textContent = critical;
        document.getElementById('warningCount').textContent = warning;
    }

    showNotification(alert) {
        if ('Notification' in window && Notification.permission === 'granted') {
            new Notification('NexusRoute Alert', {
                body: alert.message,
                icon: '/favicon.ico',
                tag: alert.id
            });
        }
    }
}

if ('Notification' in window && Notification.permission === 'default') {
    Notification.requestPermission();
}
