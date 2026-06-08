# Screenshots Folder

## 📸 How to Add Screenshots

Place your application screenshots in this folder with the following naming convention:

### Naming Convention

1. **01-dashboard-overview.png** - Main dashboard with metrics
2. **02-fleet-status.png** - Fleet status table view
3. **03-production-summary.png** - Production metrics
4. **04-alerts.png** - Alerts system view
5. **05-convoys.png** - Convoy management view

### Screenshot Guidelines

**Resolution**: Recommended 1920x1080 or higher  
**Format**: PNG (preferred) or JPG  
**Quality**: High quality, clear text  
**Content**: Show realistic data, avoid sensitive information

### Current Screenshots

Based on the uploaded images, you should have:

✅ **Dashboard Overview** - Shows 38 active assets, 5850t production, 77% weekly target  
✅ **Fleet Status** - Complete equipment list with locations (Mt. Mwendamboko, Muviringu, Kakula, Namoya Summit pits)  
✅ **Production Summary** - 5850t total, 1.88 g/t grade, 77% achievement  
✅ **Alerts** - No active alerts display  
✅ **Convoys** - No active convoys display  

### What Each Screenshot Should Show

#### 01-dashboard-overview.png
- Active Assets count (38)
- Active Convoys count (0)
- Today's Production (5850t)
- Weekly Target (77.0%)
- Live Asset Positions grid
- Asset codes with GPS coordinates

#### 02-fleet-status.png
- Complete fleet table
- Asset codes (DR-DP1500i-01, DT-777D-01, etc.)
- Equipment names
- Types (DrillRig, HaulTruck, Excavator)
- Status badges (IDLE, HAULING, LOADING)
- Locations showing 4 Namoya pits
- Last update timestamps

#### 03-production-summary.png
- Total Tonnage: 5850t
- Average Grade: 1.88 g/t
- Target Achievement: 77.0%
- Clean production metrics display

#### 04-alerts.png
- Active Alerts section
- 0 Critical alerts
- 0 Warning alerts
- "No active alerts" message
- Alert severity indicators

#### 05-convoys.png
- Active Convoys table
- Column headers (Convoy Code, Route, Lead Asset, Cargo, Status, Departure)
- "No active convoys" message
- Clean table layout

---

## 📋 Screenshot Checklist

When capturing screenshots, ensure:

- [ ] Application is running at http://localhost:5000
- [ ] All 38 assets are loaded and visible
- [ ] Production data shows 5850t
- [ ] Weekly target shows 77.0%
- [ ] Fleet locations show the 4 Namoya pits
- [ ] Status badges are clearly visible
- [ ] No sensitive or real data is shown
- [ ] Browser UI is minimal or cropped
- [ ] Text is sharp and readable
- [ ] Colors are accurate

---

## 🎯 Purpose

These screenshots help non-technical users:
- Understand the application without installing it
- See the user interface and features
- Visualize the fleet management capabilities
- Understand the data being tracked
- Make informed decisions about using the system

---

## 📝 Notes

- Screenshots should be updated when major UI changes occur
- Keep file sizes reasonable (< 2MB each)
- Use descriptive filenames
- Include captions in the main docs/README.md
- Ensure screenshots reflect the current version

---

**Folder Structure:**
```
docs/
├── README.md (main documentation with screenshot descriptions)
└── screenshots/
    ├── README.md (this file)
    ├── 01-dashboard-overview.png
    ├── 02-fleet-status.png
    ├── 03-production-summary.png
    ├── 04-alerts.png
    └── 05-convoys.png
```

---

**Last Updated**: June 8, 2026
