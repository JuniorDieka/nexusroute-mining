export class DashboardManager {
    constructor() {
        this.assets = new Map();
        this.convoys = new Map();
        this.production = null;
    }

    async loadAssets() {
        try {
            const response = await fetch('/api/assets');
            const assets = await response.json();
            
            assets.forEach(asset => this.assets.set(asset.id, asset));
            this.renderAssets();
            this.updateStats();
        } catch (error) {
            console.error('Error loading assets:', error);
        }
    }

    async loadConvoys() {
        try {
            const response = await fetch('/api/convoys/active');
            const convoys = await response.json();
            
            convoys.forEach(convoy => this.convoys.set(convoy.id, convoy));
            this.renderConvoys();
            this.updateStats();
        } catch (error) {
            console.error('Error loading convoys:', error);
        }
    }

    async loadProduction() {
        try {
            const response = await fetch('/api/production/summary');
            this.production = await response.json();
            this.renderProduction();
        } catch (error) {
            console.error('Error loading production:', error);
        }
    }

    renderAssets() {
        const tbody = document.getElementById('assetsTableBody');
        const positionsContainer = document.getElementById('assetPositions');
        
        if (this.assets.size === 0) {
            tbody.innerHTML = '<tr><td colspan="6" class="loading">No assets found</td></tr>';
            return;
        }

        tbody.innerHTML = Array.from(this.assets.values())
            .map(asset => `
                <tr>
                    <td><strong>${asset.assetCode}</strong></td>
                    <td>${asset.name}</td>
                    <td>${asset.type}</td>
                    <td><span class="status-badge ${asset.status.toLowerCase()}">${asset.status}</span></td>
                    <td>${asset.currentLocation || 'Unknown'}</td>
                    <td>${asset.lastTelemetryTime ? new Date(asset.lastTelemetryTime).toLocaleTimeString() : 'N/A'}</td>
                </tr>
            `).join('');

        positionsContainer.innerHTML = Array.from(this.assets.values())
            .filter(asset => asset.currentLatitude && asset.currentLongitude)
            .map(asset => `
                <div class="asset-position">
                    <div class="asset-position-header">${asset.assetCode}</div>
                    <div class="asset-position-coords">
                        ${asset.currentLatitude.toFixed(4)}, ${asset.currentLongitude.toFixed(4)}
                    </div>
                </div>
            `).join('');
    }

    renderConvoys() {
        const tbody = document.getElementById('convoysTableBody');
        
        if (this.convoys.size === 0) {
            tbody.innerHTML = '<tr><td colspan="6" class="loading">No active convoys</td></tr>';
            return;
        }

        tbody.innerHTML = Array.from(this.convoys.values())
            .map(convoy => `
                <tr>
                    <td><strong>${convoy.convoyCode}</strong></td>
                    <td>${convoy.routeName}</td>
                    <td>${convoy.leadAssetCode}</td>
                    <td>${convoy.cargoType}</td>
                    <td><span class="status-badge">${convoy.status}</span></td>
                    <td>${new Date(convoy.scheduledDepartureTime).toLocaleString()}</td>
                </tr>
            `).join('');
    }

    renderProduction() {
        if (!this.production) return;

        document.getElementById('totalTonnage').textContent = `${this.production.totalTonnage.toFixed(0)}t`;
        document.getElementById('averageGrade').textContent = `${this.production.averageGrade.toFixed(2)} g/t`;
        document.getElementById('targetAchievement').textContent = `${this.production.achievementPercentage.toFixed(1)}%`;
    }

    updateStats() {
        const activeAssets = Array.from(this.assets.values()).filter(a => a.isActive).length;
        const activeConvoys = this.convoys.size;

        document.getElementById('activeAssetsCount').textContent = activeAssets;
        document.getElementById('activeConvoysCount').textContent = activeConvoys;
        
        if (this.production) {
            document.getElementById('todayProduction').textContent = `${this.production.totalTonnage.toFixed(0)}t`;
            document.getElementById('weeklyProgress').textContent = `${this.production.achievementPercentage.toFixed(1)}%`;
        }
    }

    updateTelemetry(telemetry) {
        console.log('Telemetry update:', telemetry);
    }

    updateAssetStatus(asset) {
        this.assets.set(asset.id, asset);
        this.renderAssets();
        this.updateStats();
    }

    updateConvoy(convoy) {
        this.convoys.set(convoy.id, convoy);
        this.renderConvoys();
        this.updateStats();
    }

    updateProduction(production) {
        this.production = production;
        this.renderProduction();
        this.updateStats();
    }
}
